using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Triple_Triad
{
    class Windowskin
    {
        private Texture2D _WindowskinTexture;
        private Texture2D _CurrentWindowskinTexture;

        private Rectangle _Background;
        private Rectangle _OverlayTexture;
        private Rectangle[] _FrameCorners;
        private Rectangle[] _FrameSides;
        private Rectangle[] _CursorCorners;
        private Rectangle[] _CursorSides;
        private Rectangle _CursorBody;
        private Rectangle[] _PendingAnimation;
        private Rectangle[] _Arrows;

        private int _CurrentPeddingIndex;

        //private short _Red;
        //private short _Green;
        //private short _Blue;
        //private short _Alpha;

        public Texture2D Texture
        {
            get { return _CurrentWindowskinTexture; }
        }
        public Rectangle Background
        {
            get { return _Background; }
        }
        public Rectangle OverlayTexture
        {
            get { return _OverlayTexture; }
        }
        public Rectangle[] FrameCorners
        {
            get { return _FrameCorners; }
        }
        public Rectangle[] FrameSides
        {
            get { return _FrameSides; }
        }
        public Rectangle[] CursorCorners
        {
            get { return _CursorCorners; }
        }
        public Rectangle[] CursorSides
        {
            get { return _CursorSides; }
        }
        public Rectangle CursorBody
        {
            get { return _CursorBody; }
        }
        public Rectangle[] PendingAnimation
        {
            get { return _PendingAnimation; }
        }


        public int CurrentPeddingIndex
        {
            get { return _CurrentPeddingIndex; }
            set { _CurrentPeddingIndex = value; }
        }


        public Windowskin()
        {
            //_Red = _Green = _Blue = 0;
            //_Alpha = 150;
            _Background = new Rectangle(0, 0, 63, 64);
            _OverlayTexture = new Rectangle(0, 64, 63, 64);
            _FrameCorners = new Rectangle[4];
            _FrameSides = new Rectangle[4];
            _PendingAnimation = new Rectangle[4];

            _FrameCorners[0] = new Rectangle(64, 0, 16, 16);
            _FrameCorners[1] = new Rectangle(112, 0, 16, 16);
            _FrameCorners[2] = new Rectangle(64, 48, 16, 16);
            _FrameCorners[3] = new Rectangle(112, 48, 16, 16);

            _FrameSides[0] = new Rectangle(80, 0, 32, 16);
            _FrameSides[1] = new Rectangle(64, 16, 16, 32);
            _FrameSides[2] = new Rectangle(112, 16, 16, 32);
            _FrameSides[3] = new Rectangle(80, 48, 32, 16);

            _Arrows = new Rectangle[4];
            _Arrows[0] = new Rectangle(88, 32, 16, 16);
            _Arrows[1] = new Rectangle(80, 24, 16, 16);
            _Arrows[2] = new Rectangle(96, 24, 16, 16);
            _Arrows[3] = new Rectangle(88, 16, 16, 16);

            _CursorCorners = new Rectangle[4];
            _CursorCorners[0] = new Rectangle(64, 64, 2, 2);
            _CursorCorners[1] = new Rectangle(94, 64, 2, 2);
            _CursorCorners[2] = new Rectangle(64, 94, 2, 2);
            _CursorCorners[3] = new Rectangle(94, 94, 2, 2);

            _CursorSides = new Rectangle[4];
            _CursorSides[0] = new Rectangle(66, 64, 28, 2);
            _CursorSides[1] = new Rectangle(64, 66, 2, 28);
            _CursorSides[2] = new Rectangle(94, 66, 2, 28);
            _CursorSides[3] = new Rectangle(66, 94, 28, 2);

            _CursorBody = new Rectangle(66, 66, 28, 28);

            _PendingAnimation[0] = new Rectangle(96, 64, 16, 16);
            _PendingAnimation[1] = new Rectangle(112, 64, 16, 16);
            _PendingAnimation[2] = new Rectangle(96, 80, 16, 16);
            _PendingAnimation[3] = new Rectangle(112, 80, 16, 16);
            _CurrentPeddingIndex = 0;
        }

        public void LoadContent(string assetname)
        {
            _WindowskinTexture = Global.Content.Load<Texture2D>(assetname);
            ChangeWindowColor(0, 0, 0, 160);
        }

        public void ChangeWindowColor(short dr, short dg, short db, byte a = 255)
        {
            Color[] tmp = new Color[128 * 128];
            _WindowskinTexture.GetData(tmp);

            for (int i = 0; i < 64; ++i)
            {
                for (int j = 0; j < 64; ++j)
                {
                    tmp[i * 128 + j].R = (byte)MathHelper.Clamp(tmp[i * 128 + j].R + dr, 0, 255);
                    tmp[i * 128 + j].G = (byte)MathHelper.Clamp(tmp[i * 128 + j].G + dg, 0, 255);
                    tmp[i * 128 + j].B = (byte)MathHelper.Clamp(tmp[i * 128 + j].B + db, 0, 255);
                    tmp[i * 128 + j].A = (byte)(tmp[i * 128 + j].A * (float) a / 255);
                }
            }

            if (_CurrentWindowskinTexture != null)
                _CurrentWindowskinTexture.Dispose();
            _CurrentWindowskinTexture = new Texture2D(Global.Graphics.GraphicsDevice, 128, 128);
            _CurrentWindowskinTexture.SetData(tmp);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_CurrentWindowskinTexture, Vector2.Zero, Color.White);
        }
    }
}
