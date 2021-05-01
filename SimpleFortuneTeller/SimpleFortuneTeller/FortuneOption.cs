using System;
using System.Collections.Generic;

namespace SimpleFortuneTeller
{
    public class FortuneOption
    {
        public string OptionDisplay { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public bool IsHighlighted { get; set; }
        public List<FortuneOption> NextOptions { get; set; } = new List<FortuneOption>();
    }
}
