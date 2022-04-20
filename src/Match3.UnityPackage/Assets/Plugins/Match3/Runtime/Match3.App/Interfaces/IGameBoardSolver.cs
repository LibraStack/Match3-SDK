using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.App.Interfaces
{
    public interface IGameBoardSolver<TGridSlot> where TGridSlot : IGridSlot
    {
        SolvedData<TGridSlot> Solve(IGameBoard<TGridSlot> gameBoard, params GridPosition[] gridPositions);
    }
}