using Cysharp.Threading.Tasks;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Match3.Core.Interfaces
{
    public interface IGameBoard<TItem> : IGrid where TItem : IItem
    {
        bool IsFilled { get; }

        GridSlot<TItem> this[GridPosition gridPosition] { get; }
        GridSlot<TItem> this[int rowIndex, int columnIndex] { get; }
        
        void CreateGridSlots();
        bool IsSlotActive(GridPosition slotPosition);
        void ActivateSlot(GridPosition slotPosition);
        void DeactivateSlot(GridPosition slotPosition);

        UniTask FillAsync(IBoardFillStrategy<TItem> fillStrategy);
        UniTask SwapItemsAsync(IBoardFillStrategy<TItem> fillStrategy, GridPosition position1, GridPosition position2);
    }
}