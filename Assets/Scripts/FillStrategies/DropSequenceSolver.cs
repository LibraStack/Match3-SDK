using System.Collections.Generic;
using Enums;
using Interfaces;
using Models;
using UnityEngine;

namespace FillStrategies
{
    public abstract class DropSequenceSolver : ISequenceSolver
    {
        protected IGrid GameBoard { get; }

        protected DropSequenceSolver(IGrid gameBoard)
        {
            GameBoard = gameBoard;
        }

        public abstract IEnumerable<IJob> SolveSequences(List<ItemSequence> itemSequences);

        protected List<ItemDropData> GetItemsMoveData(GridSlot gridSlot)
        {
            var offset = 1;
            var itemsDropData = new List<ItemDropData>();
            var newUpPosition = gridSlot.GridPosition - GridPosition.Up;

            while (GameBoard.IsPositionOnBoard(newUpPosition))
            {
                var aboveSlot = GameBoard[newUpPosition.RowIndex, newUpPosition.ColumnIndex];

                newUpPosition -= GridPosition.Up;

                if (aboveSlot.State == GridSlotState.Free)
                {
                    offset++;
                    continue;
                }

                if (aboveSlot.State == GridSlotState.Occupied)
                {
                    var item = aboveSlot.Item;
                    var dropPosition = aboveSlot.GridPosition + new GridPosition(offset, 0);
                    var dropGridSlot = GameBoard[dropPosition.RowIndex, dropPosition.ColumnIndex];

                    var itemDropData =
                        new ItemDropData(item, new List<Vector3> {dropGridSlot.WorldPosition});

                    aboveSlot.Clear();
                    dropGridSlot.SetItem(item);
                    itemsDropData.Add(itemDropData);
                }
            }

            itemsDropData.Reverse();
            return itemsDropData;
        }
    }
}