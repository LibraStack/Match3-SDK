using System.Collections.Generic;
using Match3.App;
using Match3.Core.Interfaces;

namespace Terminal.Match3.Extensions
{
    public static class ItemsSequenceExtensions
    {
        public static IEnumerable<TGridSlot> GetUniqueGridSlots<TGridSlot>(
            this IEnumerable<ItemSequence<TGridSlot>> sequences, bool onlyMovable = false) where TGridSlot : IGridSlot
        {
            var solvedGridSlots = new HashSet<TGridSlot>();

            foreach (var sequence in sequences)
            {
                foreach (var solvedGridSlot in sequence.SolvedGridSlots)
                {
                    if (onlyMovable && solvedGridSlot.IsMovable == false)
                    {
                        continue;
                    }

                    if (solvedGridSlots.Add(solvedGridSlot) == false)
                    {
                        continue;
                    }

                    yield return solvedGridSlot;
                }
            }

            solvedGridSlots.Clear();
        }
    }
}