namespace RollTheCredits.Entities.Fonts
{
    public class Budmo : Font
    {
        private static readonly string[] Variants =
        {
            "Jiggler", "Jigglish"
        };

        private static string Variant
        {
            get { return Variants[randomizer.Next(0, Variants.Length)]; }
        }

        public override string File
        {
            get { return string.Format("Budmo {0}", Variant); }
        }
    }
}