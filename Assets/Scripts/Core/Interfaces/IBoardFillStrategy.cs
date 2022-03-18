using System.Collections.Generic;
using Match3.Core.Models;

namespace Match3.Core.Interfaces
{
    public interface IBoardFillStrategy
    {
        string Name { get; }

        IEnumerable<IJob> GetFillJobs();
        IEnumerable<IJob> GetSolveJobs(IReadOnlyCollection<ItemSequence> sequences);
    }
}