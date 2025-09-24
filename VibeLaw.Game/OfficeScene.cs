using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace VibeLaw.Game;

public class OfficeScene : Scene
{
    private readonly string _playerName;
    private SpriteBatch _spriteBatch = default!;
    private SpriteFont _font = default!;

    public OfficeScene(Game1 game, string playerName) : base(game)
    {
        _playerName = playerName;
    }

    public override void Load()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _font = Game.Content.Load<SpriteFont>("default");
    }

    public override void Update(GameTime gameTime)
    {
        // Later: add office interactions, NPCs, tasks, etc.
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, $"Welcome to the Office, {_playerName}!", new Vector2(20,20), Color.White);
        _spriteBatch.End();

        Console.WriteLine($"[OfficeScene] Running as {_playerName}");
    }
}
