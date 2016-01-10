using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Triple_Triad
{
    class Window : VisibleEntity
    {
        protected SpriteFont _Font = Global.Content.Load<SpriteFont>("VL_Gothic");
        protected string _OriginalText = "";
        protected string _FormattedText = "";
        protected Vector2 _ScreenPosition = Vector2.Zero;
        protected Vector2 _WorldPosition = Vector2.Zero;
        protected int _Width = 100;
        protected int _Height = 100;
        protected Vector2 _Padding = new Vector2(12, 12);
        protected Rectangle _Contents;

        private bool reformatText = false;
        protected bool _IsPending = false;
        protected bool _CenterText = false;

        private double _AnimationSpeed = 0.16;
        private double _AnimationTimer = 0.00;

        protected bool isOpen = false;
        private bool isOpening = false;


        public virtual SpriteFont Font
        {
            get { return _Font; }
            set { _Font = value; }
        }
        public virtual string Text
        {
            get { return _OriginalText; }
            set { _OriginalText = value; FormatText(_WorldPosition + _Padding); }
        }
        public virtual Vector2 ScreenPosition
        {
            get { return _ScreenPosition; }
            //set { _ScreenPosition = value; _WorldPosition = Global.CurrentCamera.WorldToScreen(_ScreenPosition); reformatText = true; }
            set { _ScreenPosition = value; _WorldPosition = _ScreenPosition; reformatText = true; }
        }
        public virtual int Width
        {
            get { return _Width; }
            set { _Width = value; reformatText = true; }
        }
        public virtual int Height
        {
            get { return _Height; }
            set { _Height = value; reformatText = true; }
        }

        public bool CenterText
        {
            get { return _CenterText; }
            set { _CenterText = value; }
        }

        public Window()
        {
            UpdateContents();
        }

        public Window(int x, int y, int width, int height)
        {
            _WorldPosition = new Vector2(x, y);
            _Width = width;
            _Height = height;
            UpdateContents();
        }

        public override void LoadContent(string fontName)
        {
            _Font = Global.Content.Load<SpriteFont>(fontName);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _AnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            UpdatePosition();
            UpdateContents();

            if (reformatText)
            {
                FormatText(_WorldPosition + _Padding);
                reformatText = false;
            }

            if (_IsPending && (_AnimationTimer >= _AnimationSpeed))
            {
                Global.Windowskin.CurrentPeddingIndex = (Global.Windowskin.CurrentPeddingIndex + 1) % 4;
                _AnimationTimer = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            DrawWindow();
            DrawText(new Vector2(_Contents.X, _Contents.Y), 0.93f);
        }





        public int FittingHeight(int nLines)
        {
            return (int)_Font.MeasureString(" ").Y * nLines + (int)_Padding.Y * 2;
        }

        protected void UpdateContents()
        {
            _Contents = new Rectangle((int)_WorldPosition.X + (int)_Padding.X, (int)_WorldPosition.Y + (int)_Padding.Y, _Width - 2 * (int)_Padding.X, _Height - 2 * (int)_Padding.Y);
        }

        protected void UpdatePosition()
        {
            //_WorldPosition = Global.CurrentCamera.ScreenToWorld(_ScreenPosition);
            _WorldPosition = _ScreenPosition;
        }

        protected void DrawWindow()
        {
            if (isOpen)
            {
                DrawBackground();
                DrawOverlayTexture();
                DrawFrameCorners();
                DrawFrameSides();
                if (_IsPending) DrawPendingAnimation(0.94f);
            }
        }

        private void DrawByTiling(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float depth)
        {
            Vector2 curPos = new Vector2();
            Rectangle realSrcRec;

            for (curPos.Y = destinationRectangle.Y; curPos.Y < destinationRectangle.Y + destinationRectangle.Height; curPos.Y += sourceRectangle.Height)
            {
                for (curPos.X = destinationRectangle.X; curPos.X < destinationRectangle.X + destinationRectangle.Width; curPos.X += sourceRectangle.Width)
                {
                    int w = destinationRectangle.Width + destinationRectangle.X - (int)curPos.X;
                    int h = destinationRectangle.Height + destinationRectangle.Y - (int)curPos.Y;

                    realSrcRec = new Rectangle(sourceRectangle.X, sourceRectangle.Y, (int)Math.Min(w, sourceRectangle.Width), (int)Math.Min(h, sourceRectangle.Height));

                    Global.SpriteBatch.Draw(
                        Global.Windowskin.Texture,
                        curPos,
                        realSrcRec,
                        Color.White,
                        0, Vector2.Zero, 1, SpriteEffects.None, depth);
                }
            }
        }

        private void DrawBackground()
        {
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Rectangle((int)_WorldPosition.X + 2, (int)_WorldPosition.Y + 2, _Width - 4, _Height - 4),
                Global.Windowskin.Background,
                Color.White,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                0.9f);
        }

        private void DrawOverlayTexture()
        {
            DrawByTiling(
                Global.Windowskin.Texture,
                new Rectangle((int)_WorldPosition.X + 2, (int)_WorldPosition.Y + 2, _Width - 4, _Height - 4),
                Global.Windowskin.OverlayTexture, 0.91f);
        }

        private void DrawFrameCorners()
        {
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Vector2(_WorldPosition.X, _WorldPosition.Y),
                Global.Windowskin.FrameCorners[0],
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Vector2(_WorldPosition.X + _Width - 16, _WorldPosition.Y),
                Global.Windowskin.FrameCorners[1],
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Vector2(_WorldPosition.X, _WorldPosition.Y + _Height - 16),
                Global.Windowskin.FrameCorners[2],
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Vector2(_WorldPosition.X + _Width - 16, _WorldPosition.Y + _Height - 16),
                Global.Windowskin.FrameCorners[3],
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);
        }

        private void DrawFrameSides()
        {
            DrawByTiling(
                Global.Windowskin.Texture,
                new Rectangle((int)_WorldPosition.X + 16, (int)_WorldPosition.Y, _Width - 32, 16),
                Global.Windowskin.FrameSides[0], 0.92f);
            DrawByTiling(
                Global.Windowskin.Texture,
                new Rectangle((int)_WorldPosition.X, (int)_WorldPosition.Y + 16, 16, _Height - 32),
                Global.Windowskin.FrameSides[1], 0.92f);
            DrawByTiling(
                Global.Windowskin.Texture,
                new Rectangle((int)_WorldPosition.X + _Width - 16, (int)_WorldPosition.Y + 16, 16, _Height - 32),
                Global.Windowskin.FrameSides[2], 0.92f);
            DrawByTiling(
                Global.Windowskin.Texture,
                new Rectangle((int)_WorldPosition.X + 16, (int)_WorldPosition.Y + _Height - 16, _Width - 32, 16),
                Global.Windowskin.FrameSides[3], 0.92f);
        }

        private void DrawPendingAnimation(float depth)
        {
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                //Global.CurrentCamera.ScreenToWorld(new Vector2((_ScreenPosition.X + _Width - 16) / 2, (_ScreenPosition.Y + Height) - 16)),
                new Vector2((_ScreenPosition.X + _Width - 16) / 2, (_ScreenPosition.Y + Height) - 16),
                Global.Windowskin.PendingAnimation[Global.Windowskin.CurrentPeddingIndex],
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
        }

        protected virtual void DrawText(Vector2 position, float depth)
        {
            if (isOpen)
            {
                if (_CenterText)
                {
                    string[] lines = _FormattedText.Split('\n');
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        Vector2 fontMeasurement = _Font.MeasureString(lines[i]);
                        Vector2 pos = new Vector2(position.X + (_Contents.Width - fontMeasurement.X) / 2, position.Y + i * fontMeasurement.Y);
                        Global.SpriteBatch.DrawString(_Font, lines[i], pos, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
                    }
                }
                else
                    Global.SpriteBatch.DrawString(_Font, _FormattedText, position, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
            }
        }

        protected virtual void FormatText(Vector2 position)
        {
            //Rectangle textContentRegion = 
            //    new Rectangle((int)position.X, (int)position.Y, 
            //        (int)(_Contents.Width - position.X), 
            //        (int)(_Contents.Height - position.Y));
            Rectangle textContentRegion = _Contents;
            _FormattedText = "";
            string[] words = _OriginalText.Split(' ');
            string line = "";

            for (int i = 0; i < words.Length; ++i)
            {
                if (_Font.MeasureString(line + words[i]).X > textContentRegion.Width)
                {
                    _FormattedText += line + "\n";
                    line = "";
                }
                line += words[i] + ' ';
            }
            _FormattedText += line;
        }

        protected virtual void DrawIcon(Vector2 position, int iconID)
        {
            //Global.SpriteBatch.Draw(IconSet.IconSetTexture, position, IconSet.Icons[iconID], Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.93f);
        }

        protected virtual void DrawFace(int faceIndex, Vector2 position, bool enabled = true)
        {
            Texture2D faceTexture = Global.Content.Load<Texture2D>("facename");
            Rectangle rect = new Rectangle(faceIndex % 4 * 96, faceIndex / 4 * 96, 96, 96);
            Global.SpriteBatch.Draw(faceTexture, position, rect, (enabled) ? Color.White : new Color(255, 255, 255, 200), 0, Vector2.Zero, 1, SpriteEffects.None, 0.93f);
            faceTexture.Dispose();
        }



        public virtual void Open()
        {
            isOpen = true;
        }

        public virtual void Close()
        {
            isOpen = false;
        }
    }
}
