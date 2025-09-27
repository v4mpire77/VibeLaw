using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace VibeLaw.Game;

public class Game1 : Microsoft.Xna.Framework.Game
{
<<<<<<< HEAD:VibeLaw.Game/Game1.cs
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = default!;
    private SpriteFont _font = default!;

    public SceneManager SceneManager { get; }
=======
    private const int TileSize = 64;
    private const int GridWidth = 10;
    private const int GridHeight = 8;
    private const float WalkAnimationHoldSeconds = 0.35f;

    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _tileTexture;
    private Texture2D _omarTexture;
    private SpriteAnimator _omarAnimator;
    private SpriteEffects _spriteEffects = SpriteEffects.None;
    private Point _playerTile = new(0, 0);
    private KeyboardState _previousKeyboardState;
    private float _walkBlendTimer;
    private bool _talkHeld;
>>>>>>> 4ede010 (Hook up Omar sprite sheet and animator):Game1.cs

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content/bin/DesktopGL";
        IsMouseVisible = true;
<<<<<<< HEAD:VibeLaw.Game/Game1.cs

        SceneManager = new SceneManager(this);
    }

    protected override void Initialize()
    {
        base.Initialize();
        SceneManager.ChangeScene(new CharacterSelectScene(this));
=======

        _graphics.PreferredBackBufferWidth = GridWidth * TileSize;
        _graphics.PreferredBackBufferHeight = GridHeight * TileSize;
        _graphics.ApplyChanges();
>>>>>>> 4ede010 (Hook up Omar sprite sheet and animator):Game1.cs
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
<<<<<<< HEAD:VibeLaw.Game/Game1.cs
        _font = Content.Load<SpriteFont>("default");
=======

        _tileTexture = new Texture2D(GraphicsDevice, 1, 1);
        _tileTexture.SetData(new[] { Color.DimGray });

        var spriteSheetPath = Path.Combine(AppContext.BaseDirectory, Content.RootDirectory, "Characters", "Omar", "Omar_Spritesheet.json");
        var definition = LoadSpriteSheetDefinition(spriteSheetPath);

        _omarTexture = Content.Load<Texture2D>("Characters/Omar/Omar_Spritesheet");
        _omarAnimator = new SpriteAnimator(_omarTexture, definition);
>>>>>>> 4ede010 (Hook up Omar sprite sheet and animator):Game1.cs
    }

    protected override void Update(GameTime gameTime)
    {
<<<<<<< HEAD:VibeLaw.Game/Game1.cs
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        SceneManager.Update(gameTime);
=======
        var keyboard = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
            Exit();

        var moved = false;

        if (WasKeyPressed(keyboard, Keys.W) && _playerTile.Y > 0)
        {
            _playerTile.Y--;
            moved = true;
        }

        if (WasKeyPressed(keyboard, Keys.S) && _playerTile.Y < GridHeight - 1)
        {
            _playerTile.Y++;
            moved = true;
        }

        if (WasKeyPressed(keyboard, Keys.A) && _playerTile.X > 0)
        {
            _playerTile.X--;
            _spriteEffects = SpriteEffects.FlipHorizontally;
            moved = true;
        }

        if (WasKeyPressed(keyboard, Keys.D) && _playerTile.X < GridWidth - 1)
        {
            _playerTile.X++;
            _spriteEffects = SpriteEffects.None;
            moved = true;
        }

        if (keyboard.IsKeyDown(Keys.Space))
        {
            if (!_talkHeld)
            {
                _talkHeld = true;
                _omarAnimator?.Play("talk", restart: true);
            }
        }
        else if (_talkHeld)
        {
            _talkHeld = false;
            if (_omarAnimator != null)
            {
                var next = _walkBlendTimer > 0f ? "walk" : "idle";
                _omarAnimator.Play(next, restart: true);
            }
        }

        if (!_talkHeld && moved)
        {
            _walkBlendTimer = WalkAnimationHoldSeconds;
            _omarAnimator?.Play("walk", restart: true);
        }

        if (!_talkHeld)
        {
            if (_walkBlendTimer > 0f)
            {
                _walkBlendTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_walkBlendTimer <= 0f)
                {
                    _omarAnimator?.Play("idle");
                }
            }
            else
            {
                _omarAnimator?.Play("idle");
            }
        }

        _omarAnimator?.Update(gameTime);
        _previousKeyboardState = keyboard;

>>>>>>> 4ede010 (Hook up Omar sprite sheet and animator):Game1.cs
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
<<<<<<< HEAD:VibeLaw.Game/Game1.cs
        GraphicsDevice.Clear(Color.CornflowerBlue);
        SceneManager.Draw(gameTime);
=======
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, depthStencilState: null, rasterizerState: null);

        for (var y = 0; y < GridHeight; y++)
        {
            for (var x = 0; x < GridWidth; x++)
            {
                var color = (x + y) % 2 == 0 ? Color.DarkSlateGray : Color.DarkSlateBlue;
                var destination = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
                _spriteBatch.Draw(_tileTexture, destination, color);
            }
        }

        if (_omarAnimator != null)
        {
            var position = new Vector2(_playerTile.X * TileSize + TileSize / 2f, _playerTile.Y * TileSize + TileSize);
            _spriteBatch.Draw(_omarAnimator.Texture, position, _omarAnimator.SourceRect, Color.White, 0f, _omarAnimator.Origin, 1f, _spriteEffects, 0f);
        }

        _spriteBatch.End();

>>>>>>> 4ede010 (Hook up Omar sprite sheet and animator):Game1.cs
        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        _tileTexture?.Dispose();
        base.UnloadContent();
    }

    private bool WasKeyPressed(KeyboardState current, Keys key) =>
        current.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);

    private static SpriteSheetDefinition LoadSpriteSheetDefinition(string path)
    {
        using var stream = File.OpenRead(path);
        return JsonSerializer.Deserialize<SpriteSheetDefinition>(stream) ?? throw new InvalidOperationException($"Failed to load sprite sheet metadata from '{path}'.");
    }

    private sealed class SpriteAnimator
    {
        private readonly Texture2D _texture;
        private readonly Dictionary<string, AnimationClip> _clips;
        private AnimationClip _currentClip;
        private string _currentName = string.Empty;
        private int _frameIndex;
        private float _timer;

        public SpriteAnimator(Texture2D texture, SpriteSheetDefinition definition)
        {
            _texture = texture;
            Origin = new Vector2(definition.Pivot.X, definition.Pivot.Y);
            _clips = new Dictionary<string, AnimationClip>(StringComparer.OrdinalIgnoreCase);

            foreach (var pair in definition.Animations)
            {
                _clips[pair.Key] = new AnimationClip(definition, pair.Value);
            }

            Play("idle", restart: true);
        }

        public Texture2D Texture => _texture;
        public Rectangle SourceRect => _currentClip.Frames[_frameIndex];
        public Vector2 Origin { get; }
        public string CurrentClip => _currentName;

        public void Play(string name, bool restart = false)
        {
            if (!_clips.TryGetValue(name, out var clip))
                throw new ArgumentException($"Animation '{name}' was not found in the sprite sheet.", nameof(name));

            if (!restart && _currentName == name)
                return;

            _currentClip = clip;
            _currentName = name;
            _frameIndex = 0;
            _timer = 0f;
        }

        public void Update(GameTime gameTime)
        {
            if (_currentClip == null)
                return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            var frameDuration = _currentClip.FrameTimeSeconds;

            if (frameDuration <= 0f)
                return;

            while (_timer >= frameDuration)
            {
                _timer -= frameDuration;
                _frameIndex++;

                if (_frameIndex >= _currentClip.Frames.Length)
                {
                    if (_currentClip.Loop)
                    {
                        _frameIndex = 0;
                    }
                    else
                    {
                        _frameIndex = _currentClip.Frames.Length - 1;
                        break;
                    }
                }
            }
        }

        private sealed class AnimationClip
        {
            public AnimationClip(SpriteSheetDefinition sheet, SpriteSheetDefinition.AnimationDefinition definition)
            {
                Frames = new Rectangle[definition.Frames.Length];
                for (var i = 0; i < definition.Frames.Length; i++)
                {
                    Frames[i] = sheet.Frames[definition.Frames[i]].ToRectangle();
                }

                FrameTimeSeconds = definition.FrameTimeMs / 1000f;
                Loop = definition.Loop;
            }

            public Rectangle[] Frames { get; }
            public float FrameTimeSeconds { get; }
            public bool Loop { get; }
        }
    }

    private sealed class SpriteSheetDefinition
    {
        [JsonPropertyName("texture")]
        public string Texture { get; set; }

        [JsonPropertyName("pivot")]
        public PivotDefinition Pivot { get; set; }

        [JsonPropertyName("frames")]
        public FrameDefinition[] Frames { get; set; }

        [JsonPropertyName("animations")]
        public Dictionary<string, AnimationDefinition> Animations { get; set; }

        public sealed class PivotDefinition
        {
            [JsonPropertyName("x")]
            public int X { get; set; }

            [JsonPropertyName("y")]
            public int Y { get; set; }
        }

        public sealed class FrameDefinition
        {
            [JsonPropertyName("i")]
            public int Index { get; set; }

            [JsonPropertyName("x")]
            public int X { get; set; }

            [JsonPropertyName("y")]
            public int Y { get; set; }

            [JsonPropertyName("w")]
            public int Width { get; set; }

            [JsonPropertyName("h")]
            public int Height { get; set; }

            public Rectangle ToRectangle() => new Rectangle(X, Y, Width, Height);
        }

        public sealed class AnimationDefinition
        {
            [JsonPropertyName("frames")]
            public int[] Frames { get; set; }

            [JsonPropertyName("frame_time_ms")]
            public int FrameTimeMs { get; set; }

            [JsonPropertyName("loop")]
            public bool Loop { get; set; }
        }
    }
}
