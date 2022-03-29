using System.Collections.Generic;
using Common.Interfaces;
using Match3.App.Interfaces;
using Match3.App.Models;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Common.SequenceDetectors
{
    public abstract class LineDetector : ISequenceDetector<IUnityItem>
    {
        public abstract ItemSequence<IUnityItem> GetSequence(IGameBoard<IUnityItem> gameBoard,
            GridPosition gridPosition);

        protected ItemSequence<IUnityItem> GetSequenceByDirection(IGameBoard<IUnityItem> gameBoard,
            GridPosition gridPosition, IEnumerable<GridPosition> directions)
        {
            var gridSlot = gameBoard[gridPosition];
            var gridSlots = new List<GridSlot<IUnityItem>>();

            foreach (var direction in directions)
            {
                gridSlots.AddRange(GetSequenceOfGridSlots(gameBoard, gridSlot, gridPosition, direction));
            }

            if (gridSlots.Count < 2)
            {
                return null;
            }

            gridSlots.Add(gridSlot);

            return new ItemSequence<IUnityItem>(GetType(), gridSlots);
        }

        private IEnumerable<GridSlot<IUnityItem>> GetSequenceOfGridSlots(IGameBoard<IUnityItem> gameBoard,
            GridSlot<IUnityItem> gridSlot, GridPosition gridPosition, GridPosition direction)
        {
            var newPosition = gridPosition + direction;
            var slotsSequence = new List<GridSlot<IUnityItem>>();

            while (gameBoard.IsPositionOnBoard(newPosition))
            {
                var currentSlot = gameBoard[newPosition];
                if (currentSlot.Item == null)
                {
                    break;
                }

                if (currentSlot.Item.ContentId == gridSlot.Item.ContentId)
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