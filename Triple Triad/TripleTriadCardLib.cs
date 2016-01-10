using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Triple_Triad
{
    static class TripleTriadCardLib
    {
        public enum Element
        {
            None, Fire, Blizzard, Thunder, Earth, Water, Poison, Wind, Holy,
        }

        private static Dictionary<int, TripleTriadCard> _CardInfo;
        private static Color _P1Start = new Color(189, 223, 255);
        private static Color _P1End = new Color(49, 65, 132);
        private static Color _P2Start = new Color(255, 190, 222);
        private static Color _P2End = new Color(132, 48, 66);
        private static Texture2D _Player1Background;
        private static Texture2D _Player2Background;
        private static Texture2D _CardNumbers;
        private static Texture2D _Scores;
        private static Texture2D _Elements;
        private static Texture2D _CardBack;

        private static Vector2[] _GameBoardPos;
        private static Vector2[] _Player1HandPos;
        private static Vector2[] _Player2HandPos;
        private static Rectangle[] _NumberRect;
        private static Rectangle[] _ScoreRect;
        private static Rectangle[] _ElementRect;

        internal static Dictionary<int, TripleTriadCard> CardInfo
        {
            get { return TripleTriadCardLib._CardInfo; }
        }

        public static Color Player1GradientStart
        {
            get { return _P1Start; }
        }
        public static Color Player1GradientEnd
        {
            get { return _P1End; }
        }
        public static Color Player2GradientStart
        {
            get { return _P2Start; }
        }
        public static Color Player2GradientEnd
        {
            get { return _P2End; }
        }
        public static Texture2D Player1Background
        {
            get { return TripleTriadCardLib._Player1Background; }
        }
        public static Texture2D Player2Background
        {
            get { return TripleTriadCardLib._Player2Background; }
        }
        public static Texture2D CardNumbers
        {
            get { return TripleTriadCardLib._CardNumbers; }
        }
        public static Texture2D CardBack
        {
            get { return TripleTriadCardLib._CardBack; }
        }
        public static Vector2[] GameBoardPos
        {
            get { return TripleTriadCardLib._GameBoardPos; }
        }
        public static Vector2[] Player1HandPos
        {
            get { return TripleTriadCardLib._Player1HandPos; }
        }
        public static Vector2[] Player2HandPos
        {
            get { return TripleTriadCardLib._Player2HandPos; }
        }
        public static Rectangle[] NumberRect
        {
            get { return TripleTriadCardLib._NumberRect; }
        }

        public static Texture2D Scores
        {
            get { return _Scores; }
        }
        public static Rectangle[] ScoreRect
        {
            get { return _ScoreRect; }
        }
        public static Texture2D Elements
        {
            get { return _Elements; }
        }
        public static Rectangle[] ElementRect
        {
            get { return _ElementRect; }
        }

        public static void Initialize()
        {
            InitGamePositions();
            _Player1Background = GenerateGradient(Player1GradientStart, Player1GradientEnd);
            _Player2Background = GenerateGradient(Player2GradientStart, Player2GradientEnd);
            ReadCardList("Card List.txt");
            InitNumbers();
            _CardBack = Global.Content.Load<Texture2D>("Cards/Card back");
            InitScores();
            InitElements();
        }

        private static void InitElements()
        {
            _Elements = Global.Content.Load<Texture2D>("Triple Triad Elements");
            _ElementRect = new Rectangle[8];
            for (int i = 0; i < 2; ++i)
                for (int j = 0; j < 4; ++j)
                    _ElementRect[i * 4 + j] = new Rectangle(20 * j, 17 * i, 20, 17);
        }

        private static void InitScores()
        {
            _Scores = Global.Content.Load<Texture2D>("score");
            _ScoreRect = new Rectangle[9];
            for (int i = 0; i < 9; ++i)
                _ScoreRect[i] = new Rectangle(0, 30 * i, 40, 30);
        }

        private static void InitNumbers()
        {
            _CardNumbers = Global.Content.Load<Texture2D>("Triple Triad Card Numbers");
            _NumberRect = new Rectangle[10];
            for (int i = 0; i < 10; ++i)
                _NumberRect[i] = new Rectangle(20 * i, 0, 20, 20);
        }

        private static void InitGamePositions()
        {
            // Board coordinate
            _GameBoardPos = new Vector2[9];
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    _GameBoardPos[i * 3 + j] = new Vector2(96 + 64 * j, 16 + 64 * i);

            // Players' hand coordinate
            _Player1HandPos = new Vector2[5];
            _Player2HandPos = new Vector2[5];
            for (int i = 0; i < 5; ++i)
            {
                _Player2HandPos[i] = new Vector2(16, 16 + 32 * i);
                _Player1HandPos[i] = new Vector2(304, 16 + 32 * i);
            }
        }

        private static void ReadCardList(string filename)
        {
            _CardInfo = new Dictionary<int, TripleTriadCard>();
            System.IO.StreamReader input = new System.IO.StreamReader(filename);
            for (int i = 0; !input.EndOfStream; ++i)
            {
                TripleTriadCard card = new TripleTriadCard();
                card.LoadContent("Cards/", input.ReadLine());
                _CardInfo.Add(i, card);
            }
            input.Close();
        }

        private static Texture2D GenerateGradient(Color start, Color end)
        {
            Texture2D result = new Texture2D(Global.Graphics.GraphicsDevice, 64, 64);
            Color[] data = new Color[64 * 64];
            for (int i = 0; i < 64; ++i)
            {
                for (int j = 0; j < 64; ++j)
                {
                    Color c = new Color();
                    if (i != 0 && i != 63 && j != 0 && j != 63)
                    {
                        c.R = (byte)Interpolate(start.R, end.R, i, 64);
                        c.G = (byte)Interpolate(start.G, end.G, i, 64);
                        c.B = (byte)Interpolate(start.B, end.B, i, 64);
                        c.A = 255;
                    }
                    data[i * 64 + j] = c;
                }
            }
            result.SetData(data);
            return result;
        }

        private static float Interpolate(float start, float end, int pos, int length)
        {
            if (start < 0 || start > 255 || end < 0 || end > 255 ||
                pos > length || pos < 0 || length < 0)
                return 0;
            return MathHelper.Clamp((pos * end + (length - pos) * start) / length, 0, 255);
        }


        public static Element GetElementByName(string name)
        {
            switch (name)
            {
                case "Fire":
                    return Element.Fire;
                case "Blizzard":
                    return Element.Blizzard;
                case "Thunder":
                    return Element.Thunder;
                case "Earth":
                    return Element.Earth;
                case "Water":
                    return Element.Water;
                case "Poison":
                    return Element.Poison;
                case "Wind":
                    return Element.Wind;
                case "Holy":
                    return Element.Holy;
                default:
                    return Element.None;
            }
        }

        public static int SelectedGridPosition()
        {
            Vector2 mousePos = Global.MouseManager.GetCurrentMousePosition();
            mousePos.X /= Global.BoardScaleH;
            mousePos.Y /= Global.BoardScaleV;
            for (int i = 0; i < _GameBoardPos.Length; ++i)
            {
                if (mousePos.X >= _GameBoardPos[i].X &&
                    mousePos.X <= _GameBoardPos[i].X + 64 &&
                    mousePos.Y >= _GameBoardPos[i].Y &&
                    mousePos.Y <= _GameBoardPos[i].Y + 64)
                    return i;
            }
            return -1;
        }
    }
}
