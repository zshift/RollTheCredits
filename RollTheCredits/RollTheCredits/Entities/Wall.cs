using Cocos2D;

namespace RollTheCredits.Entities
{
    public class Wall : CCSprite
    {
        private ushort hitCount;
        private bool touching;

        private readonly CCSprite HitOnce;
        private readonly CCSprite HitTwice;
        private readonly CCSprite HitThrice;
        private readonly CCSprite Broken;

        public bool IsBroken { get; private set;}

        public Wall(string filename, CCPoint position)
        {
            base.InitWithFile(filename);
            HitOnce = new CCSprite("sprites/wall_hitonce.png");
            HitTwice = new CCSprite("sprites/wall_hittwice.png");
            HitThrice = new CCSprite("sprites/wall_hitthrice.png");
            Broken = new CCSprite("sprites/wall_broken_top.png");
            Position = position;
        }

        public void Update()
        {
            switch (hitCount)
            {
                case 1:
                    Texture = HitOnce.Texture;
                    break;
                case 2:
                    Texture = HitTwice.Texture;
                    break;
                case 3:
                    Texture = HitThrice.Texture;
                    break;
                case 4:
                    this.IsBroken = true;
                    Texture = Broken.Texture;
                    break;
            }
        }

        public void Hitting()
        {
            if (touching || this.IsBroken) return;

            touching = true;
            hitCount++;
        }

        public void StopHitting()
        {
            touching = false;
        }
    }
}