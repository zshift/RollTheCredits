namespace RollTheCredits.Entities.Fonts
{
    public class AdamGorry : Font
    {
        private static readonly string[] Variants =
        {
            "Inline", "Lights"
        };

        private static string Variant
        {
            get { return Variants[randomizer.Next(0, Variants.Length)]; }
        }

        public override string File
        {
            get { return string.Format("AdamGory-{0}", Variant); }
        }
    }
}