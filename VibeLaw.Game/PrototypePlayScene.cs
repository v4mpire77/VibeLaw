using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VibeLaw.Game;

public class PrototypePlayScene : Scene
{
    private readonly string _selectedCharacter;

    private SpriteBatch _spriteBatch = default!;
    private SpriteFont _font = default!;
    private Texture2D _pixel = default!;
    private PlayerCharacter _player = default!;
    private Rectangle _playArea;

    private readonly Color _backgroundColor = new(18, 24, 38);
    private readonly Color _arenaColor = new(45, 64, 96);
    private readonly Color _arenaBorderColor = new(255, 236, 160);

    public PrototypePlayScene(Game1 game, string selectedCharacter) : base(game)
    {
        _selectedCharacter = selectedCharacter;
    }

    public override void Load()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _font = Game.Content.Load<SpriteFont>("default");

        _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        Texture2D spriteSheet = Game.Content.Load<Texture2D>($"Characters/{_selectedCharacter}");
        Viewport viewport = Game.GraphicsDevice.Viewport;
        _playArea = BuildPlayArea(viewport);

        Vector2 start = new(
            _playArea.X + (_playArea.Width - spriteSheet.Width / 2f) / 2f,
            _playArea.Y + (_playArea.Height - spriteSheet.Height / 4f) / 2f
        );

        _player = new PlayerCharacter(spriteSheet, start, _playArea);
        Console.WriteLine($"[PrototypePlayScene] Loaded with {_selectedCharacter}");
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardState keyboard = Keyboard.GetState();
        _player.Update(gameTime, keyboard);
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(_backgroundColor);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        DrawArena();
        _player.Draw(_spriteBatch);
        DrawHud();
        _spriteBatch.End();
    }

    private Rectangle BuildPlayArea(Viewport viewport)
    {
        const int horizontalMargin = 100;
        const int verticalMargin = 140;
        int width = Math.Max(320, viewport.Width - horizontalMargin * 2);
        int height = Math.Max(240, viewport.Height - verticalMargin * 2);
        int x = (viewport.Width - width) / 2;
        int y = (viewport.Height - height) / 2 + 20;
        return new Rectangle(x, y, width, height);
    }

    private void DrawArena()
    {
        _spriteBatch.Draw(_pixel, _playArea, _arenaColor * 0.9f);

        // Border (top, bottom, left, right)
        _spriteBatch.Draw(_pixel, new Rectangle(_playArea.X - 4, _playArea.Y - 4, _playArea.Width + 8, 4), _arenaBorderColor);
        _spriteBatch.Draw(_pixel, new Rectangle(_playArea.X - 4, _playArea.Bottom, _playArea.Width + 8, 4), _arenaBorderColor);
        _spriteBatch.Draw(_pixel, new Rectangle(_playArea.X - 4, _playArea.Y, 4, _playArea.Height), _arenaBorderColor);
        _spriteBatch.Draw(_pixel, new Rectangle(_playArea.Right, _playArea.Y, 4, _playArea.Height), _arenaBorderColor);
    }

    private void DrawHud()
    {
        string header = $"{_selectedCharacter} Training Grounds";
        string movement = "Move: Arrow Keys / WASD";
        string confirm = "Boundary Test: Walk to the edge to see the clamp";
        string debounce = "Confirm scene: Enter/Space was debounced on selection";

        Vector2 headPos = new(60, 40);
        _spriteBatch.DrawString(_font, header, headPos, Color.White);
        _spriteBatch.DrawString(_font, movement, headPos + new Vector2(0, 32), Color.LightGray);
        _spriteBatch.DrawString(_font, confirm, headPos + new Vector2(0, 56), Color.LightGray * 0.85f);
        _spriteBatch.DrawString(_font, debounce, headPos + new Vector2(0, 80), Color.LightGray * 0.7f);
    }
}
