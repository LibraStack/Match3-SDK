using System.Collections.Generic;
using Match3.Core.Enums;
using Match3.Core.Interfaces;

namespace Match3.Core.Models
{
    public class ItemSequence<TItem> where TItem : IItem
    {
        public ItemSequenceType Type { get; }
        public IReadOnlyList<GridSlot<TItem>> SolvedGridSlots { get; }

        public ItemSequence(ItemSequenceType type, IReadOnlyList<GridSlot<TItem>> solvedGridSlots)
        {
            Type = type;
            SolvedGridSlots = solvedGridSlots;
        }
    }
}