using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Triple_Triad.SpriteAnimation
{
    class SpriteAnimation_Move : SpriteAnimation_Base
    {
        private Vector2 _StartPosition;
        private Vector2 _EndPosition;

        /// <summary>
        /// Contructor for the fading animation 
        /// </summary>
        /// <param name="sprite">The sprite to be animated</param>
        /// <param name="delay">The animation delay time in miliseconds. The default value is 0.</param>
        /// <param name="time">The animation time in miliseconds. The default value is 1000.</param>
        /// <param name="direction">The direction for animation. The default value is FlyDirection.FromBottom</param>
        public SpriteAnimation_Move(Sprite sprite, Vector2 start, Vector2 end, int delay = 0, int time = 1000) : base(sprite, delay, time)
        {
            _StartPosition = start;
            _EndPosition = end;
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
            _Sprite.Position = _StartPosition;
        }

        public override void AnimationLogic()
        {
            _Sprite.Position = _EndPosition;
        }

        public override void PostAnimation()
        {
            _Sprite.Position = _EndPosition;
        }
    }
}
