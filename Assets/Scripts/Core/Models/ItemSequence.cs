using System;
using System.Collections.Generic;
using Match3.Core.Interfaces;

namespace Match3.Core.Models
{
    public class ItemSequence<TItem> where TItem : IItem
    {
        public Type SequenceDetectorType { get; }
        public IReadOnlyList<GridSlot<TItem>> SolvedGridSlots { get; }

        public ItemSequence(Type sequenceDetectorType, IReadOnlyList<GridSlot<TItem>> solvedGridSlots)
        {
            SequenceDetectorType = sequenceDetectorType;
            SolvedGridSlots = solvedGridSlots;
        }
    }
}