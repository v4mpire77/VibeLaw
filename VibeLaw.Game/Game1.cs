using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace VibeLaw.Game;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = default!;
    private SpriteFont _font = default!;

    public SceneManager SceneManager { get; }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content/bin/DesktopGL";
        IsMouseVisible = true;

        SceneManager = new SceneManager(this);
    }

    protected override void Initialize()
    {
        base.Initialize();
        SceneManager.ChangeScene(new CharacterSelectScene(this));
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("default");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        SceneManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        SceneManager.Draw(gameTime);
        base.Draw(gameTime);
    }
}
