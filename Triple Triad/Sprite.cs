using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Triple_Triad
{
    class Sprite : VisibleEntity
    {
        protected Texture2D _Texture;
        protected Vector2 _Position;
        protected Vector2 _PreviousPosition;

        protected Rectangle _Rectangle;
        protected Color _TintColor;
        protected float _Rotation;
        protected float _Scale;
        protected float _Depth;

        public Texture2D Texture
        {
            get { return _Texture; }
            set { _Texture = value; }
        }
        public virtual Vector2 Position
        {
            get { return _Position; }
            set { PreviousPosition = _Position;  _Position = value; }
        }
        public Vector2 PreviousPosition
        {
            get { return _PreviousPosition; }
            set { _PreviousPosition = value; }
        }
        public Rectangle Rectangle
        {
            get { return _Rectangle; }
            set { _Rectangle = value; }
        }
        public Color TintColor
        {
            get { return _TintColor; }
            set { _TintColor = value; }
        }
        public byte Opacity
        {
            get { return _TintColor.A; }
            set { _TintColor.A = value; }
        }
        public float Rotation
        {
            get { return _Rotation; }
            set { _Rotation = value; }
        }
        public float Scale
        {
            get { return _Scale; }
            set { _Scale = value; }
        }
        public float Depth
        {
            get { return _Depth; }
            set { _Depth = value; }
        }
        public int ViewportWidth
        {
            get { return _Rectangle.Width; }
            set { _Rectangle.Width = value; }
        }
        public int ViewportHeight
        {
            get { return _Rectangle.Height; }
            set { _Rectangle.Height = value; }
        }

        public Sprite()
        {
            _Texture = null;
            _Position = Vector2.Zero;
            _Rectangle = new Rectangle(0, 0, 0, 0);
            _TintColor = Color.White;
            _Rotation = 0;
            _Scale = 1;
            _Depth = 0.1f;
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(string assetName)
        {
            _Texture = Global.Content.Load<Texture2D>(assetName);
            _Rectangle = new Rectangle(0, 0, _Texture.Width, _Texture.Height);
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            Global.SpriteBatch.Draw(_Texture, _Position, _Rectangle, _TintColor.A == 0 ? Color.Transparent : _TintColor, _Rotation, Vector2.Zero, _Scale, SpriteEffects.None, _Depth);
        }

        public bool IsSelected()
        {
            Vector2 mousePos = Global.MouseManager.GetCurrentMousePosition();
            mousePos.X /= Global.BoardScaleH;
            mousePos.Y /= Global.BoardScaleV;
            if (isMouseInSpriteRegion(mousePos) &&
                Global.MouseManager.IsLeftMouseButtonClicked())
            {
                System.Console.WriteLine(mousePos);
                return true;
            }
            return false;
        }

        public bool IsMouseOver()
        {
            Vector2 mousePos = Global.MouseManager.GetCurrentMousePosition();
            mousePos.X /= Global.BoardScaleH;
            mousePos.Y /= Global.BoardScaleV;
            if (isMouseInSpriteRegion(mousePos) &&
                Global.MouseManager.IsLeftMouseButtonUp())
            {
                //Color[] c = new Color[1];
                //Rectangle sourceRec = new Rectangle((int)(mousePos.X - _MainSprite.Position.X), (int)(mousePos.Y - _MainSprite.Position.Y), 1, 1);
                //_MainSprite.Texture.GetData<Color>(0, sourceRec, c, 0, 1);
                //if (c[0].A != 0)
                    return true;
            }
            return false;
        }

        public bool IsMouseDown()
        {
            Vector2 mousePos = Global.MouseManager.GetCurrentMousePosition();
            mousePos.X /= Global.BoardScaleH;
            mousePos.Y /= Global.BoardScaleV;
            if (isMouseInSpriteRegion(mousePos) &&
                Global.MouseManager.IsLeftMouseButtonDown())
            {
                //Color[] c = new Color[1];
                //Rectangle sourceRec = new Rectangle((int)(mousePos.X - _MainSprite.Position.X), (int)(mousePos.Y - _MainSprite.Position.Y), 1, 1);
                //_MainSprite.Texture.GetData<Color>(0, sourceRec, c, 0, 1);
                //if (c[0].A != 0)
                    return true;
            }
            return false;
        }


        private bool isMouseInSpriteRegion(Vector2 mousePos)
        {
            return Global.Game.IsActive &&
                mousePos.X >= Position.X &&
                mousePos.Y >= Position.Y &&
                mousePos.X <= Position.X + Rectangle.Width * Scale &&
                mousePos.Y <= Position.Y + Rectangle.Height * Scale;
        }
    }
}
