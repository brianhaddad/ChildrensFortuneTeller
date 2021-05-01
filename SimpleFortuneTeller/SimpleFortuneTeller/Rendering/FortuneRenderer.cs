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

        private static List<string> SelectionResults;

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

            // Last steps:
            WriteAllGroupsToBuffer();
            RenderBuffer.DrawBuffer();
        }

        public static void RenderFinale(int tilePadding = 1, int tileMargin = 1)
        {
            //TODO: make a pretty results view
            Console.CursorTop = 0;
            foreach(var result in SelectionResults)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine(result);
            }
        }

        public static void RenderOptions(int tilePadding = 1, int tileMargin = 1)
        {
            var fortuneOptions = FortuneLogic.GetFortuneOptions();

            var windowWidth = RenderBuffer.GetWidth();
            var windowHeight = RenderBuffer.GetHeight();
            var maxTileWidth = fortuneOptions.Max(x => x.OptionDisplay.Length) + (2 * tilePadding);
            var maxTilesPerRow = Math.Min((windowWidth - tileMargin) / (maxTileWidth + tileMargin), fortuneOptions.Count());
            var actualTileWidth = (windowWidth - ((maxTilesPerRow + 1) * tileMargin)) / maxTilesPerRow;
            var actualTilesPerRow = (windowWidth - (2 * tileMargin)) / actualTileWidth;
            var numRows = (int)Math.Ceiling((decimal)fortuneOptions.Count() / actualTilesPerRow);
            var minTileHeight = 3 + (tileMargin * 2);
            
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
            var middleY = centerBounds.Y + (centerBounds.Height / 2);
            var textWidth = text.Length;
            var startX = middleX - (textWidth / 2);
            var element = new TextRenderElement
            {
                Text = text,
                X = startX,
                Y = middleY,
            };
            AddToGroup(element, foreground, background);
            if (includeUnderline)
            {
                var underline = new TextRenderElement
                {
                    Text = LineFill('\u00AF', text.Length),
                    X = startX,
                    Y = middleY + 1,
                };
                AddToGroup(underline, foreground, background);
            }
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
