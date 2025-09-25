using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VibeLaw.Game;

public class PlayerCharacter
{
    private const float FrameDuration = 0.15f;
    private readonly Texture2D _spriteSheet;
    private readonly Point _frameSize;
    private readonly Rectangle _playBounds;
    private readonly float _speed;

    private readonly Rectangle[][] _frames;

    private Vector2 _position;
    private FacingDirection _facing = FacingDirection.Down;
    private int _currentFrame;
    private float _animationTimer;

    public PlayerCharacter(Texture2D spriteSheet, Vector2 startPosition, Rectangle playBounds, float speed = 110f)
    {
        _spriteSheet = spriteSheet;
        _playBounds = playBounds;
        _frameSize = new Point(spriteSheet.Width / 2, spriteSheet.Height / 4);
        _speed = speed;
        _position = startPosition;
        _frames = BuildFrameMap();
    }

    public void Update(GameTime gameTime, KeyboardState keyboardState)
    {
        Vector2 movement = ReadMovementInput(keyboardState);

        if (movement.LengthSquared() > 0f)
        {
            movement.Normalize();
            _position += movement * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateFacing(movement);
            AdvanceAnimation(gameTime);
        }
        else
        {
            _animationTimer = 0f;
            _currentFrame = 0;
        }

        ClampToBounds();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle source = _frames[(int)_facing][_currentFrame];
        spriteBatch.Draw(_spriteSheet, _position, source, Color.White);
    }

    public Vector2 Position => _position;

    public Point FrameSize => _frameSize;

    private Rectangle[][] BuildFrameMap()
    {
        var map = new Rectangle[4][];
        for (int row = 0; row < 4; row++)
        {
            map[row] = new Rectangle[2];
            for (int col = 0; col < 2; col++)
            {
                map[row][col] = new Rectangle(col * _frameSize.X, row * _frameSize.Y, _frameSize.X, _frameSize.Y);
            }
        }

        return map;
    }

    private static Vector2 ReadMovementInput(KeyboardState keyboardState)
    {
        Vector2 movement = Vector2.Zero;

        if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
        {
            movement.X -= 1f;
        }
        if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
        {
            movement.X += 1f;
        }
        if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
        {
            movement.Y -= 1f;
        }
        if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
        {
            movement.Y += 1f;
        }

        return movement;
    }

    private void UpdateFacing(Vector2 movement)
    {
        if (MathF.Abs(movement.X) > MathF.Abs(movement.Y))
        {
            _facing = movement.X > 0 ? FacingDirection.Right : FacingDirection.Left;
        }
        else if (movement.Y != 0)
        {
            _facing = movement.Y > 0 ? FacingDirection.Down : FacingDirection.Up;
        }
    }

    private void AdvanceAnimation(GameTime gameTime)
    {
        _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_animationTimer >= FrameDuration)
        {
            _animationTimer -= FrameDuration;
            _currentFrame = (_currentFrame + 1) % 2;
        }
    }

    private void ClampToBounds()
    {
        float minX = _playBounds.Left;
        float maxX = _playBounds.Right - _frameSize.X;
        float minY = _playBounds.Top;
        float maxY = _playBounds.Bottom - _frameSize.Y;

        _position.X = MathHelper.Clamp(_position.X, minX, maxX);
        _position.Y = MathHelper.Clamp(_position.Y, minY, maxY);
    }

    private enum FacingDirection
    {
        Down = 0,
        Up = 1,
        Left = 2,
        Right = 3
    }
}
