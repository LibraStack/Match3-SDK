using System.Collections.Generic;
using Match3.App.Models;
using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface ISolvedSequencesConsumer<TItem> where TItem : IItem
    {
        void RegisterSolvedSequences(IEnumerable<ItemSequence<TItem>> sequences);
    }
}