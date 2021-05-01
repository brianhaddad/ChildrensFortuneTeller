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
                    OptionDisplay = "Option 1",
                    ForegroundColor = ConsoleColor.White,
                    BackgroundColor = ConsoleColor.DarkBlue,
                    NextOptions = new List<FortuneOption>
                    {
                        new FortuneOption
                        {
                            OptionDisplay = "Option A",
                            ForegroundColor = ConsoleColor.Green,
                            BackgroundColor = ConsoleColor.Black,
                        },
                        new FortuneOption
                        {
                            OptionDisplay = "Option B",
                            ForegroundColor = ConsoleColor.Green,
                            BackgroundColor = ConsoleColor.Black,
                        },
                    },
                },
                new FortuneOption
                {
                    OptionDisplay = "Option 2",
                    ForegroundColor = ConsoleColor.Black,
                    BackgroundColor = ConsoleColor.Cyan,
                },
                new FortuneOption
                {
                    OptionDisplay = "Really Long Option 3",
                    ForegroundColor = ConsoleColor.Cyan,
                    BackgroundColor = ConsoleColor.DarkYellow,
                },
                new FortuneOption
                {
                    OptionDisplay = "Option 4",
                    ForegroundColor = ConsoleColor.Green,
                    BackgroundColor = ConsoleColor.DarkBlue,
                },
                new FortuneOption
                {
                    OptionDisplay = "Option 5",
                    ForegroundColor = ConsoleColor.White,
                    BackgroundColor = ConsoleColor.DarkMagenta,
                },
                new FortuneOption
                {
                    OptionDisplay = "Option 6",
                    ForegroundColor = ConsoleColor.Magenta,
                    BackgroundColor = ConsoleColor.Black,
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
