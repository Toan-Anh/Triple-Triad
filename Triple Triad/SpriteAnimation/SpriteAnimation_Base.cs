using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Triple_Triad.SpriteAnimation
{
    /// <summary>
    /// Abstract Decorator class that allows animations to be added to sprites
    /// </summary>
    abstract class SpriteAnimation_Base
    {
        protected Sprite _Sprite;

        protected int _AnimationDelayInterval;
        protected int _AnimationDelayCount = 0;
        protected int _AnimationInterval;
        protected int _AnimationCount = 0;

        protected AnimationState _AnimationState = AnimationState.Waiting;
        public AnimationState AnimationState
        {
            get { return _AnimationState; }
            set { _AnimationState = value; }
        }

        protected FireTime _FireTime = FireTime.None;
        public FireTime FireTime
        {
            get { return _FireTime; }
            set { _FireTime = value; }
        }

        protected List<SpriteAnimation_Base> _SiblingAnimation = new List<SpriteAnimation_Base>();

        /// <summary>
        /// Contructor for the animation
        /// </summary>
        /// <param name="sprite">The sprite to be animated</param>
        /// <param name="delay">The animation delay time in miliseconds. The default value is 0.</param>
        /// <param name="animationInterval">The animation time in miliseconds. The default value is 1000.</param>
        public SpriteAnimation_Base(Sprite sprite, int delay = 0, int animationInterval = 1000)
        {
            _Sprite = sprite;
            _AnimationDelayInterval = delay;
            _AnimationInterval = animationInterval;
        }

        public virtual void AddSibling(SpriteAnimation_Base sibling)
        {
            _SiblingAnimation.Add(sibling);
        }

        public virtual void Update(GameTime gameTime)
        {
            //_Sprite.Update(gameTime);

            if (_AnimationState == AnimationState.Waiting)
            {
                if (_AnimationDelayCount >= _AnimationDelayInterval)
                    PreAnimation();
            }
            else if (_AnimationState == AnimationState.Animate)
            {
                StartSiblingAnimations(FireTime.AtStart);
                if (_AnimationDelayCount < _AnimationDelayInterval)
                    _AnimationDelayCount += gameTime.ElapsedGameTime.Milliseconds;
                else
                {
                    if (_AnimationCount == 0)
                        Global.SFXManager.Card.Play();
                    if (_AnimationCount < _AnimationInterval)
                    {
                        _AnimationCount = MathHelper.Clamp(_AnimationCount + gameTime.ElapsedGameTime.Milliseconds, 0, _AnimationInterval);
                        AnimationLogic();
                    }
                    else
                    {
                        PostAnimation();
                        _AnimationState = AnimationState.End;
                        StartSiblingAnimations(FireTime.AtEnd);
                    }
                }
            }
        }

        protected virtual void StartSiblingAnimations(FireTime fireTime)
        {
            foreach (SpriteAnimation_Base animation in _SiblingAnimation)
            {
                if (animation._FireTime == fireTime)
                {
                    if (animation.AnimationState == AnimationState.End)
                        animation.Reset();
                    if (animation.AnimationState == AnimationState.Waiting)
                        animation.AnimationState = AnimationState.Animate;
                }
            }
        }

        public virtual void Reset()
        {
            _AnimationDelayCount = 0;
            _AnimationCount = 0;
            _AnimationState = AnimationState.Waiting;
            PreAnimation();
        }

        public abstract void PreAnimation();
        public abstract void AnimationLogic();
        public abstract void PostAnimation();
    }

    public enum AnimationState
    {
        Waiting, Animate, End, 
    }
}
