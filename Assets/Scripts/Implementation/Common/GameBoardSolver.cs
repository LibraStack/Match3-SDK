using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Implementation.Common.Interfaces;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Implementation.Common
{
    public class GameBoardSolver : IGameBoardSolver<IUnityItem>
    {
        private readonly ISequenceDetector<IUnityItem>[] _sequenceDetectors;

        public GameBoardSolver(ISequenceDetector<IUnityItem>[] sequenceDetectors)
        {
            _sequenceDetectors = sequenceDetectors;
        }

        public IReadOnlyCollection<ItemSequence<IUnityItem>> Solve(IGameBoard<IUnityItem> gameBoard,
            params GridPosition[] gridPositions)
        {
            var resultSequences = new Collection<ItemSequence<IUnityItem>>();

            foreach (var gridPosition in gridPositions)
            {
                foreach (var sequenceDetector in _sequenceDetectors)
                {
                    var sequence = sequenceDetector.GetSequence(gameBoard, gridPosition);
                    if (sequence == null)
                    {
                        continue;
                    }

                    if (IsNewSequence(sequence, resultSequences))
                    {
                        MarkSolved(sequence.SolvedGridSlots);
                        resultSequences.Add(sequence);
                    }
                }
            }

            return resultSequences;
        }

        private bool IsNewSequence(ItemSequence<IUnityItem> newSequence,
            IEnumerable<ItemSequence<IUnityItem>> sequences)
        {
            var sequencesByType =
                sequences.Where(sequence => sequence.SequenceDetectorType == newSequence.SequenceDetectorType);
            var newSequenceGridSlot = newSequence.SolvedGridSlots[0];

            return sequencesByType.All(sequence => sequence.SolvedGridSlots.Contains(newSequenceGridSlot) == false);
        }

        private void MarkSolved(IEnumerable<GridSlot<IUnityItem>> gridSlots)
        {
            foreach (var gridSlot in gridSlots)
            {
                gridSlot.MarkSolved();
            }
        }
    }
}