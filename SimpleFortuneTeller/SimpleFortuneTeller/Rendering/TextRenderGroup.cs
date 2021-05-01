using System;
using System.Collections.Generic;

namespace SimpleFortuneTeller.Rendering
{
    public class TextRenderGroup
    {
        public List<TextRenderElement> TextRenderElements { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
    }
}
