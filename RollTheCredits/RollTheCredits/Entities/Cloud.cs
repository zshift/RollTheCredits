using Cocos2D;
using RollTheCredits.Common;

namespace RollTheCredits.Entities
{
    public class Cloud : CCSprite
    {
        private Level Level;

        private CloudTypes Type;
        private CCPoint _velocityPoint;
        public bool IsAlive = true;

        public Cloud(Level level, CloudFiles file, CloudTypes type)
        {
            Level = level;
            Type = type;
            switch (file)
            {
                case CloudFiles.One:
                    base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1024, 595, 97, 67));
                    break;
                case CloudFiles.Two:
                    base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1135, 590, 123, 78));
                    break;
                case CloudFiles.Three:
                    base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1270, 588, 112, 87));
                    break;
            }
            _velocityPoint = new CCPoint(Level.PlatformVelocity / 2.0f, 0);
            Reset();
        }

        public void Reset()
        {
            IsAlive = true;
            switch (Type)
            {
                case CloudTypes.High:
                    base.Position = Level.HighCloudPoint;
                    break;
                case CloudTypes.Middle:
                    base.Position = Level.MidCloudPoint;
                    break;
                case CloudTypes.Low:
                    base.Position = Level.LowCloudPoint;
                    break;
            }
        }

        public void Update()
        {
            _velocityPoint.X = Level.PlatformVelocity / 2.0f;
            Position += _velocityPoint;
            if (Position.X + BoundingBox.Size.Width < 0)
            {
                IsAlive = false;
            }
        }
    }
}
