using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using Common.Structs;

namespace Common.Solvers
{
    public class LinearGameBoardSolver : IGameBoardSolver
    {
        private readonly IGrid _gameBoard;
        private readonly Dictionary<ItemSequenceType, GridPosition[]> _sequenceDirections;

        public LinearGameBoardSolver(IGrid gameBoard)
        {
            _gameBoard = gameBoard;
            _sequenceDirections = new Dictionary<ItemSequenceType, GridPosition[]>
            {
                {ItemSequenceType.Vertical, new[] {GridPosition.Up, GridPosition.Down}},
                {ItemSequenceType.Horizontal, new[] {GridPosition.Left, GridPosition.Right}}
            };
        }

        public IReadOnlyCollection<ItemSequence> Solve(IGrid gameBoard, params GridPosition[] positions)
        {
            var resultSequences = new Collection<ItemSequence>();

            foreach (var position in positions)
            {
                TryGetSequence(gameBoard, position, ItemSequenceType.Vertical, resultSequences);
                TryGetSequence(gameBoard, position, ItemSequenceType.Horizontal, resultSequences);
            }

            return resultSequences;
        }

        private void TryGetSequence(IGrid gameBoard, GridPosition position, ItemSequenceType sequenceType,
            ICollection<ItemSequence> sequences)
        {
            var sequence = GetSequence(gameBoard, position, sequenceType);
            if (sequence == null)
            {
                return;
            }

            if (IsNewSequence(sequence, sequences))
            {
                sequences.Add(sequence);
            }
        }

        private ItemSequence GetSequence(IGrid gameBoard, GridPosition position, ItemSequenceType sequenceType)
        {
            var gridSlots = new List<GridSlot>();
            var slot = gameBoard[position.RowIndex, position.ColumnIndex];
            var directions = _sequenceDirections[sequenceType];

            foreach (var direction in directions)
            {
                gridSlots.AddRange(GetSequenceOfGridSlots(gameBoard, slot, position, direction));
            }

            if (gridSlots.Count < 2)
            {
                return null;
            }

            gridSlots.Add(slot);
            MarkSolved(gridSlots);

            return new ItemSequence(sequenceType, gridSlots);
        }

        private IEnumerable<GridSlot> GetSequenceOfGridSlots(IGrid gameBoard, GridSlot slot, GridPosition position,
            GridPosition direction)
        {
            var newPosition = position + direction;
            var slotsSequence = new List<GridSlot>();

            while (_gameBoard.IsPositionOnBoard(newPosition))
            {
                var currentSlot = gameBoard[newPosition.RowIndex, newPosition.ColumnIndex];

                if (currentSlot.Item.SpriteIndex == slot.Item.SpriteIndex)
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