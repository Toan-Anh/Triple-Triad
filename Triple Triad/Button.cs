using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Triple_Triad
{
    class Button
    {
        public delegate void EventHandler(object sender);
        public EventHandler MouseOver;
        public EventHandler MouseOut;
        public EventHandler Clicked;

        private bool bMouseOver = false;

        private Color _Background = Color.White;
        private Color _Foreground = Color.Black;

        private string _Text;
        private SpriteFont _Font = Global.Content.Load<SpriteFont>("VL_Gothic");

        private int _X;
        private int _Y;
        private int _Width;
        private int _Height;
        private Rectangle _Size;

        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        public int X
        {
            get { return _X; }
            set { _X = value; _Size = new Rectangle(_X, _Y, _Width, _Height); }
        }

        public int Y
        {
            get { return _Y; }
            set { _Y = value; _Size = new Rectangle(_X, _Y, _Width, _Height); }
        }

        public int Width
        {
            get { return _Width; }
            set { _Width = value; _Size = new Rectangle(_X, _Y, _Width, _Height); }
        }

        public int Height
        {
            get { return _Height; }
            set { _Height = value; _Size = new Rectangle(_X, _Y, _Width, _Height); }
        }

        public Button()
        {
            _Text = "Button";
            _X = 0;
            _Y = 0;
            _Width = 96;
            _Height = 32;
            _Size = new Rectangle(_X, _Y, _Width, _Height);
        }

        public void Update(GameTime gameTime)
        {
            Rectangle mouseRect = new Rectangle((int)Global.MouseManager.GetCurrentMousePosition().X, (int)Global.MouseManager.GetCurrentMousePosition().Y, 1, 1);
            if (_Size.Intersects(mouseRect))
                bMouseOver = true;

            // Check for mouse over and moust out.
            if (_Size.Intersects(mouseRect))
            {
                bMouseOver = true;
                _Background = Color.DarkBlue;
                _Foreground = Color.White;

                // If someone has subscribed to this event, tell them it has fired.
                if (MouseOver != null)
                    MouseOver(this);
            }
            else
            {
                if (bMouseOver)
                {
                    bMouseOver = false;
                    _Background = Color.White;
                    _Foreground = Color.Black;

                    // If someone has subscribed to this event, tell them it has fired.
                    if (MouseOut != null)
                        MouseOut(this);
                }
            }

            // Check for mouse clicked.
            if (bMouseOver && Global.MouseManager.IsLeftMouseButtonClicked())
            {
                // If someone has subscribed to this event, tell them it has fired.
                if (Clicked != null)
                    Clicked(this);
            }

            tmp.SetData<Color>(new Color[1] { _Background });
        }

        Texture2D tmp = new Texture2D(Global.Graphics.GraphicsDevice, 1, 1);

        public void Draw(GameTime gameTime)
        {
            Global.SpriteBatch.Draw(tmp, _Size, Color.White);
            Global.SpriteBatch.DrawString(_Font, _Text, Vector2.Zero, _Foreground);
        }
    }
}
