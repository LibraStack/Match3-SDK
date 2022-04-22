using System.Collections.Generic;
using Match3.Core.Interfaces;

namespace Match3.App
{
    public class SolvedData<TGridSlot> where TGridSlot : IGridSlot
    {
        public SolvedData(IReadOnlyCollection<ItemSequence<TGridSlot>> solvedSequences,
            IReadOnlyCollection<TGridSlot> specialItemGridSlots)
        {
            SolvedSequences = solvedSequences;
            SpecialItemGridSlots = specialItemGridSlots;
        }

        public IReadOnlyCollection<TGridSlot> SpecialItemGridSlots { get; }
        public IReadOnlyCollection<ItemSequence<TGridSlot>> SolvedSequences { get; }

        public IEnumerable<TGridSlot> GetSolvedGridSlots(bool onlyMovable = false)
        {
            foreach (var sequence in SolvedSequences)
            {
                foreach (var solvedGridSlot in sequence.SolvedGridSlots)
                {
                    if (onlyMovable && solvedGridSlot.IsMovable == false)
                    {
                        continue;
                    }

                    yield return solvedGridSlot;
                }
            }
        }

        public IEnumerable<TGridSlot> GetSpecialItemGridSlots(bool excludeOccupied = false)
        {
            foreach (var specialItemGridSlot in SpecialItemGridSlots)
            {
                if (excludeOccupied && specialItemGridSlot.HasItem)
                {
                    continue;
                }

                yield return specialItemGridSlot;
            }
        }
    }
}