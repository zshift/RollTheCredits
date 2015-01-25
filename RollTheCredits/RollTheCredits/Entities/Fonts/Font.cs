using System;
using System.Diagnostics;
using Cocos2D;

namespace RollTheCredits.Entities.Fonts
{
    public abstract class Font
    {
        protected static Random randomizer = new Random();
        protected Stopwatch timer;

        protected Font()
        {
            timer = new Stopwatch();
            timer.Start();
        }


        public abstract string File { get; }

        public virtual int Size { get { return 48; } }
        public virtual CCColor3B Color { get { return CCColor3B.White; } }
        public virtual float VerticalSpeedScalar { get { return 1; } }
        public virtual float HorizontalSpeedScalar { get { return 0; } }
        public virtual int HorizontalOffset { get { return 0; } }
    }
}
