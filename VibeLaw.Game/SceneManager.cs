using System.Collections.Generic;

namespace VibeLaw.Game;

public class SceneManager
{
    private readonly Game1 _game;
    private Scene? _currentScene;

    public SceneManager(Game1 game)
    {
        _game = game;
    }

    public void ChangeScene(Scene newScene)
    {
        _currentScene?.Unload();
        _currentScene = newScene;
        _currentScene.Load();
    }

    public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        => _currentScene?.Update(gameTime);

    public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        => _currentScene?.Draw(gameTime);
}



