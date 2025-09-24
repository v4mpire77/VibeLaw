namespace VibeLaw.Game;

public abstract class Scene
{
    protected Game1 Game { get; }

    protected Scene(Game1 game)
    {
        Game = game;
    }

    public virtual void Load() { }
    public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime) { }
    public virtual void Draw(Microsoft.Xna.Framework.GameTime gameTime) { }
}
