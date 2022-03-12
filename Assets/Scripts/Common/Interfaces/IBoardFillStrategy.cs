using System.Collections.Generic;
using Common.Models;

namespace Common.Interfaces
{
    public interface IBoardFillStrategy
    {
        string Name { get; }

        IEnumerable<IJob> GetFillJobs();
        IEnumerable<IJob> GetSolveJobs(IReadOnlyCollection<ItemSequence> sequences);
    }
}