using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Triple_Triad
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Global.Game = this;
            Global.Content = Content;
            Global.Graphics = graphics;
            Global.SpriteBatch = spriteBatch;
            Global.MouseManager.Cursor.LoadContent("Cursor");
            Global.Windowskin.LoadContent("Window");
            Global.Windowskin.ChangeWindowColor(-34, 0, 68, 200);
            Global.SFXManager.Initialize();

            Global.ScreenWidth = 640;
            Global.ScreenHeight = 480;

            TripleTriadCardLib.Initialize();

            Global.SceneManager.CurrentScene = new Scene_Rule();

            //Song BGM = Content.Load<Song>("15 - Shuffle or Boogie");
            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(BGM);

            //SoundEffect BGM = Content.Load<SoundEffect>("Audio/15 - Shuffle or Boogie");
            //SoundEffectInstance BGMInstance = BGM.CreateInstance();
            //BGMInstance.IsLooped = true;
            //BGMInstance.Play();
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            Global.MouseManager.Update(gameTime);
            Global.KeyboardManager.Update(gameTime);

            if (Global.KeyboardManager.IsKeyDown(Keys.LeftAlt) && Global.KeyboardManager.IsKeyPressedAndReleased(Keys.Enter))
            {
                Global.Graphics.ToggleFullScreen();
                if (graphics.IsFullScreen)
                {
                    Global.ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    Global.ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

                    Global.BoardScaleH *= Global.ScreenHeight / 480f;
                    Global.BoardScaleV *= Global.ScreenHeight / 480f;
                }
                else
                {
                    Global.BoardScaleH *= 480f / Global.ScreenHeight;
                    Global.BoardScaleV *= 480f / Global.ScreenHeight;

                    Global.ScreenWidth = 640;
                    Global.ScreenHeight = 480;
                }
                Global.Graphics.ApplyChanges();
            }

            if (Global.KeyboardManager.IsKeyPressedAndReleased(Keys.Tab))
            {
                if (Global.ScreenWidth == 1200)
                {
                    Global.ScreenWidth = 640;
                    Global.ScreenHeight = 480;
                    Global.BoardScaleH *= 640f / 1200;
                    Global.BoardScaleV *= 480f / 900;
                }
                else
                {
                    Global.ScreenWidth = 1200;
                    Global.ScreenHeight = 900;
                    Global.BoardScaleH *= 1200f / 640;
                    Global.BoardScaleV *= 900f / 480;
                }
                Global.Graphics.ApplyChanges();
            }

            if (Global.KeyboardManager.IsKeyPressedAndReleased(Keys.PrintScreen))
                Global.SceneManager.SaveSnapshot(gameTime);

            Global.SceneManager.UpdateCurrentScene(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            //float scaleFactor = Global.ScreenWidth / 800 * Global.BoardScaleH;
            //float scaleFactor = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 480;
            Global.SceneManager.DrawCurrentScene(gameTime);

            base.Draw(gameTime);
        }
    }
}
