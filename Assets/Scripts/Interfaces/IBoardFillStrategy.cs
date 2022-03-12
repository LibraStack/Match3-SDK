using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IBoardFillStrategy
    {
        string Name { get; }

        IEnumerable<IJob> GetFillJobs();
        IEnumerable<IJob> GetSolveJobs(IReadOnlyCollection<ItemSequence> sequences);
    }
}