using System.Collections.Generic;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.App.Interfaces
{
    public interface IGameBoardSolver<TGridSlot> where TGridSlot : IGridSlot
    {
        IReadOnlyCollection<ItemSequence<TGridSlot>> Solve(IGameBoard<TGridSlot> gameBoard,
            params GridPosition[] gridPositions);
    }
}