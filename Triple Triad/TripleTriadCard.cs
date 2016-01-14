using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Triple_Triad
{
    class TripleTriadCard : Sprite
    {
        private string _Name;
        private int[] _CardValue = new int[4];
        private TripleTriadCardLib.Element _Element;
        private string _Type;
        private int _Level;

        private int _PlayerNumber;
        private bool _IsOpen = true;

        public string Name
        {
            get { return _Name; }
        }
        public int[] CardValue
        {
            get { return _CardValue; }
        }
        internal TripleTriadCardLib.Element Element
        {
            get { return _Element; }
        }
        public string Type
        {
            get { return _Type; }
        }
        public int Level
        {
            get { return _Level; }
        }
        public int PlayerNumber
        {
            get { return _PlayerNumber; }
            set { _PlayerNumber = value; }
        }
        public bool IsOpen
        {
            get { return _IsOpen; }
            set { _IsOpen = value; }
        }

        public override void Initialize()
        {
        }

        public void LoadContent(string path, string info)
        {
            string[] tokens = info.Split('\t');
            _Level = int.Parse(tokens[0]);
            _Name = tokens[1];
            _Type = tokens[2];
            for (int i = 0; i < 4; ++i)
            {
                if (tokens[i + 3] == "A")
                    _CardValue[i] = 10;
                else
                    _CardValue[i] = int.Parse(tokens[i + 3]);
            }
            _Element = TripleTriadCardLib.GetElementByName(tokens[7]);

            _PlayerNumber = 1;
            base.LoadContent(path + _Name);

            //RenderTarget2D renderTarget = new RenderTarget2D(Global.Graphics.GraphicsDevice,
            //    _Texture.Width, _Texture.Height);
            //Global.Graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            //Global.Graphics.GraphicsDevice.Clear(Color.Transparent);
            //Global.SpriteBatch.Begin();
            //Global.SpriteBatch.Draw(_Texture, _Position + _Center * _Scale, _Rectangle, _TintColor.A == 0 ? Color.Transparent : _TintColor, _Rotation, _Center, _Scale, SpriteEffects.None, _Depth);
            //if (_Element != TripleTriadCardLib.Element.None)
            //    Global.SpriteBatch.Draw(TripleTriadCardLib.Elements, _Position + new Vector2(40, 4), TripleTriadCardLib.ElementRect[(int)_Element - 1], _TintColor, 0, Vector2.Zero, _Scale, SpriteEffects.None, _Depth + 0.002f);
            //DrawCardValues();
            //Global.SpriteBatch.End();
            //Global.Graphics.GraphicsDevice.SetRenderTarget(null);
            //_Texture = renderTarget;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_IsOpen)
            {
                if (_PlayerNumber == 1)
                    Global.SpriteBatch.Draw(TripleTriadCardLib.Player1Background, _Position, null, _TintColor, _Rotation, _Center, _Scale, SpriteEffects.None, _Depth - 0.001f);
                else if (_PlayerNumber == 2)
                    Global.SpriteBatch.Draw(TripleTriadCardLib.Player2Background, _Position, null, _TintColor, _Rotation, _Center, _Scale, SpriteEffects.None, _Depth - 0.001f);
                base.Draw(gameTime);
                if (_Element != TripleTriadCardLib.Element.None)
                    Global.SpriteBatch.Draw(TripleTriadCardLib.Elements, _Position + new Vector2(40, 4), TripleTriadCardLib.ElementRect[(int)_Element - 1], _TintColor, 0, Vector2.Zero, _Scale, SpriteEffects.None, _Depth + 0.002f);
                DrawCardValues();
            }
            else
            {
                Global.SpriteBatch.Draw(TripleTriadCardLib.CardBack, _Position, null, _TintColor, _Rotation, _Center, _Scale, SpriteEffects.None, _Depth);
            }
        }

        private void DrawCardValues()
        {
            Global.SpriteBatch.Draw(TripleTriadCardLib.CardNumbers, _Position + new Vector2(8, 4), TripleTriadCardLib.NumberRect[_CardValue[3] - 1], _TintColor, 0, Vector2.Zero, _Scale / 2, SpriteEffects.None, _Depth + 0.001f);
            Global.SpriteBatch.Draw(TripleTriadCardLib.CardNumbers, _Position + new Vector2(12, 12), TripleTriadCardLib.NumberRect[_CardValue[2] - 1], _TintColor, 0, Vector2.Zero, _Scale / 2, SpriteEffects.None, _Depth + 0.001f);
            Global.SpriteBatch.Draw(TripleTriadCardLib.CardNumbers, _Position + new Vector2(4, 12), TripleTriadCardLib.NumberRect[_CardValue[1] - 1], _TintColor, 0, Vector2.Zero, _Scale / 2, SpriteEffects.None, _Depth + 0.001f);
            Global.SpriteBatch.Draw(TripleTriadCardLib.CardNumbers, _Position + new Vector2(8, 20), TripleTriadCardLib.NumberRect[_CardValue[0] - 1], _TintColor, 0, Vector2.Zero, _Scale / 2, SpriteEffects.None, _Depth + 0.001f);
        }

        // Provides a copy of a card.
        // Since the attributes of a card is unchanged throughout the game, 
        // only shallow copying is used. Positions and other data associating
        // with the sprite can be changed freely.
        public TripleTriadCard GetInstance()
        {
            TripleTriadCard card = new TripleTriadCard();
            card._Texture = _Texture;
            card._Rectangle = _Rectangle;
            card._Name = _Name;
            card._CardValue = _CardValue;
            card._Element = _Element;
            card._Type = _Type;
            card._Level = _Level;
            card._PlayerNumber = _PlayerNumber;
            return card;
        }
    }
}
