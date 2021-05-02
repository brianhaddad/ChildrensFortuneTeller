using System;
using System.Collections.Generic;

namespace SimpleFortuneTeller.Handling
{
    public static class FinaleBuilder
    {
        private static readonly Random random = new Random();
        private static int GetRandom(int min, int max) => random.Next(min, max);
        private static int GetRandom(int max) => random.Next(max);
        private static T GetRandom<T>(this T[] collection) => collection[random.Next(collection.Length)];
        private static List<int> NumberPool;

        private static int GetNumberFromPool()
        {
            var selection = NumberPool.ToArray().GetRandom();
            NumberPool.Remove(selection);
            return selection;
        }

        private static void ResetNumberPool()
        {
            NumberPool = new List<int>();
            for (var i = 0; i < FinaleOptions.Length + RandomAdditions.Length; i++)
            {
                NumberPool.Add(i);
            }
        }

        private static readonly string[] FinaleOptions = new[]
        {
            "You now live in the world of {0} and you're married to {1}. {2}",
            "The world of {0} has banished you forever. {1} was left behind when you left, has fallen ill, and isn't getting any better. You've started a family with someone else, and {1} is only a memory now. {2}",
            "{1} welcomes you to the world of {0} but isn't emotionally available so you start a family with someone random there. {2}",
            "You arrive in the world of {0} and after a few years you finally work up the courage to talk to {1}. You bungle the encounter and end up marrying someone else. {2}",
            "Your life is a magical {0} \"Happily Ever After\" with {1}. {2}",
            "Your dreams come true in the world of {0}. You marry {1}. {2}",
            "When you arrive in the world of {0} you see {1} waiting to greet you. {1} says, \"{2}\" You stagger back a bit and smile. It doesn't matter. You know you will one day marry {1}.",
            "{1} watches as you trip and fall face first into a pile of mud. {2} You aren't all that concerned though because it turns out the world of {0} is real. {1} smiles at you.",
            "{2} You feel like you've really settled in to the world of {0}. {1} really likes you and hopes you'll stay.",
        };

        private static readonly string[] RandomAdditions = new[]
        {
            "You have a big house.",
            "Your kids are hideously ugly.",
            "You have very pretty clothes.",
            "You have a good dog.",
            "Your kids are the rulers of the land.",
            "Your neighbors trample your flowers frequently.",
            "You are, unfortunately, blind.",
            "You enjoy wealth, comfort, and plenty of cheeseburgers.",
            "You plan on staying here forever because you're so happy here.",
        };

        private static readonly ConsoleColor[] LightColors = new[]
        {
            ConsoleColor.Blue,
            ConsoleColor.Cyan,
            ConsoleColor.Gray,
            ConsoleColor.Green,
            ConsoleColor.Magenta,
            ConsoleColor.Red,
            ConsoleColor.White,
            ConsoleColor.Yellow,
        };

        private static readonly ConsoleColor[] DarkColors = new[]
        {
            ConsoleColor.Black,
            ConsoleColor.DarkBlue,
            ConsoleColor.DarkCyan,
            ConsoleColor.DarkGray,
            ConsoleColor.DarkGreen,
            ConsoleColor.DarkMagenta,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkYellow,
        };

        public static string Build()
        {
            var selections = FortuneLogic.GetSelections().ToArray();
            var finalNumber = int.Parse(selections[selections.Length - 1]);
            string finaleFormat;
            string randomAddition;
            if (finalNumber > FinaleOptions.Length && finalNumber > RandomAdditions.Length)
            {
                if (GetRandom(100) % 2 == 0)
                {
                    finaleFormat = FinaleOptions[finalNumber - RandomAdditions.Length];
                    randomAddition = RandomAdditions.GetRandom();
                }
                else
                {
                    randomAddition = RandomAdditions[finalNumber - FinaleOptions.Length];
                    finaleFormat = FinaleOptions.GetRandom();
                }
            }
            else if (finalNumber < Math.Max(RandomAdditions.Length, FinaleOptions.Length))
            {
                if (FinaleOptions.Length > RandomAdditions.Length)
                {
                    finaleFormat = FinaleOptions[finalNumber];
                    randomAddition = RandomAdditions.GetRandom();
                }
                else
                {
                    randomAddition = RandomAdditions[finalNumber];
                    finaleFormat = FinaleOptions.GetRandom();
                }
            }
            else
            {
                if (finalNumber < FinaleOptions.Length)
                {
                    finaleFormat = FinaleOptions[finalNumber];
                    randomAddition = RandomAdditions.GetRandom();
                }
                else
                {
                    randomAddition = RandomAdditions[finalNumber];
                    finaleFormat = FinaleOptions.GetRandom();
                }
            }
            selections[^1] = randomAddition;
            return string.Format(finaleFormat, selections);
        }

        public static List<FortuneOption> RandomizeFinaleOptions()
        {
            ResetNumberPool();
            var result = new List<FortuneOption>();
            var num = GetRandom(2, 8);
            for (var i = 0; i < num; i++)
            {
                if (i % 2 == 0)
                {
                    result.Add(RandomizeFinaleOptionsLightOnDark());
                }
                else
                {
                    result.Add(RandomizeFinaleOptionsDarkOnLight());
                }
            }
            return result;
        }

        private static FortuneOption RandomizeFinaleOptionsLightOnDark()
            => new FortuneOption
            {
                OptionDisplay = GetNumberFromPool().ToString(),
                ForegroundColor = LightColors.GetRandom(),
                BackgroundColor = DarkColors.GetRandom(),
            };

        private static FortuneOption RandomizeFinaleOptionsDarkOnLight()
            => new FortuneOption
            {
                OptionDisplay = GetNumberFromPool().ToString(),
                ForegroundColor = DarkColors.GetRandom(),
                BackgroundColor = LightColors.GetRandom(),
            };
    }
}
