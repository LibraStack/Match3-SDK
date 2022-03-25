using System.Collections.Generic;
using Match3.Core.Interfaces;
using Match3.Core.Models;

namespace Implementation.Common.Interfaces
{
    public interface ISolvedSequencesConsumer<TItem> where TItem : IItem
    {
        void RegisterSolvedSequences(IEnumerable<ItemSequence<TItem>> sequences);
    }
}