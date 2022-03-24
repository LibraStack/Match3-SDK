using System.Collections.Generic;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Match3.Core.Interfaces
{
    public interface IGameBoardSolver<TItem> where TItem : IItem
    {
        IReadOnlyCollection<ItemSequence<TItem>> Solve(IGameBoard<TItem> gameBoard, params GridPosition[] gridPositions);
    }
}