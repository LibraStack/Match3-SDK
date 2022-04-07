using System.Collections.Generic;
using Match3.App.Models;
using Match3.Core.Interfaces;
using Match3.Core.Models;

namespace Common.Extensions
{
    public static class ItemsSequenceExtensions
    {
        public static IEnumerable<GridSlot<TItem>> GetUniqueGridSlots<TItem>(
            this IEnumerable<ItemSequence<TItem>> sequences, bool includeLocked = true) where TItem : IItem
        {
            var solvedGridSlots = new HashSet<GridSlot<TItem>>();

            foreach (var sequence in sequences)
            {
                foreach (var solvedGridSlot in sequence.SolvedGridSlots)
                {
                    if (includeLocked == false && solvedGridSlot.IsLocked)
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