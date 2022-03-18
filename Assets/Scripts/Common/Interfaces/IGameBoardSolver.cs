using System.Collections.Generic;
using Common.Models;
using Common.Structs;

namespace Common.Interfaces
{
    public interface IGameBoardSolver
    {
        IReadOnlyCollection<ItemSequence> Solve(IGameBoard gameBoard, params GridPosition[] positions);
    }
}