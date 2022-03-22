using Cysharp.Threading.Tasks;
using Match3.Core.Models;
using Match3.Core.Structs;
using UnityEngine;

namespace Match3.Core.Interfaces
{
    public interface IGameBoard : IGrid
    {
        bool IsFilled { get; }

        GridSlot this[GridPosition gridPosition] { get; }
        GridSlot this[int rowIndex, int columnIndex] { get; }
        
        void CreateGridSlots();
        bool IsSlotActive(GridPosition slotPosition);
        void ActivateSlot(GridPosition slotPosition);
        void DeactivateSlot(GridPosition slotPosition);

        UniTask FillAsync(IBoardFillStrategy fillStrategy);
        UniTask SwapItemsAsync(IBoardFillStrategy fillStrategy, GridPosition position1, GridPosition position2);
    }
}