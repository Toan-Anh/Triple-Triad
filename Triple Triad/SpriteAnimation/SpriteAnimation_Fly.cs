using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Triple_Triad.SpriteAnimation
{
    class SpriteAnimation_Fly : SpriteAnimation_Base
    {
        public enum FlyDirection
        {
            FromBottom, FromLeft, FromRight, FromTop,
            ToBottom, ToLeft, ToRight, ToTop,
        }

        private FlyDirection _Direction;
        private Vector2 _OriginalPosition;
        private Vector2 _TemporaryPosition;

        /// <summary>
        /// Contructor for the fading animation 
        /// </summary>
        /// <param name="sprite">The sprite to be animated</param>
        /// <param name="delay">The animation delay time in miliseconds. The default value is 0.</param>
        /// <param name="time">The animation time in miliseconds. The default value is 1000.</param>
        /// <param name="direction">The direction for animation. The default value is FlyDirection.FromBottom</param>
        public SpriteAnimation_Fly(Sprite sprite, int delay = 0, int time = 1000, FlyDirection direction = FlyDirection.FromBottom) : base(sprite, delay, time)
        {
            _Direction = direction;
            _OriginalPosition = _Sprite.Position;
            switch (direction)
            {
                case FlyDirection.FromBottom:
                case FlyDirection.ToBottom:
                    _TemporaryPosition = new Vector2(_Sprite.Position.X, Global.ScreenHeight);
                    break;
                case FlyDirection.FromLeft:
                case FlyDirection.ToLeft:
                    _TemporaryPosition = new Vector2(-_Sprite.ViewportWidth, _Sprite.Position.Y);
                    break;
                case FlyDirection.FromRight:
                case FlyDirection.ToRight:
                    _TemporaryPosition = new Vector2(Global.ScreenWidth, _Sprite.Position.Y);
                    break;
                case FlyDirection.FromTop:
                case FlyDirection.ToTop:
                    _TemporaryPosition = new Vector2(_Sprite.Position.X, -_Sprite.ViewportHeight);
                    break;
                default:
                    _TemporaryPosition = Vector2.Zero;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void PreAnimation()
        {
            if ((int)_Direction > 3)
                _Sprite.Position = _OriginalPosition;
            else
                _Sprite.Position = _TemporaryPosition;
        }

        public override void AnimationLogic()
        {
            if ((int)_Direction > 3)
                _Sprite.Position = (_TemporaryPosition - _OriginalPosition) * ((float)_AnimationCount / _AnimationInterval) + _OriginalPosition;
            else
                _Sprite.Position = (_OriginalPosition - _TemporaryPosition) * ((float)_AnimationCount / _AnimationInterval) + _TemporaryPosition;
        }

        public override void PostAnimation()
        {
            if ((int)_Direction > 3)
                _Sprite.Position = _TemporaryPosition;
            else
                _Sprite.Position = _OriginalPosition;
        }
    }
}
