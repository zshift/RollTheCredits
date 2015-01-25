using System;
using System.Diagnostics;
using Cocos2D;
using RollTheCredits.Entities.Fonts;

namespace RollTheCredits.Entities
{
    public class NameText : CCLabel
    {
        protected static Random randomizer = new Random();
        private int timeOffset;

        public bool IsAlive;
        private Level level;
        public bool hasCollided = false;

        private bool MoveDown;
        private bool MoveUp;

        protected Stopwatch timer;

        public NameText(Level level, string text, CCPoint position)
        {
            this.level = level;
            timer = new Stopwatch();
            timer.Start();
            timeOffset = randomizer.Next();
            InitWithString(text, string.Empty, 48);
            Color = CCColor3B.White;
            Position = position;
        }

        public void Reset(string text, CCPoint position)
        {
            Position = position;
            Text = text;
            IsAlive = true;
            hasCollided = false;
            MoveDown = true;
            MoveUp = false;
            Visible = true;
        }

        public void UpdateRollingName()
        {
            Position += new CCPoint(1 * HorizontalSpeedScalar, 2.5f);
        }

        public void UpdateAttackingName()
        {
            Position += new CCPoint(-5, 0);

            if (Position.Y > AppDelegate.GameHeight && !MoveDown && MoveUp)
            {
                MoveDown = true;
                MoveUp = false;
            }

            if (Position.Y < -200 && MoveDown && !MoveUp)
            {
                MoveUp = true;
                MoveDown = false;
            }

            if (MoveDown)
            {
                Position += new CCPoint(0, -10);
            }

            if (MoveUp)
            {
                Position += new CCPoint(0, 10);
            }

            if (level.GetPlayer().BoundingBox.IntersectsRect(BoundingBox) && !hasCollided) //&& !level.GetPlayer().isBlinking)
            {
                if (level.GetPlayer().PositionX > PositionX - 5 && level.GetPlayer().PositionX < PositionX + BoundingBox.Size.Width / 4)
                {
                    hasCollided = true;
                    level.GetPlayer().IsBlinking = true;

                }
            }
        }

        public float HorizontalSpeedScalar
        {
            get
            {
                switch (SecondsPassed)
                {
                    case 1:
                    case 4:
                        return .5f;
                    case 2:
                    case 3:
                        return 1;
                    case 6:
                    case 9:
                        return -.5f;
                    case 7:
                    case 8:
                        return -1f;
                    default:
                        return 0;
                }
            }
        }

        private int SecondsPassed
        {
            get { return (int)(((timer.ElapsedMilliseconds + timeOffset) / 250) % 10); }
        }
    }
}
