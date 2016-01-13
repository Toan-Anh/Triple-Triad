using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Triple_Triad
{
    class Sprite_Animate_Simple : Sprite
    {
        private int _Rows = 1;
        private int _Cols = 1;

        private int _AnimationInterval = 160;
        private int _AnimationCount = 0;

        private int _FrameIndex = 0;

        public int Rows
        {
            get { return _Rows; }
            set { _Rows = value; _Rectangle.Height = _Texture.Height / _Rows; }
        }

        public int Cols
        {
            get { return _Cols; }
            set { _Cols = value; _Rectangle.Width = _Texture.Width / _Cols; }
        }

        public Sprite_Animate_Simple()
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(string assetName)
        {
            base.LoadContent(assetName);
        }

        public void LoadContent(string assetName, int rows, int cols)
        {
            base.LoadContent(assetName);
            Rows = rows;
            Cols = cols;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _Rectangle.X = (_FrameIndex % _Cols) * _Rectangle.Width;
            _Rectangle.Y = (_FrameIndex / _Cols) * _Rectangle.Height;

            _AnimationCount += gameTime.ElapsedGameTime.Milliseconds;

            if (_AnimationCount >= _AnimationInterval)
            {
                _FrameIndex = (_FrameIndex + 1) % (_Rows * _Cols);
                _AnimationCount = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
