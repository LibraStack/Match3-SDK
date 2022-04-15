using System.Collections.Generic;
using Match3.Core.Interfaces;
using Match3.Core.Models;

namespace Common.Extensions
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