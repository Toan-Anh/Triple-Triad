using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Triple_Triad
{
    static class Global
    {
        private static Game _Game;
        private static MouseManager _MouseManager = new MouseManager();
        private static KeyboardManager _KeyboardManager = new KeyboardManager();
        private static SFXManager _SFXManager = new SFXManager();
        private static ContentManager _Content;
        private static GraphicsDeviceManager _Graphics;
        private static SpriteBatch _SpriteBatch;
        private static Camera _Camera;
        private static SceneManager _SceneManager = new SceneManager();

        private static Texture2D _ScreenSnapshot;

        private static float _BoardScaleH = 1;
        private static float _BoardScaleV = 1;

        private static VisibleEntity _PreviousFocus;
        private static VisibleEntity _CurrentFocus;

        private static Windowskin _Windowskin = new Windowskin();

        public static Game Game
        {
            get { return Global._Game; }
            set { Global._Game = value; }
        }

        internal static MouseManager MouseManager
        {
            get { return Global._MouseManager; }
        }
        internal static KeyboardManager KeyboardManager
        {
            get { return Global._KeyboardManager; }
        }
        public static SFXManager SFXManager
        {
            get { return Global._SFXManager; }
            set { Global._SFXManager = value; }
        }
        public static ContentManager Content
        {
            get { return Global._Content; }
            set { Global._Content = value; }
        }
        public static GraphicsDeviceManager Graphics
        {
            get { return Global._Graphics; }
            set { Global._Graphics = value; }
        }
        public static SpriteBatch SpriteBatch
        {
            get { return Global._SpriteBatch; }
            set { Global._SpriteBatch = value; }
        }
        internal static SceneManager SceneManager
        {
            get { return Global._SceneManager; }
            set { Global._SceneManager = value; }
        }

        public static int ScreenHeight
        {
            get { return _Graphics.PreferredBackBufferHeight; }
            set { _Graphics.PreferredBackBufferHeight = value; _Graphics.ApplyChanges(); }
        }

        public static int ScreenWidth
        {
            get { return _Graphics.PreferredBackBufferWidth; }
            set { _Graphics.PreferredBackBufferWidth = value; _Graphics.ApplyChanges(); }
        }

        public static float BoardScaleH
        {
            get { return Global._BoardScaleH; }
            set { Global._BoardScaleH = value; }
        }
        public static float BoardScaleV
        {
            get { return Global._BoardScaleV; }
            set { Global._BoardScaleV = value; }
        }

        internal static Camera Camera
        {
            get { return Global._Camera; }
            set { Global._Camera = value; }
        }
        public static Windowskin Windowskin
        {
            get { return Global._Windowskin; }
            set { Global._Windowskin = value; }
        }

        //public static Texture2D ScreenSnapshot
        //{
        //    get
        //    {
        //        int[] _backImg = new int[_Graphics.PreferredBackBufferWidth * _Graphics.PreferredBackBufferHeight];
        //        _Graphics.GraphicsDevice.GetBackBufferData(_backImg);
        //        if (_ScreenSnapshot != null)
        //            _ScreenSnapshot.Dispose();
        //        _ScreenSnapshot = new Texture2D(_Graphics.GraphicsDevice, _Graphics.PreferredBackBufferWidth, _Graphics.PreferredBackBufferHeight, false, _Graphics.GraphicsDevice.PresentationParameters.BackBufferFormat);
        //        _ScreenSnapshot.SetData(_backImg);

        //        System.IO.Stream st = new System.IO.FileStream("Screenshot.png", System.IO.FileMode.Create);
        //        _ScreenSnapshot.SaveAsPng(st, _ScreenSnapshot.Width, _ScreenSnapshot.Height);
        //        st.Close(); 

        //        return _ScreenSnapshot;
        //    }
        //}

        public static void SwitchFocus(VisibleEntity src, VisibleEntity des)
        {
            _PreviousFocus = src;
            _CurrentFocus = des;
            src.HasFocus = false;
            des.HasFocus = true;
        }
        public static void RestoreFocus()
        {
            _CurrentFocus.HasFocus = false;
            _PreviousFocus.HasFocus = true;
        }
    }
}
