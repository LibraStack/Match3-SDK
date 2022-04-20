using System.Collections.Generic;
using Match3.App;
using Match3.Core.Interfaces;

namespace Terminal.Match3.Extensions
{
    public static class ItemsSequenceExtensions
    {
        public static IEnumerable<TGridSlot> GetUniqueSolvedGridSlots<TGridSlot>(this SolvedData<TGridSlot> solvedData,
            bool onlyMovable = false) where TGridSlot : IGridSlot
        {
            var solvedGridSlots = new HashSet<TGridSlot>();

            foreach (var solvedGridSlot in solvedData.GetSolvedGridSlots())
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

            solvedGridSlots.Clear();
        }
    }
}