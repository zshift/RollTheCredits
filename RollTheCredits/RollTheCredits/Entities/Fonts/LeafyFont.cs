using Cocos2D;

namespace RollTheCredits.Entities.Fonts
{
    public class LeafyFont : Font
    {
        public override string File
        {
            get { return "Leafy font"; }
        }

        public override CCColor3B Color
        {
            get { return new CCColor3B(12, 134, 0); }
        }
    }
}