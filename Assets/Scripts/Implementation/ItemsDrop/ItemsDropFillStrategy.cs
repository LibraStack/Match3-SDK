using System.Collections.Generic;
using Implementation.ItemsDrop.Jobs;
using Implementation.ItemsDrop.Models;
using Implementation.ItemsScale.Jobs;
using Match3.Core.Enums;
using Match3.Core.Extensions;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Implementation.ItemsDrop
{
    public class ItemsDropFillStrategy : IBoardFillStrategy
    {
        private readonly IGameBoard _gameBoard;
        private readonly IItemGenerator _itemGenerator;

        public string Name => "Drop Fill Strategy";

        public ItemsDropFillStrategy(IGameBoard gameBoard, IItemGenerator itemGenerator)
        {
            _gameBoard = gameBoard;
            _itemGenerator = itemGenerator;
        }

        public IEnumerable<IJob> GetFillJobs()
        {
            return GetFillJobs(0);
        }

        public IEnumerable<IJob> GetSolveJobs(IEnumerable<ItemSequence> sequences)
        {
            var jobs = new List<IJob>();
            var itemsToHide = new List<IItem>();
            var solvedGridSlots = new HashSet<GridSlot>();

            foreach (var sequence in sequences)
            {
                foreach (var solvedGridSlot in sequence.SolvedGridSlots)
                {
                    if (solvedGridSlots.Add(solvedGridSlot) == false)
                    {
                        continue;
                    }

                    var item = solvedGridSlot.Item;
                    itemsToHide.Add(item);
                    solvedGridSlot.Clear();
                    _itemGenerator.ReturnItem(item);

                    var itemsMoveData = GetItemsMoveData(solvedGridSlot.GridPosition.ColumnIndex);
                    if (itemsMoveData.Count != 0)
                    {
                        jobs.Add(new ItemsMoveJob(itemsMoveData));
                    }
                }
            }

            solvedGridSlots.Clear();
            jobs.Add(new ItemsHideJob(itemsToHide));
            jobs.AddRange(GetFillJobs(1));

            return jobs;
        }

        private List<ItemMoveData> GetItemsMoveData(int columnIndex)
        {
            var itemsDropData = new List<ItemMoveData>();

            for (var rowIndex = _gameBoard.RowCount - 1; rowIndex >= 0; rowIndex--)
            {
                var gridSlot = _gameBoard[rowIndex, columnIndex];
                if (gridSlot.State != GridSlotState.Occupied)
                {
                    continue;
                }

                if (CanDropDown(gridSlot, out var destinationGridPosition) == false)
                {
                    continue;
                }

                var item = gridSlot.Item;
                gridSlot.Clear();
                itemsDropData.Add(
                    new ItemMoveData(item, new[] { _gameBoard.GetWorldPosition(destinationGridPosition) }));
                _gameBoard[destinationGridPosition].SetItem(item);
            }

            itemsDropData.Reverse();
            return itemsDropData;
        }

        private IEnumerable<IJob> GetFillJobs(int delayMultiplier)
        {
            var jobs = new List<IJob>();

            for (var columnIndex = 0; columnIndex < _gameBoard.ColumnCount; columnIndex++)
            {
                var itemsDropData = new List<ItemMoveData>();

                for (var rowIndex = 0; rowIndex < _gameBoard.RowCount; rowIndex++)
                {
                    var gridSlot = _gameBoard[rowIndex, columnIndex];
                    if (gridSlot.State != GridSlotState.Free)
                    {
                        continue;
                    }

                    var item = _itemGenerator.GetItem();
                    var itemGeneratorPosition = GetItemGeneratorPosition(rowIndex, columnIndex);
                    item.SetWorldPosition(_gameBoard.GetWorldPosition(itemGeneratorPosition));

                    var itemDropData =
                        new ItemMoveData(item, new[] { _gameBoard.GetWorldPosition(gridSlot.GridPosition) });

                    gridSlot.SetItem(item);
                    itemsDropData.Add(itemDropData);
                }

                if (itemsDropData.Count > 0)
                {
                    jobs.Add(new ItemsDropJob(itemsDropData, delayMultiplier));
                }
            }

            return jobs;
        }

        private GridPosition GetItemGeneratorPosition(int rowIndex, int columnIndex)
        {
            while (rowIndex >= 0)
            {
                if (_gameBoard[rowIndex, columnIndex].State == GridSlotState.NotAvailable)
                {
                    return new GridPosition(rowIndex, columnIndex);
                }

                rowIndex--;
            }

            return new GridPosition(-1, columnIndex);
        }

        private bool CanDropDown(GridSlot gridSlot, out GridPosition destinationGridPosition)
        {
            var destinationGridSlot = gridSlot;

            while (_gameBoard.CanMoveInDirection(destinationGridSlot, GridPosition.Down, out var bottomGridPosition))
            {
                destinationGridSlot = _gameBoard[bottomGridPosition];
            }

            destinationGridPosition = destinationGridSlot.GridPosition;
            return destinationGridSlot != gridSlot;
        }
    }
}