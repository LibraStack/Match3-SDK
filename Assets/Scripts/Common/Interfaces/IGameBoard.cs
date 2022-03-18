using Common.Models;
using Common.Structs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IGameBoard : IGrid
    {
        bool IsFilled { get; }

        GridSlot this[Vector3 worldPosition] { get; }
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