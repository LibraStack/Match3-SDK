using System;
using System.Collections.Generic;
using Implementation.Common.Interfaces;
using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using UnityEngine;

namespace Implementation.Common.LevelGoals
{
    public class CollectRowMaxItems : ILevelGoal
    {
        public event EventHandler Achieved;

        private readonly int _maxRowLength;
        
        public CollectRowMaxItems(IGameBoard<IUnityItem> gameBoard)
        {
            _maxRowLength = GetMaxRowLength(gameBoard);
        }

        public void RegisterSolvedSequences(IEnumerable<ItemSequence<IUnityItem>> sequences)
        {
            foreach (var sequence in sequences)
            {
                if (sequence.Type != ItemSequenceType.Horizontal)
                {
                    continue;
                }

                if (sequence.SolvedGridSlots.Count == _maxRowLength)
                {
                    Achieved?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        
        private int GetMaxRowLength(IGameBoard<IUnityItem> gameBoard)
        {
            var maxRowLength = 0;

            for (var rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
            {
                var maxRowSlots = 0;
                var availableSlots = 0;

                for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
                {
                    if (gameBoard[rowIndex, columnIndex].State != GridSlotState.NotAvailable)
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

                var maxLength = Mathf.Max(maxRowSlots, availableSlots);
                if (maxLength > maxRowLength)
                {
                    maxRowLength = maxLength;
                }
            }

            return maxRowLength;
        }
    }
}