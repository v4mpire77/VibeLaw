using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VibeLaw.Game;

public class CharacterSelectScene : Scene
{
    private const float ConfirmFlashDuration = 0.25f;
    private const int PortraitScale = 3;
    private static readonly Point PortraitSize = new(32, 32);

    private readonly string[] _choices = { "Omar", "Maham" };

    private SpriteBatch _spriteBatch = default!;
    private SpriteFont _font = default!;
    private Texture2D _omarSprite = default!;
    private Texture2D _mahamSprite = default!;
    private Texture2D _pixel = default!;
    private Rectangle[] _choiceAreas = Array.Empty<Rectangle>();

    private int _choiceIndex;
    private KeyboardState _previousState;
    private bool _isConfirming;
    private float _confirmFlashTimer;
    private string _pendingSelection = string.Empty;

    public CharacterSelectScene(Game1 game) : base(game) { }

    public override void Load()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _font = Game.Content.Load<SpriteFont>("default");
        _omarSprite = Game.Content.Load<Texture2D>("Characters/Omar");
        _mahamSprite = Game.Content.Load<Texture2D>("Characters/Maham");

        _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        BuildChoiceAreas();
        _previousState = Keyboard.GetState();
    }

    private void BuildChoiceAreas()
    {
        int spacing = 32;
        int panelWidth = PortraitSize.X * PortraitScale + 32;
        int panelHeight = PortraitSize.Y * PortraitScale + 60;
        int totalWidth = panelWidth * _choices.Length + spacing * (_choices.Length - 1);
        int startX = (Game.GraphicsDevice.Viewport.Width - totalWidth) / 2;
        int startY = 140;

        _choiceAreas = new Rectangle[_choices.Length];
        for (int i = 0; i < _choices.Length; i++)
        {
            int x = startX + i * (panelWidth + spacing);
            _choiceAreas[i] = new Rectangle(x, startY, panelWidth, panelHeight);
        }
    }

    public override void Update(GameTime gameTime)
    {
        var state = Keyboard.GetState();

        if (_isConfirming)
        {
            _confirmFlashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_confirmFlashTimer <= 0f && !string.IsNullOrEmpty(_pendingSelection))
            {
                Game.SceneManager.ChangeScene(new PrototypePlayScene(Game, _pendingSelection));
            }
        }
        else
        {
            if (IsNewPress(state, Keys.Left) || IsNewPress(state, Keys.A) || IsNewPress(state, Keys.Up) || IsNewPress(state, Keys.W))
            {
                _choiceIndex = Math.Max(0, _choiceIndex - 1);
            }

            if (IsNewPress(state, Keys.Right) || IsNewPress(state, Keys.D) || IsNewPress(state, Keys.Down) || IsNewPress(state, Keys.S))
            {
                _choiceIndex = Math.Min(_choices.Length - 1, _choiceIndex + 1);
            }

            if (IsNewPress(state, Keys.Enter) || IsNewPress(state, Keys.Space))
            {
                _pendingSelection = _choices[_choiceIndex];
                _confirmFlashTimer = ConfirmFlashDuration;
                _isConfirming = true;
                Console.WriteLine($"[CharacterSelect] Player confirmed {_pendingSelection}");
            }
        }

        _previousState = state;
    }

    private bool IsNewPress(KeyboardState state, Keys key)
        => state.IsKeyDown(key) && !_previousState.IsKeyDown(key);

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

        DrawHeading();
        DrawChoices();

        if (_isConfirming)
        {
            float alpha = MathHelper.Clamp(_confirmFlashTimer / ConfirmFlashDuration, 0f, 1f);
            _spriteBatch.Draw(_pixel, Game.GraphicsDevice.Viewport.Bounds, Color.White * (alpha * 0.6f));
        }

        _spriteBatch.End();
    }

    private void DrawHeading()
    {
        var titlePosition = new Vector2(60, 40);
        var subtitlePosition = titlePosition + new Vector2(0, 32);
        _spriteBatch.DrawString(_font, "Choose your Advocate", titlePosition, Color.White);
        _spriteBatch.DrawString(
            _font,
            "Use Arrow or WASD keys to navigate. Confirm with Enter or Space.",
            subtitlePosition,
            Color.LightGray
        );
    }

    private void DrawChoices()
    {
        for (int i = 0; i < _choices.Length; i++)
        {
            bool selected = i == _choiceIndex;
            Rectangle area = _choiceAreas[i];

            Color panel = selected ? new Color(70, 70, 110) : new Color(40, 40, 60);
            _spriteBatch.Draw(_pixel, area, panel * 0.9f);

            if (selected)
            {
                var border = new Rectangle(area.X - 4, area.Y - 4, area.Width + 8, area.Height + 8);
                _spriteBatch.Draw(_pixel, border, new Color(255, 220, 100) * 0.55f);
            }

            Texture2D portrait = i == 0 ? _omarSprite : _mahamSprite;
            var source = new Rectangle(0, 0, PortraitSize.X, PortraitSize.Y);
            var portraitPosition = new Vector2(
                area.X + (area.Width - PortraitSize.X * PortraitScale) / 2f,
                area.Y + 18
            );

            _spriteBatch.Draw(
                portrait,
                portraitPosition,
                source,
                Color.White,
                0f,
                Vector2.Zero,
                PortraitScale,
                SpriteEffects.None,
                0f
            );

            var nameText = _choices[i];
            var nameWidth = _font.MeasureString(nameText).X;
            var namePosition = new Vector2(area.X + area.Width / 2f - nameWidth / 2f, area.Bottom - 42);
            _spriteBatch.DrawString(_font, nameText, namePosition, selected ? Color.White : Color.LightGray);
        }
    }
}
