using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Triple_Triad.SpriteAnimation
{
    class SpriteAnimation_Rotate : SpriteAnimation_Base
    {
        private float _StartAngle;
        private float _EndAngle;
        private Vector2 _Center;
        private Vector2 _OriginalCenter;

        /// <summary>
        /// Contructor for the rotating animation. 
        /// Be careful when using this animation because the center of the sprite will be changed according to what you've specified
        /// </summary>
        /// <param name="sprite">The sprite to be animated</param>
        /// <param name="startAngle">The angle (radian) used at the start of the animation.</param>
        /// <param name="endAngle">The angle (radian) used at the end of the animation.</param>
        /// <param name="center">The rotation center</param>
        /// <param name="delay">The animation delay time in miliseconds. The default value is 0.</param>
        /// <param name="time">The animation time in miliseconds. The default value is 1000.</param>
        public SpriteAnimation_Rotate(Sprite sprite, float startAngle, float endAngle, Vector2 center, int delay = 0, int time = 1000) : base(sprite, delay, time)
        {
            _StartAngle = startAngle;
            _EndAngle = endAngle;
            _Center = center;
        }

        /// <summary>
        /// Contructor for the rotating animation
        /// Be careful when using this animation because the center of the sprite will be changed according to what you've specified
        /// </summary>
        /// <param name="sprite">The sprite to be animated</param>
        /// <param name="startAngle">The angle (radian) used at the start of the animation.</param>
        /// <param name="endAngle">The angle (radian) used at the end of the animation.</param>
        /// <param name="center">The rotation center</param>
        /// <param name="delay">The animation delay time in miliseconds. The default value is 0.</param>
        /// <param name="time">The animation time in miliseconds. The default value is 1000.</param>
        public SpriteAnimation_Rotate(Sprite sprite, int startAngle, int endAngle, Vector2 center, int delay = 0, int time = 1000) : base(sprite, delay, time)
        {
            _StartAngle = (float)(Math.PI * startAngle / 180);
            _EndAngle = (float)(Math.PI * endAngle / 180);
            _Center = center;
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
            if (_Sprite.Center != _Center)
            {
                _OriginalCenter = _Sprite.Center;
                _Sprite.Center = _Center;
            }
            _Sprite.Rotation = _StartAngle;
        }

        public override void AnimationLogic()
        {
            if (_Sprite.Center != _Center)
            {
                _OriginalCenter = _Sprite.Center;
                _Sprite.Center = _Center;
            }
            _Sprite.Rotation = _StartAngle + ((float)_AnimationCount / _AnimationInterval) * (_EndAngle - _StartAngle);
        }

        public override void PostAnimation()
        {
            _Sprite.Rotation = _EndAngle;
        }
    }
}
