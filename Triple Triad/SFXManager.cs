using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;

namespace Triple_Triad
{
    class SFXManager
    {
        private SoundEffect _Select;
        private SoundEffect _Confirm;
        private SoundEffect _Cancel;
        private SoundEffect _Card;
        private SoundEffect _CardWon;
        private SoundEffect _FlyIn;

        public SoundEffect Select
        {
            get { return _Select; }
        }
        public SoundEffect Confirm
        {
            get { return _Confirm; }
        }
        public SoundEffect Cancel
        {
            get { return _Cancel; }
        }
        public SoundEffect Card
        {
            get { return _Card; }
        }
        public SoundEffect CardWon
        {
            get { return _CardWon; }
        }

        public SoundEffect FlyIn
        {
            get { return _FlyIn; }
        }

        public void Initialize()
        {
            _Select = Global.Content.Load<SoundEffect>("card_touch");
            _Confirm = Global.Content.Load<SoundEffect>("card_touch");
            _Cancel = Global.Content.Load<SoundEffect>("card_touch");
            _Card = Global.Content.Load<SoundEffect>("card_touch");
            _CardWon = Global.Content.Load<SoundEffect>("card_win");
            _FlyIn = Global.Content.Load<SoundEffect>("FlyIn");
        }
    }
}
