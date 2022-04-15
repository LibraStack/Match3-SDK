using System.Collections.Generic;
using Match3.Core.Interfaces;
using Match3.Core.Models;

namespace Match3.UnityApp.Interfaces
{
    public interface IBoardFillStrategy<TGridSlot> where TGridSlot : IGridSlot
    {
        string Name { get; }

        IEnumerable<IJob> GetFillJobs(IGameBoard<TGridSlot> gameBoard);
        IEnumerable<IJob> GetSolveJobs(IGameBoard<TGridSlot> gameBoard, IEnumerable<ItemSequence<TGridSlot>> sequences);
    }
}