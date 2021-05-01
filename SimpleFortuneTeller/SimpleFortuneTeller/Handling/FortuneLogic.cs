using SimpleFortuneTeller.Rendering;
using System;
using System.Collections.Generic;

namespace SimpleFortuneTeller.Handling
{
    public static class FortuneLogic
    {
        private static readonly List<string> Selections = new List<string>();
        private static List<FortuneOption> FortuneOptions = new List<FortuneOption>();
        private static int CurrentFortuneOption = 0;

        public static void AddFortuneOptions(List<FortuneOption> fortuneOptions)
        {
            FortuneOptions.AddRange(fortuneOptions);
            SetHighlightedFortuneOption();
        }

        public static List<string> GetSelections() => Selections;
        public static List<FortuneOption> GetFortuneOptions() => FortuneOptions;

        public static FortuneOptionEvent KeyPress(ConsoleKey consoleKey)
        {
            var (layoutCols, layoutRows) = FortuneRenderer.GetLayoutColsRows();
            var selectionX = CurrentFortuneOption % layoutCols;
            var selectionY = CurrentFortuneOption / layoutCols;

            var thisEvent = FortuneOptionEvent.Nothing;
            switch (consoleKey)
            {
                case ConsoleKey.LeftArrow:
                    if (selectionX - 1 >= 0)
                    {
                        selectionX -= 1;
                        thisEvent = FortuneOptionEvent.OnScreenSelectionChange;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (selectionX + 1 <= layoutCols)
                    {
                        selectionX += 1;
                        thisEvent = FortuneOptionEvent.OnScreenSelectionChange;
                    }
                    break;

                case ConsoleKey.UpArrow:
                    if (selectionY - 1 >= 0)
                    {
                        selectionY -= 1;
                        thisEvent = FortuneOptionEvent.OnScreenSelectionChange;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (selectionY + 1 <= layoutRows)
                    {
                        selectionY += 1;
                        thisEvent = FortuneOptionEvent.OnScreenSelectionChange;
                    }
                    break;

                case ConsoleKey.Spacebar:
                    Selections.Add(FortuneOptions[CurrentFortuneOption].OptionDisplay);
                    if (FortuneOptions[CurrentFortuneOption].NextOptions.Count > 0)
                    {
                        FortuneOptions = FortuneOptions[CurrentFortuneOption].NextOptions;
                        CurrentFortuneOption = 0;
                        FortuneOptions[CurrentFortuneOption].IsHighlighted = true;
                        return FortuneOptionEvent.SelectionMade;
                    }
                    else
                    {
                        return FortuneOptionEvent.FinalSelectionMade;
                    }
            }
            var nextSelection = (selectionY * layoutCols) + selectionX;
            if (nextSelection >= 0 && nextSelection < FortuneOptions.Count)
            {
                CurrentFortuneOption = nextSelection;
                SetHighlightedFortuneOption();
                return thisEvent;
            }
            return FortuneOptionEvent.Nothing;
        }

        private static void SetHighlightedFortuneOption()
        {
            for (var i = 0; i < FortuneOptions.Count; i++)
            {
                FortuneOptions[i].IsHighlighted = i == CurrentFortuneOption;
            }
        }
    }
}
