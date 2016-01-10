using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Triple_Triad
{
    class MouseManager
    {
        private MouseState _PreviousMouseState;
        private MouseState _CurrentMouseState;
        private Sprite _Cursor = new Sprite();

        internal Sprite Cursor
        {
            get { return _Cursor; }
            set { _Cursor = value; }
        }

        public void Update(GameTime gameTime)
        {
            _PreviousMouseState = _CurrentMouseState;
            _CurrentMouseState = Mouse.GetState();
            _Cursor.Position = GetCurrentMousePosition();
        }

        public void DrawMouseCursor(GameTime gameTime)
        {
            Global.SpriteBatch.Begin();
            _Cursor.Draw(gameTime);
            Global.SpriteBatch.End();
        }

        public bool IsLeftMouseButtonClicked()
        {
            if (_PreviousMouseState.LeftButton == ButtonState.Pressed &&
                _CurrentMouseState.LeftButton == ButtonState.Released)
                return true;
            return false;
        }

        public bool IsLeftMouseButtonDown()
        {
            if (_CurrentMouseState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }

        public bool IsLeftMouseButtonUp()
        {
            if (_CurrentMouseState.LeftButton == ButtonState.Released)
                return true;
            return false;
        }

        public bool IsRightMouseButtonClicked()
        {
            if (_PreviousMouseState.RightButton == ButtonState.Pressed &&
                _CurrentMouseState.RightButton == ButtonState.Released)
                return true;
            return false;
        }

        public bool IsRightMouseButtonDown()
        {
            if (_CurrentMouseState.RightButton == ButtonState.Pressed)
                return true;
            return false;
        }

        public bool IsRightMouseButtonUp()
        {
            if (_CurrentMouseState.RightButton == ButtonState.Released)
                return true;
            return false;
        }

        public Vector2 GetCurrentMousePosition()
        {
            return new Vector2(_CurrentMouseState.X, _CurrentMouseState.Y);
        }

        public bool IsMouseOver(Rectangle rect)
        {
            if (IsMouseInRectangleRegion(rect) && IsLeftMouseButtonUp())
                return true;
            return false;
        }

        private bool IsMouseInRectangleRegion(Rectangle rect)
        {
            Vector2 mousePos = new Vector2(_CurrentMouseState.X / Global.BoardScaleH, _CurrentMouseState.Y / Global.BoardScaleV);
            return mousePos.X >= rect.X &&
                mousePos.X <= rect.X + rect.Width &&
                mousePos.Y >= rect.Y &&
                mousePos.Y <= rect.Y + rect.Height;
        }
    }
}
