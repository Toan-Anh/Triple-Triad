using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Triple_Triad.SpriteAnimation
{
    class SpriteAnimation_Scale : SpriteAnimation_Base
    {
        private float _StartScale;
        private float _EndScale;

        /// <summary>
        /// Contructor for the scaling animation
        /// </summary>
        /// <param name="sprite">The sprite to be animated</param>
        /// <param name="delay">The animation delay time in miliseconds. The default value is 0.</param>
        /// <param name="time">The animation time in miliseconds. The default value is 1000.</param>
        /// <param name="startScale">The scale used at the start of the animation. The default value is 1.</param>
        /// <param name="endScale">The scale used at the end of the animation. The default value is 2.</param>
        public SpriteAnimation_Scale(Sprite sprite, int delay = 0, int time = 1000, float startScale = 1f, float endScale = 2f) : base(sprite, delay, time)
        {
            _StartScale= startScale;
            _EndScale = endScale;
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
            _Sprite.Scale = _StartScale;
        }

        public override void AnimationLogic()
        {
            _Sprite.Scale = _StartScale + ((float)_AnimationCount / _AnimationInterval) * (_EndScale - _StartScale);
        }

        public override void PostAnimation()
        {
            _Sprite.Scale = _EndScale;
        }
    }
}
