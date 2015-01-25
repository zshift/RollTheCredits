using System;
using Cocos2D;
using RollTheCredits.Common;

namespace RollTheCredits.Entities
{
    public class Platform : CCSprite
    {
        public bool spawnPlatform;
        public bool reachedSpawnPoint;
        public bool hasExplosion;
        public bool isAlive;

        private Level Level;

        public int SpawnTimer = 200;

        public PlatformTypes PlatformType;
        public Platform(string filename, PlatformTypes platformType, CCPoint position)
        {
            base.InitWithFile(filename);
            PlatformType = platformType;
            Position = position;
        }

        public Platform(Level level, PlatformTypes platformType)
        {
            Level = level;
            PlatformType = platformType;
            int randomTileIndex = Level.Rand.Next(5);
            switch (PlatformType)
            {
                case PlatformTypes.Top:
                    switch (randomTileIndex)
                    {
                        case 0:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1255, 200, 32, 32));
                            break;
                        case 1:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1290, 200, 32, 32));
                            break;
                        case 2:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1325, 200, 32, 32));
                            break;
                        case 3:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1360, 200, 32, 32));
                            break;
                        case 4:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1220, 200, 32, 32));
                            break;
                        default:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1360, 200, 32, 32));
                            break;
                    }

                    break;
                case PlatformTypes.Middle:
                    switch (randomTileIndex)
                    {
                        case 0:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1220, 242, 32, 32));
                            break;
                        case 1:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1255, 242, 32, 32));
                            break;
                        case 2:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1290, 242, 32, 32));
                            break;
                        case 3:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1325, 242, 32, 32));
                            break;
                        case 4:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1360, 242, 32, 32));
                            break;
                        default:
                            base.InitWithTexture(level.PlayerCharacterSheet.Texture, new CCRect(1220, 242, 32, 32));
                            break;
                    }
                    break;
            }

            Reset();
        }

        public void Reset(CCPoint position)
        {
            Position = position;
        }

        public void Reset()
        {
            spawnPlatform = false;
            reachedSpawnPoint = false;
            hasExplosion = false;
            isAlive = true;
            Visible = true;
            Rotation = 90;
            SpawnTimer = 4;
            switch (PlatformType)
            {
                case PlatformTypes.Top:
                    Position = new CCPoint(((BoundingBox.Size.Width) * (40)) + (BoundingBox.Size.Width / 2),
                                           (BoundingBox.Size.Height / 2) + BoundingBox.Size.Height);
                    RunAction(new CCRotateTo(0.5f, 0));
                    break;
                case PlatformTypes.Middle:
                    Position = new CCPoint(((BoundingBox.Size.Width) * (40)) + (BoundingBox.Size.Width / 2),
                                           (BoundingBox.Size.Height / 2));
                    RunAction(new CCRotateTo(0.5f, 0));
                    break;
            }
        }

        public void Update()
        {
            if (PlatformType == PlatformTypes.Top || PlatformType == PlatformTypes.Middle)
            {
                UpdateTiles();
            }



            if (!Visible)
            {
                return;
            }

            float direction = 0;

            switch (PlatformType)
            {
                case PlatformTypes.YouWin:
                    direction = -1;
                    break;
                case PlatformTypes.Large:
                    direction = 1;
                    break;
            }

            Position += new CCPoint(0, 5 * direction);

            if (Position.Y > AppDelegate.GameHeight + 200 ||
                PositionY < -200)
            {
                Visible = false;
            }
        }

        private void UpdateTiles()
        {
            try
            {
                SpawnTimer--;
                if (hasExplosion)
                {
                    Position = new CCPoint(Position.X + Level.PlatformVelocity, Position.Y + (Level.PlatformVelocity / 6));
                }
                else
                {
                    Position = new CCPoint(Position.X - 5, Position.Y);
                }

                if (Position.X - BoundingBox.Size.Width / 2 < -BoundingBox.Size.Width)
                {
                    isAlive = false;
                }
            }
            catch (Exception exception)
            {

            }
        }
    }
}
