﻿using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using Common.Structs;
using ItemsDropImplementation.Jobs;
using ItemsDropImplementation.Models;
using ItemsScaleImplementation.Jobs;
using UnityEngine;

namespace ItemsDropImplementation
{
    public class ItemsDropFillStrategy : IBoardFillStrategy
    {
        private readonly IGrid _gameBoard;
        private readonly IItemGenerator _itemGenerator;

        public string Name => "Drop Fill Strategy";

        public ItemsDropFillStrategy(IGrid gameBoard, IItemGenerator itemGenerator)
        {
            _gameBoard = gameBoard;
            _itemGenerator = itemGenerator;
        }

        public IEnumerable<IJob> GetFillJobs()
        {
            return GetFillJobs(0);
        }

        public IEnumerable<IJob> GetSolveJobs(IReadOnlyCollection<ItemSequence> sequences)
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
                    if (itemsMoveData.Count == 0)
                    {
                        continue;
                    }

                    jobs.Add(new ItemsMoveJob(itemsMoveData));
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

                if (CanDropDown(gridSlot, out Vector3 destinationWorldPosition) == false)
                {
                    continue;
                }

                var item = gridSlot.Item;
                gridSlot.Clear();
                itemsDropData.Add(new ItemMoveData(item, new List<Vector3> { destinationWorldPosition }));
                _gameBoard[destinationWorldPosition].SetItem(item);
            }

            itemsDropData.Reverse();
            return itemsDropData;
        }

        private IEnumerable<IJob> GetFillJobs(int priority)
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

                    var itemDropData = new ItemMoveData(item, new List<Vector3> { gridSlot.WorldPosition });

                    gridSlot.SetItem(item);
                    itemsDropData.Add(itemDropData);
                }

                if (itemsDropData.Count > 0)
                {
                    jobs.Add(new ItemsDropJob(itemsDropData, priority));
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

        private bool CanDropDown(GridSlot gridSlot, out Vector3 worldPosition)
        {
            var anyDrop = false;

            while (CanDropDown(gridSlot, out GridPosition downGridPosition))
            {
                anyDrop = true;
                gridSlot = _gameBoard[downGridPosition];
            }

            worldPosition = gridSlot.WorldPosition;
            return anyDrop;
        }

        private bool CanDropDown(GridSlot gridSlot, out GridPosition gridPosition)
        {
            var downGridSlot = GetSideGridSlot(gridSlot, GridPosition.Up);
            if (downGridSlot is { State: GridSlotState.Free } == false)
            {
                gridPosition = GridPosition.Zero;
                return false;
            }

            gridPosition = downGridSlot.GridPosition;
            return true;
        }

        private GridSlot GetSideGridSlot(GridSlot gridSlot, GridPosition direction)
        {
            var sideGridSlotPosition = gridSlot.GridPosition + direction;

            return _gameBoard.IsPositionOnGrid(sideGridSlotPosition)
                ? _gameBoard[sideGridSlotPosition]
                : null;
        }
    }
}