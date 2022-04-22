using System;
using Match3.Infrastructure;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3
{
    public class TerminalItemGenerator : ItemGenerator<ITerminalItem>
    {
        private readonly Random _random;

        public TerminalItemGenerator()
        {
            _random = new Random();
        }

        protected override ITerminalItem CreateItem()
        {
            return new TerminalItem();
        }

        protected override ITerminalItem ConfigureItem(ITerminalItem item)
        {
            var (icon, color) = GetRandomIcon();

            item.Icon = icon;
            item.Color = color;

            return item;
        }

        private (char, ConsoleColor) GetRandomIcon()
        {
            var index = _random.Next(0, 5);
            return index switch
            {
                0 => ('❖', ConsoleColor.DarkCyan),
                1 => ('★', ConsoleColor.DarkYellow),
                2 => ('✦', ConsoleColor.DarkGreen),
                3 => ('♥', ConsoleColor.DarkRed),
                4 => ('♣', ConsoleColor.DarkBlue),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}