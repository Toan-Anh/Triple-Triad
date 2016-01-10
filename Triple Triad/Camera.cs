using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Triple_Triad
{
    class Camera
    {
        private Matrix _World;
        private Matrix _View;
        private Matrix _Projection;

        public Matrix World
        {
            get { return _World; }
            set { _World = value; }
        }
        public Matrix View
        {
            get { return _View; }
            set { _View = value; }
        }
        public Matrix Projection
        {
            get { return _Projection; }
            set { _Projection = value; }
        }

        public Camera()
        {
            _World = _View = _Projection = Matrix.Identity;
        }

        public Matrix WVP
        {
            get { return _World * _View * _Projection; }
        }

        public Matrix InverseWVP
        {
            get { return Matrix.Invert(WVP); }
        }

        public void Translate(float dx, float dy)
        {
            _View = _View * Matrix.CreateTranslation(dx, dy, 0);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Vector2 worldPosition = Vector2.Transform(screenPosition, InverseWVP);
            return worldPosition;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Vector2 screenPosition = Vector2.Transform(worldPosition, WVP);
            return screenPosition;
        }
    }
}
