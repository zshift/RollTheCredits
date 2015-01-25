namespace RollTheCredits.Entities.Fonts
{
    public class BirthdayBalloon : Font
    {
        public override string File
        {
            get { return "birthday balon tfb"; }
        }

        public override float VerticalSpeedScalar
        {
            get { return 1.5f; }
        }

        public override float HorizontalSpeedScalar
        {
            get
            {
                switch(SecondsPassed) {
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
            get { return (int) ((timer.ElapsedMilliseconds / 250) % 10); }
        }
    }
}