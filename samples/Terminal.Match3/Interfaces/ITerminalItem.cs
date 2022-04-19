using System;

namespace Terminal.Match3.Interfaces
{
    public interface ITerminalItem
    {
        int ContentId { get; }
        public char Icon { get; set; }
        public ConsoleColor Color { get; set; }
    }
}