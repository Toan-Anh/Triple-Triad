using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Triple_Triad
{
    class Scene_Rule : Scene
    {
        private Sprite background;
        private Window _HelpWindow;
        private Window _RuleWindow;

        public Scene_Rule()
        {
            // Set game rule
            GameRule.Open = true;
            GameRule.Same = true;
            GameRule.Plus = true;
            GameRule.Elemental = true;
            GameRule.Random = true;

            // Initialize the game
            TripleTriadGame.Initialize();

            // Set background
            background = new Sprite();
            background.LoadContent("Game Complete Screen");
            background.Depth = 0;

            // Scale the board to fit the screen
            Global.BoardScaleH = (float)Global.ScreenWidth / background.ViewportWidth;
            Global.BoardScaleV = (float)Global.ScreenHeight / background.ViewportHeight;

            // Initialize Rule Window
            InitRuleWindow();

            // Initialize Help Window
            InitHelpWindow();

            Song BGM = Global.Content.Load<Song>("Audio/15");
            MediaPlayer.Stop();
            MediaPlayer.Play(BGM);
            MediaPlayer.IsRepeating = true;
        }

        private void InitHelpWindow()
        {
            _HelpWindow = new Window();
            _HelpWindow.CenterText = true;
            _HelpWindow.Height = _HelpWindow.FittingHeight(1);
            _HelpWindow.Text = "Press ENTER to continue";
            _HelpWindow.Open();
        }

        private void InitRuleWindow()
        {
            _RuleWindow = new Window();
            _RuleWindow.Text = "Open:          " + GameRule.Open + "\n";
            _RuleWindow.Text += "Elemental:     " + GameRule.Elemental + "\n";
            _RuleWindow.Text += "Same:          " + GameRule.Same + "\n";
            _RuleWindow.Text += "Same Wall:     " + GameRule.SameWall + "\n";
            _RuleWindow.Text += "Plus:          " + GameRule.Plus + "\n";
            _RuleWindow.Text += "Plus Wall:     " + GameRule.PlusWall + "\n";
            _RuleWindow.Text += "--------------------\n";
            _RuleWindow.Text += "Trade Rule: " + GameRule.TradingRule + "\n";
            _RuleWindow.Open();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Global.KeyboardManager.IsKeyPressedAndReleased(Keys.Enter))
            {
                if (GameRule.Random)
                {
                    TripleTriadGame.GenerateRandomHands();
                    Global.SceneManager.CurrentScene = new Scene_Triple_Triad_Main();
                }
                //else
                //    Global.SceneManager.CurrentScene = new Scene_PickCards();
            }

            _HelpWindow.Width = (int)(background.ViewportWidth * Global.BoardScaleH - Global.BoardScaleH * 64 * 2);
            _HelpWindow.ScreenPosition = new Vector2((int)(Global.BoardScaleH * 64), (int)(Global.ScreenHeight - _HelpWindow.Height - Global.BoardScaleV * 16));
            _HelpWindow.Update(gameTime);

            _RuleWindow.ScreenPosition = new Vector2((int)(Global.BoardScaleH * 64), (int)(Global.BoardScaleV * 16));
            _RuleWindow.Width = (int)(background.ViewportWidth * Global.BoardScaleH - Global.BoardScaleH * 64 * 2);
            _RuleWindow.Height = (int)(_HelpWindow.ScreenPosition.Y - _RuleWindow.ScreenPosition.Y - Global.BoardScaleV * 8);
            _RuleWindow.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(Global.BoardScaleH, Global.BoardScaleV, 1f));
            background.Draw(gameTime);
            Global.SpriteBatch.End();

            Global.SpriteBatch.Begin();
            _RuleWindow.Draw(gameTime);
            _HelpWindow.Draw(gameTime);
            Global.SpriteBatch.End();
        }
    }
}
