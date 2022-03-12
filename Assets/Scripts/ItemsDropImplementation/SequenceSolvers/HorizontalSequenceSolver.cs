using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using ItemsDropImplementation.Jobs;

namespace ItemsDropImplementation.SequenceSolvers
{
    public class HorizontalSequenceSolver : DropSequenceSolver
    {
        public HorizontalSequenceSolver(IGrid gameBoard) : base(gameBoard)
        {
        }

        public override IEnumerable<IJob> SolveSequences(List<ItemSequence> itemSequences)
        {
            var jobs = new List<IJob>();

            var maxRowIndex = 0;
            var maxColumnIndex = 0;
            var minColumnIndex = GameBoard.ColumnCount;

            foreach (var itemSequence in itemSequences)
            {
                foreach (var solvedGridSlot in itemSequence.SolvedGridSlots)
                {
                    var gridPosition = solvedGridSlot.GridPosition;

                    if (gridPosition.RowIndex > maxRowIndex)
                    {
                        maxRowIndex = gridPosition.RowIndex;
                    }

                    if (gridPosition.ColumnIndex > maxColumnIndex)
                    {
                        maxColumnIndex = gridPosition.ColumnIndex;
                    }

                    if (gridPosition.ColumnIndex < minColumnIndex)
                    {
                        minColumnIndex = gridPosition.ColumnIndex;
                    }
                }
            }

            for (var columnIndex = minColumnIndex; columnIndex <= maxColumnIndex; columnIndex++)
            {
                var gridSlot = GameBoard[maxRowIndex, columnIndex];
                if (gridSlot.State != GridSlotState.Free)
                {
                    continue;
                }

                jobs.Add(new ItemsMoveJob(GetItemsMoveData(gridSlot)));
            }

            return jobs;
        }
    }
}