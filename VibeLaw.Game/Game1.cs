using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VibeLaw.Game;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private const int DefaultWidth = 640;
    private const int DefaultHeight = 512;

    private readonly GraphicsDeviceManager _graphics;

    public SceneManager SceneManager { get; }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = DefaultWidth;
        _graphics.PreferredBackBufferHeight = DefaultHeight;
        _graphics.ApplyChanges();

        SceneManager = new SceneManager(this);
    }

    protected override void Initialize()
    {
        base.Initialize();
        SceneManager.ChangeScene(new HomeScene(this));
    }

    protected override void LoadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboard = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.F10))
        {
            Exit();
        }

        SceneManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        SceneManager.Draw(gameTime);
    }
}



