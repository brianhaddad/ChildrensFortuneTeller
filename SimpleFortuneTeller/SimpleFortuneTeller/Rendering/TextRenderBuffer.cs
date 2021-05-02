using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleFortuneTeller.Rendering
{
    public class TextRenderBuffer
    {
        private const int MaxWidth = 800;
        private const int MaxHeight = 800;
        private readonly string[] TextBuffer = new string[MaxWidth * MaxHeight];
        private int Width = 0;
        private int Height = 0;
        private int CombinedColorCode;
        private SortedDictionary<int, SortedList<int, int>> DrawDictionary;
        private SortedDictionary<int, int> DrawDictionaryReverseLookup;

        public int GetWidth() => Width;
        public int GetHeight() => Height;

        public void ClearBuffer(string to = "")
        {
            DrawDictionary = new SortedDictionary<int, SortedList<int, int>>();
            DrawDictionaryReverseLookup = new SortedDictionary<int, int>();
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
                ForceClear();
            }
        }

        public void ForceClear()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            for (var y = 0; y <= Console.WindowHeight + 1; y++)
            {
                for (var x = 0; x < Console.WindowWidth; x++)
                {
                    ProtectedXYWrite(x, y, " ");
                }
            }
            Console.Clear();
        }

        public void SetColors(ConsoleColor foreground, ConsoleColor background)
            => CombinedColorCode = EncodeCombinedColor(foreground, background);

        public void WriteToBuffer(string text, int x, int y)
        {
            var start = (y * Width) + x;
            for (var i = 0; i < text.Length; i++)
            {
                TextBuffer[start + i] = text.Substring(i, 1);
                LogColorPosition(start + i);
            }
        }

        private int EncodeCombinedColor(ConsoleColor foreground, ConsoleColor background)
            => ((int)background << 4) | (int)foreground;

        private ConsoleColor DecodeForegroundColor(int combinedColorCode)
            => (ConsoleColor)(combinedColorCode & 0b00001111);

        private ConsoleColor DecodeBackgroundColor(int combinedColorCode)
            => (ConsoleColor)(combinedColorCode >> 4);

        private void LogColorPosition(int index)
        {
            if (DrawDictionaryReverseLookup.ContainsKey(index))
            {
                DrawDictionary[DrawDictionaryReverseLookup[index]].Remove(index);
                DrawDictionaryReverseLookup[index] = CombinedColorCode;
            }
            else
            {
                DrawDictionaryReverseLookup.Add(index, CombinedColorCode);
            }
            if (DrawDictionary.ContainsKey(CombinedColorCode))
            {
                DrawDictionary[CombinedColorCode].Add(index, index);
            }
            else
            {
                DrawDictionary.Add(CombinedColorCode, new SortedList<int, int> { { index, index } });
            }
        }

        //Stream writer flush method:
        //https://blog.anarks2.com/Buffered-dotnet-console-1/
        //Doesn't support setting the position or changing colors though. :(
        //Also Console.OpenStandardInput() doesn't get a stream that can be written to here for some reason.
        public void DrawBuffer()
        {
            Console.CursorVisible = false;
            Console.SetWindowPosition(0, 0);
            foreach (var dataKVP in DrawDictionary)
            {
                Console.BackgroundColor = DecodeBackgroundColor(dataKVP.Key);
                Console.ForegroundColor = DecodeForegroundColor(dataKVP.Key);
                var arr = dataKVP.Value.Keys.ToArray();
                for (var i = 0; i < arr.Length; i++)
                {
                    if (Console.WindowWidth >= Width && Console.WindowHeight >= Height)
                    {
                        Console.CursorLeft = arr[i] % Width;
                        Console.CursorTop = arr[i] / Width;
                        Console.Write(TextBuffer[arr[i]]);
                    }
                }
            }
        }

        private void ProtectedXYWrite(int x, int y, string text)
        {
            if (Console.WindowWidth >= Width
                && Console.WindowHeight >= Height
                && x < Console.WindowWidth)
            {
                Console.CursorLeft = x;
                Console.CursorTop = y;
                Console.Write(text);
            }
        }
    }
}
