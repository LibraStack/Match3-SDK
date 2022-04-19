using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.App.Interfaces
{
    public interface ISequenceDetector<TGridSlot> where TGridSlot : IGridSlot
    {
        ItemSequence<TGridSlot> GetSequence(IGameBoard<TGridSlot> gameBoard, GridPosition gridPosition);
    }
}