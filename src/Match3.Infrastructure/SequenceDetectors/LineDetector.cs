using System.Collections.Generic;
using Match3.App;
using Match3.App.Interfaces;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.Infrastructure.SequenceDetectors
{
    public abstract class LineDetector<TGridSlot> : ISequenceDetector<TGridSlot> where TGridSlot : IGridSlot
    {
        public abstract ItemSequence<TGridSlot> GetSequence(IGameBoard<TGridSlot> gameBoard, GridPosition gridPosition);

        protected ItemSequence<TGridSlot> GetSequenceByDirection(IGameBoard<TGridSlot> gameBoard, GridPosition gridPosition,
            IEnumerable<GridPosition> directions)
        {
            var gridSlot = gameBoard[gridPosition];
            var gridSlots = new List<TGridSlot>();

            foreach (var direction in directions)
            {
                gridSlots.AddRange(GetSequenceOfGridSlots(gameBoard, gridSlot, gridPosition, direction));
            }

            if (gridSlots.Count < 2)
            {
                return null;
            }

            gridSlots.Add(gridSlot);

            return new ItemSequence<TGridSlot>(GetType(), gridSlots);
        }

        private IEnumerable<TGridSlot> GetSequenceOfGridSlots(IGameBoard<TGridSlot> gameBoard,
            TGridSlot gridSlot, GridPosition gridPosition, GridPosition direction)
        {
            var newPosition = gridPosition + direction;
            var slotsSequence = new List<TGridSlot>();

            while (gameBoard.IsPositionOnBoard(newPosition))
            {
                var currentSlot = gameBoard[newPosition];
                if (currentSlot.HasItem == false)
                {
                    break;
                }

                if (currentSlot.ItemId == gridSlot.ItemId)
                {
                    newPosition += direction;
                    slotsSequence.Add(currentSlot);
                }
                else
                {
                    break;
                }
            }

            return slotsSequence;
        }
    }
}