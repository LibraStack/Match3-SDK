using System.Collections.Generic;
using Common.Interfaces;
using Common.Models;

namespace ItemsDropImplementation.Interfaces
{
    public interface ISequenceSolver
    {
        IEnumerable<IJob> SolveSequences(List<ItemSequence> itemSequences);
    }
}