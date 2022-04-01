using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Match3.App.Interfaces
{
    public interface IGameBoard<TItem> : IGrid where TItem : IItem
    {
        GridSlot<TItem> this[GridPosition gridPosition] { get; }
        GridSlot<TItem> this[int rowIndex, int columnIndex] { get; }
    }
}