using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface ISequenceSolver
    {
        IEnumerable<IJob> SolveSequences(List<ItemSequence> itemSequences);
    }
}