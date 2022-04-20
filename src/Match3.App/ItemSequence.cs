using System;
using System.Collections.Generic;
using Match3.Core.Interfaces;

namespace Match3.App
{
    public class ItemSequence<TGridSlot> where TGridSlot : IGridSlot
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