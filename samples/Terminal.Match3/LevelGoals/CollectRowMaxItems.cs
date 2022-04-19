using System;
using System.Collections.Generic;
using Match3.App;
using Match3.App.Interfaces;
using Match3.Template.SequenceDetectors;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3.LevelGoals
{
    public class CollectRowMaxItems : LevelGoal<ITerminalGridSlot>
    {
        private readonly int _maxRowLength;

        public CollectRowMaxItems(IGameBoard<ITerminalGridSlot> gameBoard)
        {
            _maxRowLength = GetMaxRowLength(gameBoard);
        }

        public override void OnSequencesSolved(IEnumerable<ItemSequence<ITerminalGridSlot>> sequences)
        {
            foreach (var sequence in sequences)
            {
                if (sequence.SequenceDetectorType != typeof(HorizontalLineDetector<ITerminalGridSlot>))
                {
                    continue;
                }

                if (sequence.SolvedGridSlots.Count == _maxRowLength)
                {
                    MarkAchieved();
                }
            }
        }

        private int GetMaxRowLength(IGameBoard<ITerminalGridSlot> gameBoard)
        {
            var maxRowLength = 0;

            for (var rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
            {
                var maxRowSlots = 0;
                var availableSlots = 0;

                for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
                {
                    if (gameBoard[rowIndex, columnIndex].CanContainItem)
                    {
                        availableSlots++;
                        continue;
                    }

                    if (availableSlots > maxRowSlots)
                    {
                        maxRowSlots = availableSlots;
                    }

                    availableSlots = 0;
                }

                var maxLength = Math.Max(maxRowSlots, availableSlots);
                if (maxLength > maxRowLength)
                {
                    maxRowLength = maxLength;
                }
            }

            return maxRowLength;
        }
    }
}