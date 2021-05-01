using System;
using System.Collections.Generic;

namespace SimpleFortuneTeller.Rendering
{
    public class TextRenderBuffer
    {
        private const int MaxWidth = 800;
        private const int MaxHeight = 800;
        private readonly string[] TextBuffer = new string[MaxWidth * MaxHeight];
        private int Width = 0;
        private int Height = 0;
        private ConsoleColor CurrentForegroundColor;
        private ConsoleColor CurrentBackgroundColor;
        private SortedDictionary<ConsoleColor, SortedDictionary<ConsoleColor, List<int>>> DrawDictionary;
        /*
         * Note:
         * Thinking about trying something like a byte that is
         * (backgroundConsoleColor << 4) | foregroundConsoleColor
         * That key can be easily decoded (I think) and can be used
         * for a reverse lookup to make sure we only have each index listed
         * with one color pairing.
         * It simplifies the DrawDictionary substantially as well.
         * Could implement my own icomparable for the datatype if byte doesn't work.
         * */

        public int GetWidth() => Width;
        public int GetHeight() => Height;

        public void ClearBuffer(string to = "")
        {
            DrawDictionary = new SortedDictionary<ConsoleColor, SortedDictionary<ConsoleColor, List<int>>>();
            if (to.Length > 1)
            {
                to = to.Substring(0, 1);
            }
            for (var i = 0; i < TextBuffer.Length; i++)
            {
                TextBuffer[i] = to;
            }
        }

        public void SetSize()
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var oldWidth = Width;
            var oldHeight = Height;
            Width = Math.Min(width, MaxWidth);
            Height = Math.Min(height, MaxHeight);
            if (Width != oldWidth || Height != oldHeight)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                for (var y = 0; y <= height + 1; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        ProtectedXYWrite(x, y, " ");
                    }
                }
                Console.Clear();
            }
        }

        public void SetColors(ConsoleColor foreground, ConsoleColor background)
        {
            CurrentForegroundColor = foreground;
            CurrentBackgroundColor = background;
        }

        public void WriteToBuffer(string text, int x, int y)
        {
            var start = (y * Width) + x;
            for (var i = 0; i < text.Length; i++)
            {
                TextBuffer[start + i] = text.Substring(i, 1);
                LogColorPosition(start + i);
            }
        }

        private void LogColorPosition(int index)
        {
            if (DrawDictionary.ContainsKey(CurrentBackgroundColor))
            {
                var foregroundCollection = DrawDictionary[CurrentBackgroundColor];
                if (foregroundCollection.ContainsKey(CurrentForegroundColor))
                {
                    foregroundCollection[CurrentForegroundColor].Add(index);
                }
                else
                {
                    foregroundCollection.Add(CurrentForegroundColor, new List<int>
                        {
                            index,
                        });
                }
            }
            else
            {
                DrawDictionary.Add(CurrentBackgroundColor, new SortedDictionary<ConsoleColor, List<int>>
                {
                    { CurrentForegroundColor, new List<int>
                        {
                            index,
                        }
                    }
                });
            }
        }

        public void DrawBuffer()
        {
            Console.CursorVisible = false;
            foreach (var backgroundKvp in DrawDictionary)
            {
                Console.BackgroundColor = backgroundKvp.Key;
                foreach (var foregroundKvp in backgroundKvp.Value)
                {
                    Console.ForegroundColor = foregroundKvp.Key;
                    foreach (var index in foregroundKvp.Value)
                    {
                        if (Console.WindowWidth >= Width && Console.WindowHeight >= Height)
                        {
                            Console.CursorLeft = index % Width;
                            Console.CursorTop = index / Width;
                            Console.Write(TextBuffer[index]);
                        }
                    }
                }
            }
        }

        private void ProtectedXYWrite(int x, int y, string text)
        {
            if (Console.WindowWidth >= Width && Console.WindowHeight >= Height)
            {
                Console.CursorLeft = x;
                Console.CursorTop = y;
                Console.Write(text);
            }
        }
    }
}
