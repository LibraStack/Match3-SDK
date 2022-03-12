using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IGameBoard : IGrid
    {
        bool IsFilled { get; }

        void CreateGridSlots();
        bool IsSlotActive(GridPosition slotPosition);
        void ActivateSlot(GridPosition slotPosition);
        void DeactivateSlot(GridPosition slotPosition);

        UniTask FillAsync(IBoardFillStrategy fillStrategy);
        UniTask SwapItemsAsync(IBoardFillStrategy fillStrategy, GridPosition position1, GridPosition position2);
    }
}