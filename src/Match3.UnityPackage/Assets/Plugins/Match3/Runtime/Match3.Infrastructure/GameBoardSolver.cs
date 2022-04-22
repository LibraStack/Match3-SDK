using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Match3.App;
using Match3.App.Interfaces;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.Infrastructure
{
    public class GameBoardSolver<TGridSlot> : IGameBoardSolver<TGridSlot> where TGridSlot : IGridSlot
    {
        private readonly ISpecialItemDetector<TGridSlot>[] _specialItemDetectors;
        private readonly ISequenceDetector<TGridSlot>[] _sequenceDetectors;

        public GameBoardSolver(ISequenceDetector<TGridSlot>[] sequenceDetectors,
            ISpecialItemDetector<TGridSlot>[] specialItemDetectors)
        {
            _sequenceDetectors = sequenceDetectors;
            _specialItemDetectors = specialItemDetectors;
        }

        public SolvedData<TGridSlot> Solve(IGameBoard<TGridSlot> gameBoard, params GridPosition[] gridPositions)
        {
            var resultSequences = new Collection<ItemSequence<TGridSlot>>();
            var specialItemGridSlots = new HashSet<TGridSlot>();

            foreach (var gridPosition in gridPositions)
            {
                foreach (var sequenceDetector in _sequenceDetectors)
                {
                    var sequence = sequenceDetector.GetSequence(gameBoard, gridPosition);
                    if (sequence == null)
                    {
                        continue;
                    }

                    if (IsNewSequence(sequence, resultSequences) == false)
                    {
                        continue;
                    }

                    foreach (var specialItemGridSlot in GetSpecialItemGridSlots(gameBoard, sequence))
                    {
                        specialItemGridSlots.Add(specialItemGridSlot);
                    }

                    resultSequences.Add(sequence);
                }
            }

            return new SolvedData<TGridSlot>(resultSequences, specialItemGridSlots);
        }

        private bool IsNewSequence(ItemSequence<TGridSlot> newSequence, IEnumerable<ItemSequence<TGridSlot>> sequences)
        {
            var sequencesByType =
                sequences.Where(sequence => sequence.SequenceDetectorType == newSequence.SequenceDetectorType);
            var newSequenceGridSlot = newSequence.SolvedGridSlots[0];

            return sequencesByType.All(sequence => sequence.SolvedGridSlots.Contains(newSequenceGridSlot) == false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<TGridSlot> GetSpecialItemGridSlots(IGameBoard<TGridSlot> gameBoard,
            ItemSequence<TGridSlot> sequence)
        {
            foreach (var itemDetector in _specialItemDetectors)
            {
                foreach (var solvedGridSlot in sequence.SolvedGridSlots)
                {
                    foreach (var specialItemGridSlot in itemDetector.GetSpecialItemGridSlots(gameBoard, solvedGridSlot))
                    {
                        var hasNextState = ((IStatefulSlot) specialItemGridSlot.State).NextState();
                        if (hasNextState)
                        {
                            continue;
                        }

                        yield return specialItemGridSlot;
                    }
                }
            }
        }
    }
}