using System.Collections.Generic;
using Enums;
using Interfaces;
using Jobs;
using Models;
using UnityEngine;

namespace FillStrategies
{
    public class ItemsDropFillStrategy : IBoardFillStrategy
    {
        private readonly IGrid _gameBoard;
        private readonly IItemGenerator _itemGenerator;
        private readonly Dictionary<ItemSequenceType, ISequenceSolver> _itemSequenceSolver;

        public string Name => "Items Drop Fill Strategy";

        public ItemsDropFillStrategy(IGrid gameBoard, IItemGenerator itemGenerator)
        {
            _gameBoard = gameBoard;
            _itemGenerator = itemGenerator;
            _itemSequenceSolver = new Dictionary<ItemSequenceType, ISequenceSolver>
            {
                {ItemSequenceType.Vertical, new VerticalSequenceSolver(gameBoard)},
                {ItemSequenceType.Horizontal, new HorizontalSequenceSolver(gameBoard)}
            };
        }

        public IEnumerable<IJob> GetFillJobs()
        {
            var jobs = new List<IJob>();

            for (var columnIndex = 0; columnIndex < _gameBoard.ColumnCount; columnIndex++)
            {
                var itemsDropData = new List<ItemDropData>();

                for (var rowIndex = 0; rowIndex < _gameBoard.RowCount; rowIndex++)
                {
                    var gridSlot = _gameBoard[rowIndex, columnIndex];
                    if (gridSlot.State != GridSlotState.Free)
                    {
                        continue;
                    }

                    var item = _itemGenerator.GetItem();
                    item.SetWorldPosition(_gameBoard.GetWorldPosition(-1, columnIndex));

                    var itemDropData = new ItemDropData(item, new List<Vector3> {gridSlot.WorldPosition});

                    gridSlot.SetItem(item);
                    itemsDropData.Add(itemDropData);
                }

                jobs.Add(new ItemsDropJob(itemsDropData));
            }

            return jobs;
        }

        public IEnumerable<IJob> GetSolveJobs(IReadOnlyCollection<ItemSequence> sequences)
        {
            var jobs = new List<IJob>();
            var itemsToHide = new HashSet<IItem>();
            var solvedGridSlots = GetUniqGridSlots(sequences);

            foreach (var solvedGridSlot in solvedGridSlots)
            {
                var item = solvedGridSlot.Item;

                itemsToHide.Add(item);
                solvedGridSlot.Clear();

                _itemGenerator.ReturnItem(item);
            }

            jobs.Add(new ItemsHideJob(itemsToHide));

            var groupedSequences = new Dictionary<ItemSequenceType, List<ItemSequence>>();
            foreach (var sequence in sequences)
            {
                if (groupedSequences.ContainsKey(sequence.Type))
                {
                    groupedSequences[sequence.Type].Add(sequence);
                }
                else
                {
                    groupedSequences.Add(sequence.Type, new List<ItemSequence> {sequence});
                }
            }

            foreach (var sequenceGroup in groupedSequences)
            {
                jobs.AddRange(_itemSequenceSolver[sequenceGroup.Key].SolveSequences(sequenceGroup.Value));
            }

            jobs.AddRange(GetFillJobs());

            return jobs;
        }

        private IEnumerable<GridSlot> GetUniqGridSlots(IEnumerable<ItemSequence> sequences)
        {
            var solvedGridSlots = new HashSet<GridSlot>();

            foreach (var sequence in sequences)
            {
                foreach (var solvedGridSlot in sequence.SolvedGridSlots)
                {
                    solvedGridSlots.Add(solvedGridSlot);
                }
            }

            return solvedGridSlots;
        }
    }
}