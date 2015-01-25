using Cocos2D;

namespace RollTheCredits.Entities
{
    public class Question : CCLabel
    {
        private readonly string question;

        public Question(string question, CCPoint startPosition, int fontSize = 40)
        {
            this.question = question;
            InitWithString(this.question, "OpenSans-Bold", fontSize);
            Position = startPosition;
        }

        public void Update()
        {
            this.Position += new CCPoint(0, 7f);
        }
    }
}