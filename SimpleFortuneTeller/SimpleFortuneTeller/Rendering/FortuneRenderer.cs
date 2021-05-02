using SimpleFortuneTeller.Handling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SimpleFortuneTeller.Rendering
{
    public static class FortuneRenderer
    {
        private static List<TextRenderGroup> TextRenderGroups = new List<TextRenderGroup>();
        private static readonly TextRenderBuffer RenderBuffer = new TextRenderBuffer();
        private static int LayoutRows = 0;
        private static int LayoutCols = 0;

        private static string LastFortuneOptionsSiguature = "";
        private static string GrandFinale = "";
        private static List<string> SelectionResults;
        private static bool ClearedScreenForResults = false;

        private const char WinUpperLeft = '\u2554';
        private const char WinHorizontal = '\u2550';
        private const char WinUpperRight = '\u2557';
        private const char WinVertical = '\u2551';
        private const char WinLowerLeft = '\u255A';
        private const char WinLowerRight = '\u255D';

        public static (int, int) GetLayoutColsRows() => (LayoutCols, LayoutRows);

        public static void SetSelectionResults()
            => SelectionResults = FortuneLogic.GetSelections();

        public static void Render(int tilePadding = 1, int tileMargin = 1)
        {
            // First Steps
            RenderBuffer.ClearBuffer(" ");
            RenderBuffer.SetSize();
            TextRenderGroups = new List<TextRenderGroup>();

            if (SelectionResults is null)
            {
                RenderOptions(tilePadding, tileMargin);
            }
            else
            {
                RenderFinale(tilePadding, tileMargin);
            }

            WriteAllGroupsToBuffer();
            RenderBuffer.DrawBuffer();
        }

        public static void RenderFinale(int tilePadding = 1, int tileMargin = 1)
        {
            if (!ClearedScreenForResults)
            {
                ClearedScreenForResults = true;
                RenderBuffer.ForceClear();
            }
            if (string.IsNullOrEmpty(GrandFinale))
            {
                GrandFinale = FinaleBuilder.Build();
            }
            var lines = WordWrapLine(GrandFinale, (Console.WindowWidth / 3) * 2);
            AddTile(
                string.Join(Environment.NewLine, lines),
                new Rectangle(tileMargin, tileMargin, Console.WindowWidth - (2 * tileMargin), Console.WindowHeight - (2 * tileMargin)),
                ConsoleColor.Magenta,
                ConsoleColor.Black);
        }

        public static void RenderOptions(int tilePadding = 1, int tileMargin = 1)
        {
            var fortuneOptions = FortuneLogic.GetFortuneOptions();
            var newFortuneOptionsSignature = FortuneOptionsSignature(fortuneOptions);
            if (newFortuneOptionsSignature != LastFortuneOptionsSiguature)
            {
                LastFortuneOptionsSiguature = newFortuneOptionsSignature;
                RenderBuffer.ForceClear();
            }

            //TODO: refactor to allow draw area to be passed in.
            var windowWidth = RenderBuffer.GetWidth();
            var windowHeight = RenderBuffer.GetHeight();
            var maxTileWidth = fortuneOptions.Max(x => x.OptionDisplay.Length) + (2 * tilePadding);
            var maxTilesPerRow = Math.Min((windowWidth - tileMargin) / (maxTileWidth + tileMargin), fortuneOptions.Count());
            var multiLineConsideration = 0;
            if (maxTilesPerRow < 2)
            {
                var maxLines = 0;
                maxTileWidth = ((windowWidth - (3 * tileMargin)) / 2) - (2 * tilePadding);
                maxTilesPerRow = (windowWidth - tileMargin) / (maxTileWidth + (2 * tilePadding) + tileMargin);
                foreach (var option in fortuneOptions)
                {
                    var lines = WordWrapLine(option.OptionDisplay, maxTileWidth);
                    maxLines = Math.Max(maxLines, lines.Length);
                    option.OptionDisplay = string.Join(Environment.NewLine, lines);
                }
                multiLineConsideration = maxLines;
            }
            var actualTileWidth = (windowWidth - ((maxTilesPerRow + 1) * tileMargin)) / maxTilesPerRow;
            var actualTilesPerRow = (windowWidth - (2 * tileMargin)) / actualTileWidth;
            var numRows = (int)Math.Ceiling((decimal)fortuneOptions.Count() / actualTilesPerRow);
            var minTileHeight = 3 + (tileMargin * 2) + multiLineConsideration;
            
            if (numRows * minTileHeight > windowHeight)
            {
                // Can't fit everything on screen.
                AddTile("Drawing Error", new Rectangle(0, 0, windowWidth, windowHeight), ConsoleColor.White, ConsoleColor.Red);
                LayoutRows = 0;
                LayoutCols = 0;
            }
            else
            {
                LayoutRows = numRows;
                LayoutCols = actualTilesPerRow;
                var actualTileHeight = (windowHeight - ((numRows + 1) * tileMargin)) / numRows;
                for (var y = 0; y < numRows; y++)
                {
                    for (var x = 0; x < actualTilesPerRow; x++)
                    {
                        var index = (y * actualTilesPerRow) + x;
                        if (index < fortuneOptions.Count())
                        {
                            var tileRect = new Rectangle(
                                tileMargin + (x * (actualTileWidth + tileMargin)),
                                tileMargin + (y * (actualTileHeight + tileMargin)),
                                actualTileWidth,
                                actualTileHeight);
                            var fortuneOption = fortuneOptions.ElementAt(index);
                            var foregroundColor = fortuneOption.ForegroundColor;
                            var backgroundColor = fortuneOption.BackgroundColor;
                            var includeUnderline = false;
                            if (fortuneOption.IsHighlighted && DateTime.Now.Millisecond / 500 % 2 == 0)
                            {
                                foregroundColor = fortuneOption.BackgroundColor;
                                backgroundColor = fortuneOption.ForegroundColor;
                            }
                            if (fortuneOption.IsHighlighted && actualTileHeight > minTileHeight + 1)
                            {
                                includeUnderline = true;
                            }
                            AddTile(fortuneOption.OptionDisplay, tileRect, foregroundColor, backgroundColor, includeUnderline);
                        }
                    }
                }
            }
        }

        private static string[] WordWrapLine(string line, int maxLineWidth)
        {
            var newLines = new List<string>();
            var words = line.Split(" ");
            var i = 0;
            var lineCount = 1;
            while (i < words.Length)
            {
                var nextLine = words[i];
                while (i < words.Length - 1 && nextLine.Length + words[i + 1].Length + 1 < maxLineWidth)
                {
                    i++;
                    nextLine += " " + words[i];
                }
                newLines.Add(nextLine);
                lineCount++;
                i++;
            }
            return newLines.ToArray();
        }

        private static string FortuneOptionsSignature(IEnumerable<FortuneOption> fortuneOptions)
            => string.Join(",", fortuneOptions.Select(x => x.OptionDisplay));

        private static void AddTile(string text, Rectangle bounds, ConsoleColor foreground, ConsoleColor background, bool includeUnderline = false)
        {
            var firstLine = 0;
            var lastLine = bounds.Height - 1;
            for (var y = 0; y < bounds.Height; y++)
            {
                string line;
                if (y == firstLine)
                {
                    line = BoxTopRow(bounds.Width);
                }
                else if (y == lastLine)
                {
                    line = BoxBottomRow(bounds.Width);
                }
                else
                {
                    line = BoxMiddleRow(bounds.Width);
                }
                var tre = new TextRenderElement
                {
                    Text = line,
                    X = bounds.X,
                    Y = bounds.Y + y,
                };
                AddToGroup(tre, foreground, background);
            }
            //TODO:
            //For some reason adding the text last does result in it getting drawn last.
            //Once it's all flattened in the buffer, the idea was for same color stuff
            //to be drawn sequentially. This requires tedious troubleshooting. Good luck!
            //TODO: expand to allow for non-centered text too. Formatting choices!
            AddCenteredText(text, bounds, foreground, background, includeUnderline);
        }

        private static string BoxTopRow(int width)
            => BoxRow(WinUpperLeft, WinHorizontal, WinUpperRight, width);

        private static string BoxMiddleRow(int width)
            => BoxRow(WinVertical, ' ', WinVertical, width);

        private static string BoxBottomRow(int width)
            => BoxRow(WinLowerLeft, WinHorizontal, WinLowerRight, width);

        private static string BoxRow(char left, char fill, char right, int width)
        {
            string line = left.ToString();
            line += LineFill(fill, width - 2);
            return line + right.ToString();
        }

        private static string LineFill(char fill, int repeated)
        {
            var line = "";
            for (var x = 0; x < repeated; x++)
            {
                line += fill.ToString();
            }
            return line;
        }

        private static void AddCenteredText(string text, Rectangle centerBounds, ConsoleColor foreground, ConsoleColor background, bool includeUnderline = false)
        {
            var middleX = centerBounds.X + (centerBounds.Width / 2);
            var lines = new List<string>();
            if (text.IndexOf(Environment.NewLine) > -1)
            {
                lines.AddRange(text.Split(Environment.NewLine));
            }
            else
            {
                lines.Add(text);
            }
            if (includeUnderline)
            {
                lines.Add(LineFill('\u00AF', lines.Max(x => x.Length)));
            }
            var blockHeight = lines.Count;
            var startY = centerBounds.Y + ((centerBounds.Height - blockHeight) / 2);
            var elements = new List<TextRenderElement>();
            for (var y = 0; y < blockHeight; y++)
            {
                var textWidth = lines[y].Length;
                var startX = middleX - (textWidth / 2);
                var element = new TextRenderElement
                {
                    Text = lines[y],
                    X = startX,
                    Y = startY + y,
                };
                elements.Add(element);
            }
            AddToGroup(elements, foreground, background);
        }

        private static void AddToGroup(TextRenderElement element, ConsoleColor foreground, ConsoleColor background)
            => AddToGroup(new List<TextRenderElement> { element }, foreground, background);

        private static void AddToGroup(List<TextRenderElement> elements, ConsoleColor foreground, ConsoleColor background)
        {
            var group = TextRenderGroups.Find(x => x.ForegroundColor == foreground && x.BackgroundColor == background);
            if (group is null)
            {
                group = new TextRenderGroup
                {
                    ForegroundColor = foreground,
                    BackgroundColor = background,
                    TextRenderElements = elements,
                };
                TextRenderGroups.Add(group);
            }
            else
            {
                group.TextRenderElements.AddRange(elements);
            }
        }

        private static void WriteAllGroupsToBuffer()
        {
            foreach (var group in TextRenderGroups)
            {
                WriteRenderGroupToBuffer(group);
            }
        }

        private static void WriteRenderGroupToBuffer(TextRenderGroup textRenderGroup)
        {
            RenderBuffer.SetColors(textRenderGroup.ForegroundColor, textRenderGroup.BackgroundColor);
            foreach (var element in textRenderGroup.TextRenderElements)
            {
                RenderBuffer.WriteToBuffer(element.Text, element.X, element.Y);
            }
        }
    }
}
