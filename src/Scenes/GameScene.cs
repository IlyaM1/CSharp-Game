using Dyhar.src.Control;
using Dyhar.src.Drawing;
using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Entities;
using Dyhar.src.Utils;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Dyhar.src.Scenes;

public class GameScene : Scene
{
    SpriteFont standardFont;

    Player player;
    GameControl control = new GameControl();
    Camera camera;

    int levelNumber = 1;
    Level.Level currentLevel;

    Texture2D _gridCube;

    public GameScene()
    {
        currentLevel = Level.Level.CreateLevelFromFile("Level1", control, out player);

        //this.IsFixedTimeStep = true;//false;
        //this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d); //60);
    }

    public override void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
        Player.sprite = content.Load<Texture2D>("player");
        EarthBlock.sprite = content.Load<Texture2D>("Earth2");
        Sword.sprite = content.Load<Texture2D>("Sword");
        Swordsman.sprite = content.Load<Texture2D>("Swordsman");
        _gridCube = content.Load<Texture2D>("GridCube1");

        standardFont = content.Load<SpriteFont>("galleryFont");

        camera = new Camera(graphics.Viewport, currentLevel.Width, currentLevel.Height);
        control.setCamera(camera);
    }

    public override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            IsDone = true;
            SceneToRun = typeof(MenuScene);
        }


        control.onUpdate(Mouse.GetState(), Keyboard.GetState());

        for (var i = 0; i < currentLevel.GameObjects.Count; i++)
        {
            var gameObject = currentLevel.GameObjects[i];
            if (TypesUtils.CanBeDownCasted<GameObject, MovingGameObject>(gameObject))
                currentLevel.Physics.Move((MovingGameObject)gameObject);

            if (TypesUtils.CanBeDownCasted<GameObject, Enemy>(gameObject))
            {
                var enemy = (Enemy)gameObject;
                if (enemy.IsOnPlayerScreen(camera))
                    enemy.onPlayerScreen(player);
            }

            foreach (var weaponUser in currentLevel.GetWeaponUsers())
                if (weaponUser.IsAttacking())
                    if (weaponUser.GetCurrentWeapon().CheckCollision(gameObject, weaponUser.GetDirection()))
                        gameObject.onAttacked(weaponUser.GetCurrentWeapon());

            if (!gameObject.IsAlive)
                currentLevel.GameObjects.Remove(gameObject);

            gameObject.onUpdate(gameTime);
        }

        if (player.X + 200 >= currentLevel.Width)
            if (currentLevel.EnemyCount == 0)
                NextLevel();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        camera.Update(player.Position);
        spriteBatch.Begin(transformMatrix: camera.Transform);

        for (var i = 0; i < currentLevel.GameObjects.Count; i++)
        {
            var gameObject = currentLevel.GameObjects[i];
            gameObject.Draw(spriteBatch);

            if (TypesUtils.CanBeDownCasted<GameObject, IWeaponUser>(gameObject))
            {
                var weaponUser = (IWeaponUser)gameObject;
                if (weaponUser.IsAttacking())
                    weaponUser.GetCurrentWeapon().Draw(spriteBatch);
            }
        }

        //for (var i = 0; i < currentLevel.Width; i += 50)
        //    for (var j = 0; j < currentLevel.Height; j += 50)
        //        spriteBatch.Draw(_gridCube, new Rectangle(i, j, 50, 50), Color.White);

        spriteBatch.End();
    }

    private void NextLevel()
    {
        levelNumber += 1;
        currentLevel = Level.Level.CreateLevelFromFile("Level" + levelNumber.ToString(), control, out player);
        camera.SetNewMapSize(new System.Drawing.Size(currentLevel.Width, currentLevel.Height));
    }
}
