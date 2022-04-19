using System;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3
{
    public class TerminalItem : ITerminalItem
    {
        public TerminalItem()
        {
            Icon = '-';
            Color = ConsoleColor.Magenta;
        }

        public TerminalItem(char icon, ConsoleColor color)
        {
            Icon = icon;
            Color = color;
        }

        public int ContentId => Icon.GetHashCode();
        public char Icon { get; set; }
        public ConsoleColor Color { get; set; }
    }
}