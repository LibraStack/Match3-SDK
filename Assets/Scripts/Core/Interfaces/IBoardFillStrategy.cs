using System.Collections.Generic;
using Match3.Core.Models;

namespace Match3.Core.Interfaces
{
    public interface IBoardFillStrategy<TItem> where TItem : IItem
    {
        string Name { get; }

        IEnumerable<IJob> GetFillJobs();
        IEnumerable<IJob> GetSolveJobs(IEnumerable<ItemSequence<TItem>> sequences);
    }
}