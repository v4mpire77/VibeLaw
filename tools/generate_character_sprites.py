#!/usr/bin/env python3
"""Generate placeholder 8-bit sprite sheets for Omar and Maham."""
from __future__ import annotations

import os
import struct
import zlib
from typing import Dict, Tuple

WIDTH = 64  # 2 frames horizontally (32px each)
HEIGHT = 128  # 4 directions vertically (32px each)
TILE_SIZE = 32
DIRECTIONS = ["down", "up", "left", "right"]

Color = Tuple[int, int, int, int]


def create_canvas(width: int, height: int) -> list[bytearray]:
    return [bytearray([0] * width * 4) for _ in range(height)]


def set_pixel(canvas: list[bytearray], x: int, y: int, color: Color) -> None:
    if not (0 <= x < WIDTH and 0 <= y < HEIGHT):
        return
    row = canvas[y]
    idx = x * 4
    row[idx : idx + 4] = bytes(color)


def fill_rect(canvas: list[bytearray], x0: int, y0: int, x1: int, y1: int, color: Color) -> None:
    for y in range(y0, y1):
        for x in range(x0, x1):
            set_pixel(canvas, x, y, color)


def draw_eye(canvas: list[bytearray], x: int, y: int) -> None:
    white: Color = (250, 250, 250, 255)
    pupil: Color = (30, 30, 30, 255)
    set_pixel(canvas, x, y, white)
    set_pixel(canvas, x + 1, y, white)
    set_pixel(canvas, x + 1, y, pupil)


def draw_character_tile(
    canvas: list[bytearray],
    origin_x: int,
    origin_y: int,
    palette: Dict[str, Color],
    facing: str,
    frame: int,
) -> None:
    skin = palette["skin"]
    hair = palette["hair"]
    outfit = palette["outfit"]
    pants = palette["pants"]
    accent = palette["accent"]
    outline = (20, 20, 20, 255)

    # Head + hair
    head_top = origin_y + 4
    head_bottom = head_top + 10
    head_left = origin_x + 10
    head_right = origin_x + 22

    fill_rect(canvas, head_left, head_top, head_right, head_top + 4, hair)
    fill_rect(canvas, head_left, head_top + 4, head_right, head_bottom, skin)

    # Body core
    body_top = head_bottom
    body_bottom = body_top + 9
    body_left = origin_x + 12
    body_right = origin_x + 20
    fill_rect(canvas, body_left, body_top, body_right, body_bottom, outfit)

    # Belt / accent
    fill_rect(canvas, body_left, body_bottom, body_right, body_bottom + 2, accent)

    # Legs base
    leg_top = body_bottom + 2
    leg_bottom = leg_top + 6

    if facing in ("down", "up"):
        # Both legs visible; alternate animation by offsetting feet.
        left_leg_x = origin_x + 12
        right_leg_x = origin_x + 16
        left_offset = 0 if frame == 0 else 2
        right_offset = 2 if frame == 0 else 0
        fill_rect(canvas, left_leg_x, leg_top + left_offset, left_leg_x + 3, leg_bottom, pants)
        fill_rect(canvas, right_leg_x, leg_top + right_offset, right_leg_x + 3, leg_bottom, pants)
    elif facing == "left":
        front_leg_x = origin_x + 12
        back_leg_x = origin_x + 17
        offset = 2 if frame == 0 else 0
        fill_rect(canvas, back_leg_x, leg_top + offset, back_leg_x + 3, leg_bottom, pants)
        fill_rect(canvas, front_leg_x, leg_top + (0 if frame == 0 else 2), front_leg_x + 3, leg_bottom, pants)
    else:  # right
        front_leg_x = origin_x + 17
        back_leg_x = origin_x + 12
        offset = 2 if frame == 0 else 0
        fill_rect(canvas, back_leg_x, leg_top + offset, back_leg_x + 3, leg_bottom, pants)
        fill_rect(canvas, front_leg_x, leg_top + (0 if frame == 0 else 2), front_leg_x + 3, leg_bottom, pants)

    # Arms / sleeves differ by facing for a hint of depth.
    sleeve_top = body_top + 1
    sleeve_bottom = body_top + 6
    if facing in ("down", "up"):
        fill_rect(canvas, body_left - 3, sleeve_top, body_left, sleeve_bottom, outfit)
        fill_rect(canvas, body_right, sleeve_top, body_right + 3, sleeve_bottom, outfit)
    elif facing == "left":
        fill_rect(canvas, body_right - 2, sleeve_top, body_right + 2, sleeve_bottom, outfit)
    else:
        fill_rect(canvas, body_left - 2, sleeve_top, body_left + 2, sleeve_bottom, outfit)

    # Simple outline for readability
    for x in range(origin_x + 9, origin_x + 23):
        set_pixel(canvas, x, head_top - 1, outline)
        set_pixel(canvas, x, leg_bottom + 1, outline)
    for y in range(head_top, leg_bottom + 1):
        set_pixel(canvas, origin_x + 9, y, outline)
        set_pixel(canvas, origin_x + 22, y, outline)

    # Facial details per facing direction
    if facing == "down":
        eye_y = head_top + 6
        left_eye_x = origin_x + 12
        right_eye_x = origin_x + 17
        draw_eye(canvas, left_eye_x, eye_y)
        draw_eye(canvas, right_eye_x, eye_y)
        mouth_color: Color = (180, 80, 90, 255)
        set_pixel(canvas, origin_x + 15, eye_y + 4, mouth_color)
    elif facing == "up":
        # hair band accent for visibility from behind
        fill_rect(canvas, head_left, head_top + 2, head_right, head_top + 3, accent)
    elif facing == "left":
        draw_eye(canvas, origin_x + 11, head_top + 6)
        fill_rect(canvas, head_left - 1, head_top + 2, head_left + 1, head_top + 6, hair)
    else:  # right
        draw_eye(canvas, origin_x + 18, head_top + 6)
        fill_rect(canvas, head_right, head_top + 2, head_right + 2, head_top + 6, hair)

    # Simple shadow to ground the sprite
    shadow_color: Color = (0, 0, 0, 60)
    fill_rect(canvas, origin_x + 10, origin_y + 28, origin_x + 22, origin_y + 30, shadow_color)


def emit_png(path: str, canvas: list[bytearray]) -> None:
    raw = bytearray()
    for row in canvas:
        raw.append(0)
        raw.extend(row)

    compressed = zlib.compress(bytes(raw), 9)

    def chunk(tag: bytes, data: bytes) -> bytes:
        return (
            struct.pack(">I", len(data))
            + tag
            + data
            + struct.pack(">I", zlib.crc32(tag + data) & 0xFFFFFFFF)
        )

    ihdr = struct.pack(">IIBBBBB", WIDTH, HEIGHT, 8, 6, 0, 0, 0)
    with open(path, "wb") as fh:
        fh.write(b"\x89PNG\r\n\x1a\n")
        fh.write(chunk(b"IHDR", ihdr))
        fh.write(chunk(b"IDAT", compressed))
        fh.write(chunk(b"IEND", b""))


CHARACTERS = [
    {
        "name": "Omar",
        "palette": {
            "skin": (239, 208, 181, 255),
            "hair": (60, 42, 30, 255),
            "outfit": (26, 158, 141, 255),
            "pants": (33, 70, 120, 255),
            "accent": (245, 210, 65, 255),
        },
    },
    {
        "name": "Maham",
        "palette": {
            "skin": (247, 221, 198, 255),
            "hair": (110, 45, 90, 255),
            "outfit": (178, 102, 214, 255),
            "pants": (92, 60, 130, 255),
            "accent": (255, 140, 180, 255),
        },
    },
]


def main() -> None:
    output_root = os.path.join("VibeLaw.Game", "Content", "Characters")
    os.makedirs(output_root, exist_ok=True)

    for character in CHARACTERS:
        canvas = create_canvas(WIDTH, HEIGHT)
        for row, facing in enumerate(DIRECTIONS):
            for col in range(2):
                origin_x = col * TILE_SIZE
                origin_y = row * TILE_SIZE
                draw_character_tile(
                    canvas,
                    origin_x,
                    origin_y,
                    character["palette"],
                    facing,
                    frame=col,
                )
        emit_png(os.path.join(output_root, f"{character['name']}.png"), canvas)
        print(f"Wrote sprite sheet for {character['name']}")


if __name__ == "__main__":
    main()
