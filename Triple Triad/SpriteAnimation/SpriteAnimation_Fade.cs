using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Triple_Triad.SpriteAnimation
{
    class SpriteAnimation_Fade : SpriteAnimation_Base
    {
        private byte _StartOpacity;
        private byte _EndOpacity;

        /// <summary>
        /// Contructor for the fading animation
        /// </summary>
        /// <param name="sprite">The sprite to be animated</param>
        /// <param name="delay">The animation delay time in miliseconds. The default value is 0.</param>
        /// <param name="time">The animation time in miliseconds. The default value is 1000.</param>
        /// <param name="startOpacity">The starting opacity. The default value is 0.</param>
        /// <param name="endOpacity">The ending opacity. The default value is 255.</param>
        public SpriteAnimation_Fade(Sprite sprite, int delay = 0, int time = 1000, byte startOpacity = 0, byte endOpacity = 255) : base(sprite, delay, time)
        {
            _StartOpacity = startOpacity;
            _EndOpacity = endOpacity;
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
            _Sprite.Opacity = _StartOpacity;
        }

        public override void AnimationLogic()
        {
            _Sprite.Opacity = (byte)MathHelper.Clamp(_StartOpacity + ((float)_AnimationCount / _AnimationInterval) * (_EndOpacity - _StartOpacity), 0, 255);
        }

        public override void PostAnimation()
        {
            _Sprite.Opacity = _EndOpacity;
        }
    }
}
