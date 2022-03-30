using System.Collections.Generic;
using Common.Interfaces;
using FillStrategies.Jobs;
using FillStrategies.Models;
using Match3.App.Extensions;
using Match3.App.Interfaces;
using Match3.App.Models;
using Match3.Core.Enums;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace FillStrategies
{
    public class FallDownFillStrategy : IBoardFillStrategy<IUnityItem>
    {
        private readonly IGameBoardRenderer _gameBoardRenderer;
        private readonly IItemsPool<IUnityItem> _itemsPool;

        public FallDownFillStrategy(IGameBoardRenderer gameBoardRenderer, IItemsPool<IUnityItem> itemsPool)
        {
            _itemsPool = itemsPool;
            _gameBoardRenderer = gameBoardRenderer;
        }

        public string Name => "Fall Down Fill Strategy";

        public IEnumerable<IJob> GetFillJobs(IGameBoard<IUnityItem> gameBoard)
        {
            return GetFillJobs(gameBoard, 0);
        }

        public IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityItem> gameBoard,
            IEnumerable<ItemSequence<IUnityItem>> sequences)
        {
            var jobs = new List<IJob>();
            var itemsToHide = new List<IUnityItem>();
            var solvedGridSlots = new HashSet<GridSlot<IUnityItem>>();

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
                    _itemsPool.ReturnItem(item);

                    var itemsMoveData = GetItemsMoveData(gameBoard, solvedGridSlot.GridPosition.ColumnIndex);
                    if (itemsMoveData.Count != 0)
                    {
                        jobs.Add(new ItemsMoveJob(itemsMoveData));
                    }
                }
            }

            solvedGridSlots.Clear();
            jobs.Add(new ItemsHideJob(itemsToHide));
            jobs.AddRange(GetFillJobs(gameBoard, 1));

            return jobs;
        }

        private List<ItemMoveData> GetItemsMoveData(IGameBoard<IUnityItem> gameBoard, int columnIndex)
        {
            var itemsDropData = new List<ItemMoveData>();

            for (var rowIndex = gameBoard.RowCount - 1; rowIndex >= 0; rowIndex--)
            {
                var gridSlot = gameBoard[rowIndex, columnIndex];
                if (gridSlot.State != GridSlotState.Occupied)
                {
                    continue;
                }

                if (CanDropDown(gameBoard, gridSlot, out var destinationGridPosition) == false)
                {
                    continue;
                }

                var item = gridSlot.Item;
                gridSlot.Clear();
                itemsDropData.Add(
                    new ItemMoveData(item, new[] { _gameBoardRenderer.GetWorldPosition(destinationGridPosition) }));
                gameBoard[destinationGridPosition].SetItem(item);
            }

            itemsDropData.Reverse();
            return itemsDropData;
        }

        private IEnumerable<IJob> GetFillJobs(IGameBoard<IUnityItem> gameBoard, int delayMultiplier)
        {
            var jobs = new List<IJob>();

            for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
            {
                var itemsDropData = new List<ItemMoveData>();

                for (var rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
                {
                    var gridSlot = gameBoard[rowIndex, columnIndex];
                    if (gridSlot.State != GridSlotState.Empty)
                    {
                        continue;
                    }

                    var item = _itemsPool.GetItem();
                    var itemGeneratorPosition = GetItemGeneratorPosition(gameBoard, rowIndex, columnIndex);
                    item.SetWorldPosition(_gameBoardRenderer.GetWorldPosition(itemGeneratorPosition));

                    var itemDropData =
                        new ItemMoveData(item, new[] { _gameBoardRenderer.GetWorldPosition(gridSlot.GridPosition) });

                    gridSlot.SetItem(item);
                    itemsDropData.Add(itemDropData);
                }

                if (itemsDropData.Count > 0)
                {
                    jobs.Add(new ItemsFallJob(itemsDropData, delayMultiplier));
                }
            }

            return jobs;
        }

        private GridPosition GetItemGeneratorPosition(IGameBoard<IUnityItem> gameBoard, int rowIndex, int columnIndex)
        {
            while (rowIndex >= 0)
            {
                if (gameBoard[rowIndex, columnIndex].State == GridSlotState.NotAvailable)
                {
                    return new GridPosition(rowIndex, columnIndex);
                }

                rowIndex--;
            }

            return new GridPosition(-1, columnIndex);
        }

        private bool CanDropDown(IGameBoard<IUnityItem> gameBoard, GridSlot<IUnityItem> gridSlot,
            out GridPosition destinationGridPosition)
        {
            var destinationGridSlot = gridSlot;

            while (gameBoard.CanMoveInDirection(destinationGridSlot, GridPosition.Down, out var bottomGridPosition))
            {
                destinationGridSlot = gameBoard[bottomGridPosition];
            }

            destinationGridPosition = destinationGridSlot.GridPosition;
            return destinationGridSlot != gridSlot;
        }
    }
}