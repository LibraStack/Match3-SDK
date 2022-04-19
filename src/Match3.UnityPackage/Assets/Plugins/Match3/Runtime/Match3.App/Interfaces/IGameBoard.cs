using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.App.Interfaces
{
    public interface IGameBoard<out TGridSlot> : IGrid where TGridSlot : IGridSlot
    {
        TGridSlot this[GridPosition gridPosition] { get; }
        TGridSlot this[int rowIndex, int columnIndex] { get; }

        bool IsPositionOnBoard(GridPosition gridPosition);
    }
}