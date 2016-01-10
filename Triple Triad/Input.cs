using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace Triple_Triad
{
    static class Input
    {
        public static Keys Accept
        {
            get { return Keys.Z; }
        }
        public static Keys Cancel
        {
            get { return Keys.X; }
        }
        public static Keys Menu
        {
            get { return Keys.Enter; }
        }
        public static Keys L1
        {
            get { return Keys.A; }
        }
        public static Keys R1
        {
            get { return Keys.S; }
        }

        public static Keys Up
        {
            get { return Keys.Up; }
        }
        public static Keys Down
        {
            get { return Keys.Down; }
        }
        public static Keys Left
        {
            get { return Keys.Left; }
        }
        public static Keys Right
        {
            get { return Keys.Right; }
        }
    }
}
