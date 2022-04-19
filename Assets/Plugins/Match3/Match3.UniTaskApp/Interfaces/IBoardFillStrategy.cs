using System.Collections.Generic;
using Match3.App.Interfaces;
using Match3.App.Models;
using Match3.Core.Interfaces;

namespace Match3.UniTaskApp.Interfaces
{
    public interface IBoardFillStrategy<TGridSlot> where TGridSlot : IGridSlot
    {
        string Name { get; }

        IEnumerable<IJob> GetFillJobs(IGameBoard<TGridSlot> gameBoard);
        IEnumerable<IJob> GetSolveJobs(IGameBoard<TGridSlot> gameBoard, IEnumerable<ItemSequence<TGridSlot>> sequences);
    }
}