namespace RollTheCredits.Entities.Fonts
{
    public class OpenSans : Font
    {
        private static readonly string[] Variants =
        {
            "ExtraBold", "Light", "Semibold"
        };

        private static string Variant
        {
            get { return randomizer.NextDouble() >= .75
                ? string.Empty
                : " " + Variants[randomizer.Next(0, Variants.Length)]; }
        }

        public override string File
        {
            get { return string.Format("Open Sans", Variant); }
        }
    }
}