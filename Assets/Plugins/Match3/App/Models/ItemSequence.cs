using System;
using System.Collections.Generic;
using Match3.Core.Interfaces;
using Match3.Core.Models;

namespace Match3.App.Models
{
    public class ItemSequence<TItem> where TItem : IItem
    {
        public ItemSequence(Type sequenceDetectorType, IReadOnlyList<GridSlot<TItem>> solvedGridSlots)
        {
            SequenceDetectorType = sequenceDetectorType;
            SolvedGridSlots = solvedGridSlots;
        }

        public Type SequenceDetectorType { get; }

        public IReadOnlyList<GridSlot<TItem>> SolvedGridSlots { get; }
    }
}