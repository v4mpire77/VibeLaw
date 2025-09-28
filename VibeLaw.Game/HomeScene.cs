using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VibeLaw.Game;

public class HomeScene : Scene
{
    private readonly string[] _menuItems = { "Start Game", "Quit" };

    private SpriteBatch _spriteBatch = default!;
    private SpriteFont _font = default!;
    private Texture2D _pixel = default!;
    private KeyboardState _previousState;
    private int _selectedIndex;

    private readonly Color _backgroundColor = new(12, 16, 28);
    private readonly Color _panelColor = new(36, 48, 74);
    private readonly Color _accentColor = new(255, 214, 110);

    public HomeScene(Game1 game) : base(game) { }

    public override void Load()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _font = Game.Content.Load<SpriteFont>("default");
        _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _selectedIndex = 0;
        _previousState = Keyboard.GetState();
    }

    public override void Update(GameTime gameTime)
    {
        var state = Keyboard.GetState();

        if (IsNewPress(state, Keys.Down) || IsNewPress(state, Keys.S))
        {
            _selectedIndex = Math.Min(_menuItems.Length - 1, _selectedIndex + 1);
        }

        if (IsNewPress(state, Keys.Up) || IsNewPress(state, Keys.W))
        {
            _selectedIndex = Math.Max(0, _selectedIndex - 1);
        }

        if (IsNewPress(state, Keys.Enter) || IsNewPress(state, Keys.Space))
        {
            ActivateSelection();
        }

        if (IsNewPress(state, Keys.Escape))
        {
            Game.Exit();
        }

        _previousState = state;
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(_backgroundColor);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

        DrawTitle();
        DrawMenu();
        DrawHint();

        _spriteBatch.End();
    }

    private void DrawTitle()
    {
        const string title = "VibeLaw";
        const string subtitle = "Interactive Advocacy Prototype";

        var viewport = Game.GraphicsDevice.Viewport;
        var titleSize = _font.MeasureString(title);
        var subtitleSize = _font.MeasureString(subtitle);

        var titlePosition = new Vector2(viewport.Width / 2f - titleSize.X / 2f, viewport.Height * 0.18f);
        var subtitlePosition = titlePosition + new Vector2((titleSize.X - subtitleSize.X) / 2f, titleSize.Y + 10f);

        _spriteBatch.DrawString(_font, title, titlePosition, Color.White);
        _spriteBatch.DrawString(_font, subtitle, subtitlePosition, Color.LightGray * 0.85f);
    }

    private void DrawMenu()
    {
        var viewport = Game.GraphicsDevice.Viewport;
        float spacing = 52f;
        float totalHeight = spacing * _menuItems.Length;
        float startY = viewport.Height * 0.45f - totalHeight / 2f;

        for (int i = 0; i < _menuItems.Length; i++)
        {
            bool selected = i == _selectedIndex;
            string option = _menuItems[i];
            Vector2 size = _font.MeasureString(option);
            float x = viewport.Width / 2f - size.X / 2f;
            float y = startY + spacing * i;

            if (selected)
            {
                var rect = new Rectangle((int)(x - 30f), (int)(y - 12f), (int)(size.X + 60f), (int)(size.Y + 24f));
                _spriteBatch.Draw(_pixel, rect, _panelColor * 0.9f);
                _spriteBatch.Draw(_pixel, new Rectangle(rect.X - 3, rect.Y - 3, rect.Width + 6, rect.Height + 6), _accentColor * 0.35f);
            }

            _spriteBatch.DrawString(_font, option, new Vector2(x, y), selected ? _accentColor : Color.LightGray);
        }
    }

    private void DrawHint()
    {
        const string primaryHint = "Enter / Space to confirm";
        const string secondaryHint = "Arrow Keys or WASD to navigate";

        var viewport = Game.GraphicsDevice.Viewport;
        var primarySize = _font.MeasureString(primaryHint);
        var secondarySize = _font.MeasureString(secondaryHint);

        var primaryPosition = new Vector2(viewport.Width / 2f - primarySize.X / 2f, viewport.Height * 0.72f);
        var secondaryPosition = primaryPosition + new Vector2((primarySize.X - secondarySize.X) / 2f, primarySize.Y + 6f);

        _spriteBatch.DrawString(_font, primaryHint, primaryPosition, Color.LightGray * 0.9f);
        _spriteBatch.DrawString(_font, secondaryHint, secondaryPosition, Color.LightGray * 0.7f);
    }

    private bool IsNewPress(KeyboardState state, Keys key)
        => state.IsKeyDown(key) && !_previousState.IsKeyDown(key);

    private void ActivateSelection()
    {
        switch (_selectedIndex)
        {
            case 0:
                Game.SceneManager.ChangeScene(new CharacterSelectScene(Game));
                break;
            case 1:
                Game.Exit();
                break;
        }
    }
}
