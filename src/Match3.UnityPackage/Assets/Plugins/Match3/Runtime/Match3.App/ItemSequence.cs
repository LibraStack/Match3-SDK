using System;
using System.Collections.Generic;

namespace Match3.App
{
    public class ItemSequence<TGridSlot>
    {
        public ItemSequence(Type sequenceDetectorType, IReadOnlyList<TGridSlot> solvedGridSlots)
        {
            SequenceDetectorType = sequenceDetectorType;
            SolvedGridSlots = solvedGridSlots;
        }

        public Type SequenceDetectorType { get; }
        public IReadOnlyList<TGridSlot> SolvedGridSlots { get; }
    }
}