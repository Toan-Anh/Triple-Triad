using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Triple_Triad
{
    class Window_Command : Window
    {
        public delegate void Handler();
        private List<string> _Items = new List<string>();

        public List<string> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }
        private int _CurrentIndex = 0;
        private Color _CursorTintColor = Color.White;
        private int _Columns = 1;
        private int _Rows = 5;
        private int _CurrentColumn = 0;
        private int _CurrentRow = 0;
        private Rectangle _CursorRectangle;
        private List<Vector2> _ItemPosition = new List<Vector2>();
        private int _ItemHeight;
        private int _ItemWidth;

        private double _AnimationSpeed = 0.16;
        private double _AnimationTimer = 0.00;

        private Dictionary<string, Handler> _CommandHandler = new Dictionary<string, Handler>();



        public Window_Command()
        {
            _ItemHeight = (int)_Font.MeasureString(" ").Y;
            _ItemWidth = _Contents.Width / _Columns - (_Columns - 1) * 16;
            _CursorRectangle = new Rectangle(_Contents.X, _Contents.Y, _ItemWidth, _ItemHeight);
            _AnimationSpeed = 0.5f;
            isOpen = true;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(string fontName)
        {
            base.LoadContent(fontName);
            SetCommandHandler("abc", abc);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _CurrentIndex = _CurrentRow * _Columns + _CurrentColumn;
            UpdateItemPosition();
            UpdateItems();
            UpdateCursorRectangle();
            

            if (_HasFocus && _CurrentIndex > -1)
            {
                _CursorTintColor.A = (byte)(255 * Math.Abs(Math.Sin((Math.PI) * (gameTime.TotalGameTime.TotalSeconds / _AnimationSpeed))));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            DrawWindow();
            DrawCursor();
        }





        private void DrawCursor()
        {
            DrawCursorCorners();
            DrawCursorSides();
            DrawCursorBody();
        }

        private void DrawCursorCorners()
        {
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Vector2(_CursorRectangle.X, _CursorRectangle.Y),
                Global.Windowskin.CursorCorners[0],
                _CursorTintColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Vector2(_CursorRectangle.X + _CursorRectangle.Width - 2, _CursorRectangle.Y),
                Global.Windowskin.CursorCorners[1],
                _CursorTintColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Vector2(_CursorRectangle.X, _CursorRectangle.Y + _CursorRectangle.Height - 2),
                Global.Windowskin.CursorCorners[2],
                _CursorTintColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Vector2(_CursorRectangle.X + _CursorRectangle.Width - 2, _CursorRectangle.Y + _CursorRectangle.Height - 2),
                Global.Windowskin.CursorCorners[3],
                _CursorTintColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);
        }

        private void DrawCursorSides()
        {
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Rectangle(_CursorRectangle.X + 2, _CursorRectangle.Y, _CursorRectangle.Width - 4, 2),
                Global.Windowskin.CursorSides[0],
                _CursorTintColor, 0, Vector2.Zero, SpriteEffects.None, 0.92f);
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Rectangle(_CursorRectangle.X, _CursorRectangle.Y + 2, 2, _CursorRectangle.Height - 4),
                Global.Windowskin.CursorSides[1],
                _CursorTintColor, 0, Vector2.Zero, SpriteEffects.None, 0.92f);
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Rectangle(_CursorRectangle.X + _CursorRectangle.Width - 2, _CursorRectangle.Y + 2, 2, _CursorRectangle.Height - 4),
                Global.Windowskin.CursorSides[2],
                _CursorTintColor, 0, Vector2.Zero, SpriteEffects.None, 0.92f);
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Rectangle(_CursorRectangle.X + 2, _CursorRectangle.Y + _CursorRectangle.Height - 2, _CursorRectangle.Width - 4, 2),
                Global.Windowskin.CursorSides[3],
                _CursorTintColor, 0, Vector2.Zero, SpriteEffects.None, 0.92f);
        }

        private void DrawCursorBody()
        {
            Global.SpriteBatch.Draw(
                Global.Windowskin.Texture,
                new Rectangle(_CursorRectangle.X + 2, _CursorRectangle.Y + 2, _CursorRectangle.Width - 4, _CursorRectangle.Height - 4),
                Global.Windowskin.CursorBody,
                _CursorTintColor, 0, Vector2.Zero, SpriteEffects.None, 0.92f);
        }

        private void DrawItems()
        {
            for (int i = 0; i < _Items.Count; ++i)
                Global.SpriteBatch.DrawString(_Font, _Items[i], _ItemPosition[i], Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.93f);
        }

        private void UpdateCursorRectangle()
        {
            if (Global.KeyboardManager.IsKeyPressedAndReleased(Input.Down))
                _CurrentRow = (int)MathHelper.Clamp(_CurrentRow + 1, 0, _Rows - 1);
            else if (Global.KeyboardManager.IsKeyPressedAndReleased(Input.Left))
                _CurrentColumn = (int)MathHelper.Clamp(_CurrentColumn - 1, 0, _Rows - 1);
            else if (Global.KeyboardManager.IsKeyPressedAndReleased(Input.Right))
                _CurrentColumn = (int)MathHelper.Clamp(_CurrentColumn - 1, 0, _Rows - 1);
            else if (Global.KeyboardManager.IsKeyPressedAndReleased(Input.Up))
                _CurrentRow = (int)MathHelper.Clamp(_CurrentRow - 1, 0, _Rows - 1);


            _CurrentIndex = _CurrentRow * _Columns + _CurrentColumn;
            _CursorRectangle = new Rectangle
                ((int)_ItemPosition[_CurrentIndex].X, (int)_ItemPosition[_CurrentIndex].Y, _ItemWidth, _ItemHeight);
        }

        public void SetCommandHandler(string command, Handler handler)
        {
            _CommandHandler.Add(command, handler);
        }

        public void Handle(string command)
        {
            if (_CommandHandler.ContainsKey(command))
                _CommandHandler[command]();
        }

        private void abc()
        {
        }




        protected void UpdateItems()
        {
            _ItemHeight = (int)_Font.MeasureString(" ").Y;
            _ItemWidth = _Contents.Width / _Columns - (_Columns - 1) * 16;
            _CursorRectangle = new Rectangle((int)_ItemPosition[_CurrentIndex].X, (int)_ItemPosition[_CurrentIndex].Y, _ItemWidth, _ItemHeight);
        }

        protected void UpdateItemPosition()
        {
            if (_ItemPosition.Count != 0)
                _ItemPosition.Clear();
            for (int i = 0; i < _Items.Count; ++i)
                _ItemPosition.Add(new Vector2((int)_Contents.X + (i % _Columns) * (_ItemWidth + 16), (int)_Contents.Y + (i / _Columns) * _ItemHeight));
        }

        protected void ChompMonoSpaceText()
        {
            int nChar = _ItemWidth / (int)_Font.MeasureString(" ").X;
            for (int i = 0; i < _Items.Count; ++i)
                if (_Font.MeasureString(_Items[i]).X <= _ItemWidth)
                    _Items[i] = _Items[i].Substring(0, nChar);
        }
    }
}
