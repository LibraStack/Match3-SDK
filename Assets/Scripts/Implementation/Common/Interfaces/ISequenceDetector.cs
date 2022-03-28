using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Implementation.Common.Interfaces
{
    public interface ISequenceDetector<TItem> where TItem : IItem
    {
        ItemSequence<TItem> GetSequence(IGameBoard<TItem> gameBoard, GridPosition gridPosition);
    }
}