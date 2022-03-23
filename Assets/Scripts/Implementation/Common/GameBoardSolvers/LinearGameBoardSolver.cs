using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Implementation.Common.GameBoardSolvers
{
    public class LinearGameBoardSolver : IGameBoardSolver
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

        public IReadOnlyCollection<ItemSequence> Solve(IGameBoard gameBoard, params GridPosition[] gridPositions)
        {
            var resultSequences = new Collection<ItemSequence>();

            foreach (var gridPosition in gridPositions)
            {
                TryGetSequence(gameBoard, gridPosition, ItemSequenceType.Vertical, resultSequences);
                TryGetSequence(gameBoard, gridPosition, ItemSequenceType.Horizontal, resultSequences);
            }

            return resultSequences;
        }

        private void TryGetSequence(IGameBoard gameBoard, GridPosition gridPosition, ItemSequenceType sequenceType,
            ICollection<ItemSequence> sequences)
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

        private ItemSequence GetSequence(IGameBoard gameBoard, GridPosition gridPosition, ItemSequenceType sequenceType)
        {
            var gridSlot = gameBoard[gridPosition];
            var gridSlots = new List<GridSlot>();
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

            return new ItemSequence(sequenceType, gridSlots);
        }

        private IEnumerable<GridSlot> GetSequenceOfGridSlots(IGameBoard gameBoard, GridSlot gridSlot,
            GridPosition gridPosition, GridPosition direction)
        {
            var newPosition = gridPosition + direction;
            var slotsSequence = new List<GridSlot>();

            while (gameBoard.IsPositionOnBoard(newPosition))
            {
                var currentSlot = gameBoard[newPosition];

                if (currentSlot.Item.SpriteId == gridSlot.Item.SpriteId)
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

        private bool IsNewSequence(ItemSequence newSequence, IEnumerable<ItemSequence> sequences)
        {
            var sequencesByType = sequences.Where(sequence => sequence.Type == newSequence.Type);
            var newSequenceGridSlot = newSequence.SolvedGridSlots[0];

            return sequencesByType.All(sequence => sequence.SolvedGridSlots.Contains(newSequenceGridSlot) == false);
        }

        private void MarkSolved(List<GridSlot> gridSlots)
        {
            foreach (var gridSlot in gridSlots)
            {
                gridSlot.MarkSolved();
            }
        }
    }
}