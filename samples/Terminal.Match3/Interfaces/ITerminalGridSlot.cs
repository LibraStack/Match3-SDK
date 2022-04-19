using Match3.Core.Interfaces;

namespace Terminal.Match3.Interfaces
{
    public interface ITerminalGridSlot : IGridSlot
    {
        bool IsLocked { get; }
        bool CanSetItem { get; }

        ITerminalItem Item { get; }

        void SetItem(ITerminalItem item);
    }
}