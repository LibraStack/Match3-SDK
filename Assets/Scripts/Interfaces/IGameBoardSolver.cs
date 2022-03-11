using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IGameBoardSolver
    {
        IReadOnlyCollection<ItemSequence> Solve(IGrid gameBoard, params GridPosition[] positions);
    }
}