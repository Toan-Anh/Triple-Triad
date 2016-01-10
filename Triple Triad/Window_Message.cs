using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Triple_Triad
{
    class Window_Message : Window
    {
        private List<string> _Pages = new List<string>();
        private int _CurrentPage = 0;
        private int _LetterIndex = 0;
        private int _LinesPerPage = 4;
        private bool _DrawByLetter = true;
        private bool _UseSound = false;
        private SoundEffect _LetterSound = Global.Content.Load<SoundEffect>("SFX1");

        private double _AnimationSpeed = 0.032;
        private double _AnimationTimer = 0.00;

        public bool DrawByLetter
        {
            get { return _DrawByLetter; }
            set { _DrawByLetter = value && _DrawByLetter; }
        }
        public bool UseSound
        {
            get { return _UseSound; }
            set
            {
                if (_DrawByLetter == true)
                    _UseSound = value;
                else
                    _UseSound = false;
            }
        }


        public Window_Message()
        {
            _IsPending = true;
            _HasFocus = true;
            _Width = Global.ScreenWidth;
            _Height = (int)_Font.MeasureString(" ").Y * _LinesPerPage + (int)_Padding.Y * 2;
            ScreenPosition = new Vector2(0, Global.ScreenHeight - _Height);
            UpdateContents();
        }

        public Window_Message(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            _IsPending = true;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(string fontName)
        {
            base.LoadContent(fontName);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _AnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (_DrawByLetter)
                UpdateLetterByLetter();
            else
                UpdateWholePage();

        }





        private void UpdateWholePage()
        {
            if (_HasFocus && Global.KeyboardManager.IsKeyPressedAndReleased(Input.Accept))
            {
                if (_CurrentPage < _Pages.Count - 1)
                    _CurrentPage += 1;
                else
                {
                    _CurrentPage = -1;
                    isOpen = false;
                    Global.RestoreFocus();
                }
            }
        }

        private void UpdateLetterByLetter()
        {
            if (_HasFocus && Global.KeyboardManager.IsKeyPressedAndReleased(Input.Accept))
            {
                if (_LetterIndex == _Pages[_CurrentPage].Length - 1 && _CurrentPage < _Pages.Count - 1)
                {
                    _CurrentPage += 1;
                    _LetterIndex = 0;
                }
                else if (_LetterIndex < _Pages[_CurrentPage].Length - 1)
                {
                    _LetterIndex = _Pages[_CurrentPage].Length - 1;
                }
                else
                {
                    _CurrentPage = -1;
                    isOpen = false;
                    Global.RestoreFocus();
                }
            }

            if (_AnimationTimer > _AnimationSpeed)
            {
                if (_HasFocus && _DrawByLetter && _LetterIndex < _Pages[_CurrentPage].Length - 1)
                    ++_LetterIndex;
                if (_UseSound && _Pages[_CurrentPage][_LetterIndex] != ' ' && _LetterIndex < _Pages[_CurrentPage].Length - 1)
                    _LetterSound.Play();
                _AnimationTimer = 0;
            }
        }


        public override void Draw(GameTime gameTime)
        {
            DrawWindow();

            if (_CurrentPage != -1)
                DrawTextByLetter(new Vector2(_Contents.X, _Contents.Y), 0.93f);
        }

        protected override void DrawText(Vector2 position, float depth)
        {
            if (_Pages.Count != 0 && isOpen)
                Global.SpriteBatch.DrawString(_Font, _Pages[_CurrentPage], position, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
        }

        protected void DrawTextByLetter(Vector2 position, float depth)
        {
            if (_Pages.Count != 0 && isOpen)
                Global.SpriteBatch.DrawString(_Font, _Pages[_CurrentPage].Substring(0, _LetterIndex + 1), position, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
        }

        protected override void FormatText(Vector2 position)
        {
            base.FormatText(position);
            _Pages.Clear();
            int lineHeight = (int)Font.MeasureString(" ").Y;
            //int linesPerPage = textContentRegion.Height / lineHeight;
            //if (linesPerPage <= 0)
            //    return;

            int index = 0;

            while (true)
            {
                int tmp = FindIndexOf(_FormattedText, '\n', _LinesPerPage, index);
                if (tmp < 0)
                    break;
                _Pages.Add(_FormattedText.Substring(index, tmp - index + 1));
                index = tmp + 1;
            }
            _Pages.Add(_FormattedText.Substring(index));
        }

        private int FindIndexOf(string str, char value, int occurrenceNumber, int startIndex)
        {
            int index = startIndex;
            for (int i = 0; i < occurrenceNumber; ++i)
            {
                index = str.IndexOf(value, index + 1);
                if (index < 0)
                    break;
            }
            return index;
        }

        protected void Reset()
        {
            _CurrentPage = 0;
            _LetterIndex = 0;
        }
    }
}
