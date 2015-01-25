using Cocos2D;
using RollTheCredits.Common;

namespace RollTheCredits.Entities
{
    public class Spike : CCSprite
    {
        private CCPoint _velocity;
        private ObstacleTypes ObstacleType;
        public Level level;

        public bool isAlive = true;
        public bool hasCollided = false;

        public float PosY;

        public Spike(Level level)
        {
            this.level = level;
            _velocity = CCPoint.Zero;
            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(176, 217, 32, 29));
            Scale = 1.5f;
            PosY = 85;
        }

        public void Reset(int i)
        {
            isAlive = true;
            hasCollided = false;

            Position = new CCPoint(AppDelegate.GameWidth + (i * 32), PosY);

            Scale = 1.5f;

            var scaleUp = level.Rand.Next(10) % 2 == 0;
            if (scaleUp)
            {
                Scale = 3f;
            }
        }

        public void Update()
        {
            _velocity = new CCPoint(level.PlatformVelocity, 0);
            Position += _velocity;

            if (Position.X < -100)
            {
                isAlive = false;
            }

            if (level.GetPlayer().BoundingBox.IntersectsRect(BoundingBox) && !hasCollided) 
            {
               if (level.GetPlayer().PositionX > PositionX - 5 &&
                        level.GetPlayer().PositionX < PositionX + BoundingBox.Size.Width/4)
                    {
                        hasCollided = true;
                        level.GetPlayer().IsBlinking = true;
                    }
                }
            Visible = isAlive;
        }
    }
}
