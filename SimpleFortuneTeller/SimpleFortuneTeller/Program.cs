using SimpleFortuneTeller.Handling;
using SimpleFortuneTeller.Rendering;
using System;
using System.Collections.Generic;

namespace SimpleFortuneTeller
{
    class Program
    {
        static void Main(string[] args)
        {
            var fortunes = new List<FortuneOption>
            {
                new FortuneOption
                {
                    OptionDisplay = "Harry Potter",
                    ForegroundColor = ConsoleColor.Yellow,
                    BackgroundColor = ConsoleColor.DarkRed,
                    NextOptions = new List<FortuneOption>
                    {
                        new FortuneOption
                        {
                            OptionDisplay = "Cedric",
                            ForegroundColor = ConsoleColor.DarkBlue,
                            BackgroundColor = ConsoleColor.Yellow,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Draco",
                            ForegroundColor = ConsoleColor.White,
                            BackgroundColor = ConsoleColor.DarkGreen,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Cho Chang",
                            ForegroundColor = ConsoleColor.Cyan,
                            BackgroundColor = ConsoleColor.DarkBlue,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Luna Lovegood",
                            ForegroundColor = ConsoleColor.DarkYellow,
                            BackgroundColor = ConsoleColor.Blue,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Myrtle",
                            ForegroundColor = ConsoleColor.White,
                            BackgroundColor = ConsoleColor.DarkCyan,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Madam Pomfrey",
                            ForegroundColor = ConsoleColor.Black,
                            BackgroundColor = ConsoleColor.White,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                    },
                },
                new FortuneOption
                {
                    OptionDisplay = "Dexter's Laboratory",
                    ForegroundColor = ConsoleColor.Green,
                    BackgroundColor = ConsoleColor.DarkBlue,
                    NextOptions = new List<FortuneOption>
                    {
                        new FortuneOption
                        {
                            OptionDisplay = "Dexter",
                            ForegroundColor = ConsoleColor.White,
                            BackgroundColor = ConsoleColor.DarkBlue,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Dee Dee",
                            ForegroundColor = ConsoleColor.Yellow,
                            BackgroundColor = ConsoleColor.Magenta,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Mandark",
                            ForegroundColor = ConsoleColor.Blue,
                            BackgroundColor = ConsoleColor.DarkYellow,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                    },
                },
                new FortuneOption
                {
                    OptionDisplay = "My Hero Academia",
                    ForegroundColor = ConsoleColor.DarkBlue,
                    BackgroundColor = ConsoleColor.Yellow,
                    NextOptions = new List<FortuneOption>
                    {
                        new FortuneOption
                        {
                            OptionDisplay = "Deku",
                            ForegroundColor = ConsoleColor.DarkBlue,
                            BackgroundColor = ConsoleColor.Green,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Bakugo",
                            ForegroundColor = ConsoleColor.Red,
                            BackgroundColor = ConsoleColor.DarkCyan,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Dabi",
                            ForegroundColor = ConsoleColor.White,
                            BackgroundColor = ConsoleColor.DarkBlue,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Kirishima",
                            ForegroundColor = ConsoleColor.Cyan,
                            BackgroundColor = ConsoleColor.Red,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                    },
                },
                new FortuneOption
                {
                    OptionDisplay = "Disney",
                    ForegroundColor = ConsoleColor.White,
                    BackgroundColor = ConsoleColor.Blue,
                    NextOptions = new List<FortuneOption>
                    {
                        new FortuneOption
                        {
                            OptionDisplay = "Tinkerbell",
                            ForegroundColor = ConsoleColor.Yellow,
                            BackgroundColor = ConsoleColor.DarkGreen,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Aladdin",
                            ForegroundColor = ConsoleColor.Black,
                            BackgroundColor = ConsoleColor.Blue,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Ariel",
                            ForegroundColor = ConsoleColor.Green,
                            BackgroundColor = ConsoleColor.DarkRed,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Prince Charming",
                            ForegroundColor = ConsoleColor.Yellow,
                            BackgroundColor = ConsoleColor.DarkBlue,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Rapunzel",
                            ForegroundColor = ConsoleColor.Yellow,
                            BackgroundColor = ConsoleColor.DarkMagenta,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Gaston",
                            ForegroundColor = ConsoleColor.DarkYellow,
                            BackgroundColor = ConsoleColor.DarkRed,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                    },
                },
                new FortuneOption
                {
                    OptionDisplay = "Lord of the Rings",
                    ForegroundColor = ConsoleColor.Yellow,
                    BackgroundColor = ConsoleColor.DarkGreen,
                    NextOptions = new List<FortuneOption>
                    {
                        new FortuneOption
                        {
                            OptionDisplay = "Smeagol",
                            ForegroundColor = ConsoleColor.Red,
                            BackgroundColor = ConsoleColor.DarkCyan,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Legolas",
                            ForegroundColor = ConsoleColor.Yellow,
                            BackgroundColor = ConsoleColor.DarkGreen,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Tauriel",
                            ForegroundColor = ConsoleColor.Red,
                            BackgroundColor = ConsoleColor.DarkBlue,
                            NextOptions = FinaleBuilder.RandomizeFinaleOptions(),
                        },
                    },
                },
            };
            FortuneLogic.AddFortuneOptions(fortunes);
            while (true)
            {
                FortuneRenderer.Render();
                if (Console.KeyAvailable)
                {
                    var result = FortuneLogic.KeyPress(Console.ReadKey().Key);
                    if (result == FortuneOptionEvent.FinalSelectionMade)
                    {
                        FortuneRenderer.SetSelectionResults();
                    }
                }
            }
        }
    }
}
