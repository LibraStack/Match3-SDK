using System.Collections.Generic;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Match3.Core.Interfaces
{
    public interface IGameBoardSolver
    {
        IReadOnlyCollection<ItemSequence> Solve(IGameBoard gameBoard, params GridPosition[] positions);
    }
}