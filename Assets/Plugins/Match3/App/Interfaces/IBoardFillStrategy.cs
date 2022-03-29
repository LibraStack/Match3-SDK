using System.Collections.Generic;
using Match3.App.Models;
using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface IBoardFillStrategy<TItem> where TItem : IItem
    {
        string Name { get; }

        IEnumerable<IJob> GetFillJobs(IGameBoard<TItem> gameBoard);
        IEnumerable<IJob> GetSolveJobs(IGameBoard<TItem> gameBoard, IEnumerable<ItemSequence<TItem>> sequences);
    }
}