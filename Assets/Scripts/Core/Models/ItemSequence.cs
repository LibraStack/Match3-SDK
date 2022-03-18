using System.Collections.Generic;
using Match3.Core.Enums;

namespace Match3.Core.Models
{
    public class ItemSequence
    {
        public ItemSequenceType Type { get; }
        public IReadOnlyList<GridSlot> SolvedGridSlots { get; }

        public ItemSequence(ItemSequenceType type, IReadOnlyList<GridSlot> solvedGridSlots)
        {
            Type = type;
            SolvedGridSlots = solvedGridSlots;
        }
    }
}