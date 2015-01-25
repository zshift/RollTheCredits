using System;
using System.Diagnostics;

namespace RollTheCredits.NameGenerator
{
    public static class Names
    {
        private class Odds
        {
            private const double DevOdds = .05,
                                 ContributerOdds = .025,
                                 BadassOdds = .001,
                                 GenderOdds = .50,
                                 MiddleOdds = .10;

            public const double Dev = 1 - DevOdds,
                                Contributer = Dev - ContributerOdds,
                                Badass = Contributer - BadassOdds,
                                Gender = 1 - GenderOdds,
                                Middle = 1 - MiddleOdds;
        }

        private static Stopwatch timer;
        private static readonly Random randomizer = new Random();

        private static readonly string[]
            Male = Read("Male"),
            Female = Read("Female"),
            Last = Read("Last"),
            Full = Read("Full");

        public static string Next()
        {
            if (timer == null)
            {
                timer = new Stopwatch();
                timer.Start();
            }

            var odds = Percentage;

            if (odds >= Odds.Dev) return ChooseFrom(Dev);
            if (odds >= Odds.Contributer) return ChooseFrom(Contributer);
            if (odds >= Odds.Badass) return ChooseFrom(Badass);

            var gender = Gender;
            return ChooseFrom(gender) + " "
                 + (Middle ? ChooseFrom(gender) + " " : string.Empty)
                 + ChooseFrom(Last);
        }

        private static ArraySegment<string> Dev
        {
            get { return Middle ? Full.Slice(3, 3) : Full.Slice(0, 3); }
        }

        private static ArraySegment<string> Contributer
        {
            get { return Middle ? Full.Slice(9, 3) : Full.Slice(6, 3); }
        }

        private static ArraySegment<string> Badass
        {
            get { return Full.Slice(12); }
        }

        private static string[] Gender
        {
            get { return Percentage >= Odds.Gender ? Male : Female; }
        }

        private static bool Middle
        {
            get { return Percentage >= Odds.Middle; }
        }

        private static double Percentage
        {
            get { return randomizer.NextDouble(); }
        }

        private static string ChooseFrom(string[] names)
        {
            var offset = timer.ElapsedMilliseconds/1000;
            return names[(offset + RandomIndex(25)) % 100];
        }

        private static string ChooseFrom(ArraySegment<string> names)
        {
            return names.Array[names.Offset + RandomIndex(names.Count)];
        }

        private static int RandomIndex(int length)
        {
            return (int) Math.Floor(length * Percentage);
        }

        private static string[] Read(string filename)
        {
            return System.IO.File.ReadAllLines(string.Format("NameGenerator/{0}.txt", filename));
        }
    }

    public static class StringArrayExtensions
    {
        public static ArraySegment<string> Slice(this string[] list, int offset, int? count = null)
        {
            var length = count ?? list.Length - offset;
            return new ArraySegment<string>(list, offset, length);
        }
    }
}