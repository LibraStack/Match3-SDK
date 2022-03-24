using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Implementation.Common.Interfaces;
using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Implementation.Common.GameBoardSolvers
{
    public class LinearGameBoardSolver : IGameBoardSolver<IUnityItem>
    {
        private readonly Dictionary<ItemSequenceType, GridPosition[]> _sequenceDirections;

        public LinearGameBoardSolver()
        {
            _sequenceDirections = new Dictionary<ItemSequenceType, GridPosition[]>
            {
                { ItemSequenceType.Vertical, new[] { GridPosition.Up, GridPosition.Down } },
                { ItemSequenceType.Horizontal, new[] { GridPosition.Left, GridPosition.Right } }
            };
        }

        public IReadOnlyCollection<ItemSequence<IUnityItem>> Solve(IGameBoard<IUnityItem> gameBoard,
            params GridPosition[] gridPositions)
        {
            var resultSequences = new Collection<ItemSequence<IUnityItem>>();

            foreach (var gridPosition in gridPositions)
            {
                TryGetSequence(gameBoard, gridPosition, ItemSequenceType.Vertical, resultSequences);
                TryGetSequence(gameBoard, gridPosition, ItemSequenceType.Horizontal, resultSequences);
            }

            return resultSequences;
        }

        private void TryGetSequence(IGameBoard<IUnityItem> gameBoard, GridPosition gridPosition,
            ItemSequenceType sequenceType, ICollection<ItemSequence<IUnityItem>> sequences)
        {
            var sequence = GetSequence(gameBoard, gridPosition, sequenceType);
            if (sequence == null)
            {
                return;
            }

            if (IsNewSequence(sequence, sequences))
            {
                sequences.Add(sequence);
            }
        }

        private ItemSequence<IUnityItem> GetSequence(IGameBoard<IUnityItem> gameBoard, GridPosition gridPosition,
            ItemSequenceType sequenceType)
        {
            var gridSlot = gameBoard[gridPosition];
            var gridSlots = new List<GridSlot<IUnityItem>>();
            var directions = _sequenceDirections[sequenceType];

            foreach (var direction in directions)
            {
                gridSlots.AddRange(GetSequenceOfGridSlots(gameBoard, gridSlot, gridPosition, direction));
            }

            if (gridSlots.Count < 2)
            {
                return null;
            }

            gridSlots.Add(gridSlot);
            MarkSolved(gridSlots);

            return new ItemSequence<IUnityItem>(sequenceType, gridSlots);
        }

        private IEnumerable<GridSlot<IUnityItem>> GetSequenceOfGridSlots(IGameBoard<IUnityItem> gameBoard,
            GridSlot<IUnityItem> gridSlot, GridPosition gridPosition, GridPosition direction)
        {
            var newPosition = gridPosition + direction;
            var slotsSequence = new List<GridSlot<IUnityItem>>();

            while (gameBoard.IsPositionOnBoard(newPosition))
            {
                var currentSlot = gameBoard[newPosition];

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

        private bool IsNewSequence(ItemSequence<IUnityItem> newSequence,
            IEnumerable<ItemSequence<IUnityItem>> sequences)
        {
            var sequencesByType = sequences.Where(sequence => sequence.Type == newSequence.Type);
            var newSequenceGridSlot = newSequence.SolvedGridSlots[0];

            return sequencesByType.All(sequence => sequence.SolvedGridSlots.Contains(newSequenceGridSlot) == false);
        }

        private void MarkSolved(List<GridSlot<IUnityItem>> gridSlots)
        {
            foreach (var gridSlot in gridSlots)
            {
                gridSlot.MarkSolved();
            }
        }
    }
}