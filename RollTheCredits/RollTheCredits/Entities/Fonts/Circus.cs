using Cocos2D;

namespace RollTheCredits.Entities.Fonts
{
    public class Circus : Font
    {
        public override string File
        {
            get { return "Circus"; }
        }

        public override CCColor3B Color
        {
            get { return new CCColor3B(232, 77, 0); }
        }
    }
}