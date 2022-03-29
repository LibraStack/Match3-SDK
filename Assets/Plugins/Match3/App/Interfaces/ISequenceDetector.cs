using Match3.App.Models;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.App.Interfaces
{
    public interface ISequenceDetector<TItem> where TItem : IItem
    {
        ItemSequence<TItem> GetSequence(IGameBoard<TItem> gameBoard, GridPosition gridPosition);
    }
}