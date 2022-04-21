using Match3.Core.Structs;

namespace Match3.Core.Interfaces
{
    public interface IGridSlot
    {
        int ItemId { get; }

        bool HasItem { get; }
        bool IsMovable { get; }
        bool CanContainItem { get; }

        IGridSlotState State { get; }
        GridPosition GridPosition { get; }
    }
}