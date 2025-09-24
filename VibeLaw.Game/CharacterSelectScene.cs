using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace VibeLaw.Game;

public class CharacterSelectScene : Scene
{
    private SpriteBatch _spriteBatch = default!;
    private SpriteFont _font = default!;
    private int _choiceIndex = 0; // 0 = Omar, 1 = Maham

    private readonly string[] _choices = { "Omar", "Maham" };

    public CharacterSelectScene(Game1 game) : base(game) { }

    public override void Load()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _font = Game.Content.Load<SpriteFont>("default");
    }

    public override void Update(GameTime gameTime)
    {
        var state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.Left)) _choiceIndex = 0;
        if (state.IsKeyDown(Keys.Right)) _choiceIndex = 1;

        if (state.IsKeyDown(Keys.Enter))
        {
            string chosen = _choices[_choiceIndex];
            Console.WriteLine($"[CharacterSelect] Player chose {chosen}");
            Game.SceneManager.ChangeScene(new OfficeScene(Game, chosen));
        }
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, "Select Your Character (← Omar | → Maham)", new Vector2(20,20), Color.White);

        for (int i = 0; i < _choices.Length; i++)
        {
            var color = (i == _choiceIndex) ? Color.Yellow : Color.Gray;
            _spriteBatch.DrawString(_font, _choices[i], new Vector2(40, 60 + i * 30), color);
        }

        _spriteBatch.End();
    }
}
