using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Triple_Triad.SpriteAnimation;
using Microsoft.Xna.Framework.Input;

namespace Triple_Triad
{
    public enum FireTime
    {
        None, AtStart, AtEnd, 
    }

    abstract class Scene
    {
        protected List<VisibleEntity> _SceneEntities;
        protected Dictionary<string, SpriteAnimation_Base> _Animations;

        public Scene()
        {
            _SceneEntities = new List<VisibleEntity>();
            _Animations = new Dictionary<string, SpriteAnimation_Base>();
        }

        public virtual void AddEntity(VisibleEntity entity)
        {
            if (!_SceneEntities.Contains(entity))
                _SceneEntities.Add(entity);
        }

        public virtual void RegisterAnimation(string name, SpriteAnimation_Base animation)
        {
            if (!_Animations.ContainsKey(name))
                _Animations.Add(name, animation);
        }

        public virtual void RegisterAnimation(string name, SpriteAnimation_Base animation, string cousinAnimation, FireTime fireTime)
        {
            animation.FireTime = fireTime;
            if (!_Animations.ContainsKey(name) && _Animations.ContainsKey(cousinAnimation))
            {
                _Animations[cousinAnimation].AddSibling(animation);
                _Animations.Add(name, animation);
            }
        }

        public virtual void StartAnimation(string name)
        {
            if (_Animations.ContainsKey(name))
            {
                if (_Animations[name].AnimationState == AnimationState.End)
                    _Animations[name].Reset();
                if (_Animations[name].AnimationState == AnimationState.Waiting)
                    _Animations[name].AnimationState = AnimationState.Animate;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (VisibleEntity entity in _SceneEntities)
                entity.Update(gameTime);

            foreach (SpriteAnimation_Base animation in _Animations.Values)
                animation.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            Global.SpriteBatch.Begin();
            foreach (VisibleEntity entity in _SceneEntities)
                entity.Draw(gameTime);
            Global.SpriteBatch.End();
        }
    }
}
