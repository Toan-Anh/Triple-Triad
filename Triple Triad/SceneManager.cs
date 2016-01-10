using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Triple_Triad
{
    class SceneManager
    {
        private Stack<Scene> _Scenes;
        private Scene _CurrentScene;
        private Scene _NextScene;
        private Texture2D _BackgroundImage;

        internal Scene CurrentScene
        {
            get { return _CurrentScene; }
            set { _NextScene = value; }
        }

        public SceneManager()
        {
            _Scenes = new Stack<Scene>();
        }

        public void Call(Scene newScene)
        {
            if (_CurrentScene != null)
            {
                _Scenes.Push(_CurrentScene);
                _NextScene = newScene;
            }
            else
                _NextScene = newScene;
        }

        public void ReturnToCallerScene()
        {
            if (_Scenes.Count != 0)
            {
                _NextScene = _Scenes.Pop();
            }
        }

        public void Clear()
        {
            _Scenes.Clear();
        }

        public void UpdateCurrentScene(GameTime gameTime)
        {
            if (_NextScene != null && _NextScene != _CurrentScene)
                _CurrentScene = _NextScene;
            if (_CurrentScene != null)
                _CurrentScene.Update(gameTime);
        }

        public void DrawCurrentScene(GameTime gameTime)
        {
            if (_BackgroundImage != null)
            {
                Global.SpriteBatch.Begin();
                Global.SpriteBatch.Draw(_BackgroundImage, Vector2.Zero, Color.White);
                Global.SpriteBatch.End();
            }
            if (_CurrentScene != null)
                _CurrentScene.Draw(gameTime);
        }

        public void SnapshotForBackground(GameTime gameTime)
        {
            if (_BackgroundImage != null && !_BackgroundImage.IsDisposed)
                _BackgroundImage.Dispose();

            //RenderTarget2D screenShot = new RenderTarget2D(Global.Graphics.GraphicsDevice,
            //    Global.Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
            //    Global.Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);

            //Global.Graphics.GraphicsDevice.SetRenderTarget(screenShot);

            ////Global.Graphics.GraphicsDevice.Clear(Color.Black);
            ////Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            //_CurrentScene.Draw(gameTime);
            ////Global.SpriteBatch.End();

            //Global.Graphics.GraphicsDevice.SetRenderTarget(null);

            _BackgroundImage = CreateSnapShot(gameTime);
            BlurTexture(_BackgroundImage);
        }

        public void SaveSnapshot(GameTime gameTime)
        {
            Texture2D screenShot = CreateSnapShot(gameTime);
            String name = "Screenshot" + System.DateTime.Now.ToString("_ddMMyyhhmmssff") + ".png";
            System.IO.Stream outStream = new System.IO.FileStream(name, System.IO.FileMode.Create);
            screenShot.SaveAsPng(outStream, Global.ScreenWidth, Global.ScreenHeight);
            outStream.Close();
        }

        private Texture2D CreateSnapShot(GameTime gameTime)
        {
            RenderTarget2D screenShot = new RenderTarget2D(Global.Graphics.GraphicsDevice,
                Global.Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Global.Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);

            Global.Graphics.GraphicsDevice.SetRenderTarget(screenShot);
            _CurrentScene.Draw(gameTime);
            Global.Graphics.GraphicsDevice.SetRenderTarget(null);
            return screenShot;
        }

        private void BlurTexture(Texture2D texture)
        {
            Color[] screenData = new Color[texture.Width * texture.Height];
            texture.GetData(screenData);
            int count = Global.Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth * Global.Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            int width = texture.Width;
            for (int i = 0; i < count; ++i)
            {
                Color a, b, c, d, e;
                a = screenData[(int)MathHelper.Clamp(i - 1, 0, count - 1)];
                b = screenData[(int)MathHelper.Clamp(i + 1, 0, count - 1)];
                c = screenData[i];
                d = screenData[(int)MathHelper.Clamp(i - width, 0, count - 1)];
                e = screenData[(int)MathHelper.Clamp(i + width, 0, count - 1)];

                screenData[i].R = (byte)MathHelper.Clamp(((a.R + b.R + c.R + d.R + e.R) / 5) - 20, 0, 255);
                screenData[i].G = (byte)MathHelper.Clamp(((a.G + b.G + c.G + d.G + e.G) / 5) - 20, 0, 255);
                screenData[i].B = (byte)MathHelper.Clamp(((a.B + b.B + c.B + d.B + e.B) / 5) - 20, 0, 255);
                screenData[i].A = (byte)MathHelper.Clamp(((a.A + b.A + c.A + d.A + e.A) / 5), 0, 255);
            }
            texture.SetData(screenData);
        }

        private void GrayScale(Texture2D texture)
        {
            Color[] screenData = new Color[texture.Width * texture.Height];
            texture.GetData(screenData);
            int count = Global.Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth * Global.Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            for (int i = 0; i < count; ++i)
            {
                byte tmp = (byte) ((screenData[i].R * 0.3) + (screenData[i].G * 0.59) + (screenData[i].B * 0.11));
                screenData[i].R = screenData[i].G = screenData[i].B = tmp;
            }
            texture.SetData(screenData);
        }
    }
}
