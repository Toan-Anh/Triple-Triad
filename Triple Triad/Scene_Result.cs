using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Triple_Triad
{
    class Scene_Result : Scene
    {
        private Sprite background;

        public Scene_Result()
        {
            // Set background
            background = new Sprite();
            background.LoadContent("Game Complete Screen");
            background.Depth = 0;

            // Add card to SceneEntities so they can be updated and drawn
            for (int i = 0; i < 5; ++i)
            {
                TripleTriadGame.Player1Cards[i].Position = new Vector2(32 + i * 64, 128);
                TripleTriadGame.Player2Cards[i].Position = new Vector2(32 + i * 64, 32);

                _SceneEntities.Add(TripleTriadGame.Player1Cards[i]);
                _SceneEntities.Add(TripleTriadGame.Player2Cards[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //if (Global.KeyboardManager.IsKeyPressedAndReleased(Keys.Enter))
            //{
            //    Global.SceneManager.Clear();
            //    Global.SceneManager.CurrentScene = new Scene_Rule();
            //}
        }

        public override void Draw(GameTime gameTime)
        {
            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(Global.BoardScaleH, Global.BoardScaleV, 1f));
            background.Draw(gameTime);
            foreach (VisibleEntity entity in _SceneEntities)
                entity.Draw(gameTime);
            Global.SpriteBatch.End();
        }
    }
}
