using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Triple_Triad
{
    class KeyboardManager
    {
        KeyboardState _PreviousKeyboardState;
        KeyboardState _CurrentKeyboardState;

        public void Update(GameTime gameTime)
        {
            _PreviousKeyboardState = _CurrentKeyboardState;
            _CurrentKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyPressedAndReleased(Keys key)
        {
            if (_PreviousKeyboardState.IsKeyDown(key) &&
                _CurrentKeyboardState.IsKeyUp(key))
                return true;
            return false;
        }

        public bool IsKeyDown(Keys key)
        {
            if (_CurrentKeyboardState.IsKeyDown(key))
                return true;
            return false;
        }

        public bool IsKeyUp(Keys key)
        {
            if (_CurrentKeyboardState.IsKeyUp(key))
                return true;
            return false;
        }
    }
}
