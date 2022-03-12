using System.Collections.Generic;
using Enums;
using Interfaces;
using Jobs;
using Models;

namespace FillStrategies
{
    public class VerticalSequenceSolver : DropSequenceSolver
    {
        public VerticalSequenceSolver(IGrid gameBoard) : base(gameBoard)
        {
        }

        public override IEnumerable<IJob> SolveSequences(List<ItemSequence> itemSequences)
        {
            var jobs = new List<IJob>();

            foreach (var itemSequence in itemSequences)
            {
                var lowerFreeSlot = itemSequence.SolvedGridSlots[0];

                foreach (var solvedGridSlot in itemSequence.SolvedGridSlots)
                {
                    if (solvedGridSlot.State != GridSlotState.Free)
                    {
                        continue;
                    }

                    if (solvedGridSlot.GridPosition.RowIndex > lowerFreeSlot.GridPosition.RowIndex)
                    {
                        lowerFreeSlot = solvedGridSlot;
                    }
                }

                jobs.Add(new ItemsMoveJob(GetItemsMoveData(lowerFreeSlot)));
            }

            return jobs;
        }
    }
}