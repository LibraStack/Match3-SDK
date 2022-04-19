using System.Collections.Generic;
using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface ISolvedSequencesConsumer<TGridSlot> where TGridSlot : IGridSlot
    {
        void OnSequencesSolved(IEnumerable<ItemSequence<TGridSlot>> sequences);
    }
}