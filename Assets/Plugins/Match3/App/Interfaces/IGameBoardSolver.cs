using System.Collections.Generic;
using Match3.App.Models;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.App.Interfaces
{
    public interface IGameBoardSolver<TItem> where TItem : IItem
    {
        IReadOnlyCollection<ItemSequence<TItem>> Solve(IGameBoard<TItem> gameBoard, params GridPosition[] gridPositions);
    }
}