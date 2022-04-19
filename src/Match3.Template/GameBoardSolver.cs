using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Match3.App;
using Match3.App.Interfaces;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.Template
{
    public class GameBoardSolver<TGridSlot> : IGameBoardSolver<TGridSlot> where TGridSlot : IGridSlot
    {
        private readonly ISequenceDetector<TGridSlot>[] _sequenceDetectors;

        public GameBoardSolver(ISequenceDetector<TGridSlot>[] sequenceDetectors)
        {
            _sequenceDetectors = sequenceDetectors;
        }

        public IReadOnlyCollection<ItemSequence<TGridSlot>> Solve(IGameBoard<TGridSlot> gameBoard,
            params GridPosition[] gridPositions)
        {
            var resultSequences = new Collection<ItemSequence<TGridSlot>>();

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
                        resultSequences.Add(sequence);
                    }
                }
            }

            return resultSequences;
        }

        private bool IsNewSequence(ItemSequence<TGridSlot> newSequence, IEnumerable<ItemSequence<TGridSlot>> sequences)
        {
            var sequencesByType =
                sequences.Where(sequence => sequence.SequenceDetectorType == newSequence.SequenceDetectorType);
            var newSequenceGridSlot = newSequence.SolvedGridSlots[0];

            return sequencesByType.All(sequence => sequence.SolvedGridSlots.Contains(newSequenceGridSlot) == false);
        }
    }
}