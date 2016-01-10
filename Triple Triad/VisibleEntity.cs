using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Triple_Triad
{
    abstract class VisibleEntity
    {
        protected Sprite _MainSprite = null;
        protected bool _HasFocus = false;


        internal Sprite MainSprite
        {
            get { return _MainSprite; }
            set { _MainSprite = value; }
        }
        public bool HasFocus
        {
            get { return _HasFocus; }
            set { _HasFocus = value; }
        }

        public virtual void Initialize()
        {
        }

        public virtual void LoadContent(string assetName)
        {
            _MainSprite.LoadContent(assetName);
        }

        public virtual void UnloadContent()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            if (_MainSprite != null)
                _MainSprite.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (_MainSprite != null)
                _MainSprite.Draw(gameTime);
        }

        //public bool IsSelected()
        //{
        //    Vector2 mousePos = Global.MouseManager.GetCurrentMousePosition();
        //    mousePos.X *= Global.BoardScaleH;
        //    mousePos.Y *= Global.BoardScaleV;
        //    if (mousePos.X >= _MainSprite.Position.X &&
        //        mousePos.Y >= _MainSprite.Position.Y &&
        //        mousePos.X <= _MainSprite.Position.X + _MainSprite.Rectangle.Width &&
        //        mousePos.Y <= _MainSprite.Position.Y + _MainSprite.Rectangle.Height &&
        //        Global.MouseManager.IsLeftMouseButtonClicked())
        //        return true;
        //    return false;
        //}

        //public bool IsMouseOver()
        //{
        //    Vector2 mousePos = Global.MouseManager.GetCurrentMousePosition();
        //    if (mousePos.X >= _MainSprite.Position.X &&
        //        mousePos.Y >= _MainSprite.Position.Y &&
        //        mousePos.X < _MainSprite.Position.X + _MainSprite.Rectangle.Width &&
        //        mousePos.Y < _MainSprite.Position.Y + _MainSprite.Rectangle.Height && 
        //        Global.MouseManager.IsLeftMouseButtonUp())
        //    {
        //        Color[] c = new Color[1];
        //        Rectangle sourceRec = new Rectangle((int)(mousePos.X - _MainSprite.Position.X), (int)(mousePos.Y - _MainSprite.Position.Y), 1, 1);
        //        _MainSprite.Texture.GetData<Color>(0, sourceRec, c, 0, 1);
        //        if (c[0].A != 0)
        //            return true;
        //    }
        //    return false;
        //}

        //public bool IsMouseDown()
        //{
        //    Vector2 mousePos = Global.MouseManager.GetCurrentMousePosition();
        //    if (mousePos.X >= _MainSprite.Position.X &&
        //        mousePos.Y >= _MainSprite.Position.Y &&
        //        mousePos.X < _MainSprite.Position.X + _MainSprite.Rectangle.Width &&
        //        mousePos.Y < _MainSprite.Position.Y + _MainSprite.Rectangle.Height &&
        //        Global.MouseManager.IsLeftMouseButtonDown())
        //    {
        //        Color[] c = new Color[1];
        //        Rectangle sourceRec = new Rectangle((int)(mousePos.X - _MainSprite.Position.X), (int)(mousePos.Y - _MainSprite.Position.Y), 1, 1);
        //        _MainSprite.Texture.GetData<Color>(0, sourceRec, c, 0, 1);
        //        if (c[0].A != 0)
        //            return true;
        //    }
        //    return false;
        //}
    }
}
