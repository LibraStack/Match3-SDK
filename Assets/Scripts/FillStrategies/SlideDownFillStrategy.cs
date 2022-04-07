using System.Collections.Generic;
using System.Linq;
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
    public class SlideDownFillStrategy : BaseFillStrategy
    {
        private readonly IItemsPool<IUnityItem> _itemsPool;
        private readonly IUnityGameBoardRenderer _gameBoardRenderer;

        public SlideDownFillStrategy(IUnityGameBoardRenderer gameBoardRenderer, IItemsPool<IUnityItem> itemsPool) 
            : base(gameBoardRenderer, itemsPool)
        {
            _itemsPool = itemsPool;
            _gameBoardRenderer = gameBoardRenderer;
        }

        public override string Name => "Slide Down Fill Strategy";

        // public override IEnumerable<IJob> GetFillJobs(IGameBoard<IUnityItem> gameBoard)
        // {
        //     return GetFillJobs(gameBoard, 0, 0);
        // }

        public override IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityItem> gameBoard,
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

                    var currentItem = solvedGridSlot.Item;
                    itemsToHide.Add(currentItem);
                    solvedGridSlot.Clear();

                    _itemsPool.ReturnItem(currentItem);
                    _gameBoardRenderer.TrySetNextTileState(solvedGridSlot.GridPosition); // TODO: Change logic.
                }
            }

            foreach (var solvedGridSlot in
                     solvedGridSlots.OrderBy(slot => CanDropFromTop(gameBoard, slot.GridPosition)))
            {
                var itemsMoveData = GetColumnItemsMoveData(gameBoard, solvedGridSlot.GridPosition.ColumnIndex);
                if (itemsMoveData.Count != 0)
                {
                    jobs.Add(new ItemsMoveJob(itemsMoveData));
                }
            }

            solvedGridSlots.Clear();
            jobs.Add(new ItemsHideJob(itemsToHide));
            jobs.AddRange(GetRollDownJobs(gameBoard, 1, 0));
            jobs.AddRange(GetFillJobs(gameBoard, 0, 1));

            return jobs;
        }

        private IEnumerable<IJob> GetFillJobs(IGameBoard<IUnityItem> gameBoard, int delayMultiplier, int executionOrder)
        {
            var jobs = new List<IJob>();

            for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
            {
                var gridSlot = gameBoard[0, columnIndex];
                if (IsAvailableSlot(gridSlot) == false)
                {
                    continue;
                }

                jobs.Add(new ItemsFallJob(GetGenerateJobs(gameBoard, columnIndex), delayMultiplier, executionOrder));
            }

            return jobs;
        }

        private IEnumerable<ItemMoveData> GetGenerateJobs(IGameBoard<IUnityItem> gameBoard, int columnIndex)
        {
            var gridSlot = gameBoard[0, columnIndex];
            var itemsDropData = new List<ItemMoveData>();

            while (gridSlot.State != GridSlotState.Occupied)
            {
                var item = _itemsPool.GetItem();
                var itemGeneratorPosition = new GridPosition(-1, columnIndex);
                item.SetWorldPosition(GetWorldPosition(itemGeneratorPosition));

                var dropPositions = FilterPositions(gridSlot.GridPosition, GetDropPositions(gameBoard, gridSlot));
                if (dropPositions.Count == 0)
                {
                    gridSlot.SetItem(item);
                    itemsDropData.Add(new ItemMoveData(item, new[] { GetWorldPosition(gridSlot.GridPosition) }));
                    break;
                }

                var destinationGridPosition = dropPositions[dropPositions.Count - 1];
                var destinationGridSlot = gameBoard[destinationGridPosition];

                destinationGridSlot.SetItem(item);
                itemsDropData.Add(new ItemMoveData(item, dropPositions.Select(GetWorldPosition).ToArray()));
            }

            itemsDropData.Reverse();

            return itemsDropData;
        }

        private IEnumerable<IJob> GetRollDownJobs(IGameBoard<IUnityItem> gameBoard, int delayMultiplier,
            int executionOrder)
        {
            var jobs = new List<IJob>();

            for (var rowIndex = gameBoard.RowCount - 1; rowIndex >= 0; rowIndex--)
            {
                var itemsMoveData = GetRowItemsMoveData(gameBoard, rowIndex);

                if (itemsMoveData.Count != 0)
                {
                    jobs.Add(new ItemsMoveJob(itemsMoveData, delayMultiplier, executionOrder));
                }
            }

            return jobs;
        }

        private List<ItemMoveData> GetRowItemsMoveData(IGameBoard<IUnityItem> gameBoard, int rowIndex)
        {
            var itemsMoveData = new List<ItemMoveData>();

            for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
            {
                var itemMoveData = GetItemMoveData(gameBoard, rowIndex, columnIndex);
                if (itemMoveData != null)
                {
                    itemsMoveData.Add(itemMoveData);
                }
            }

            itemsMoveData.Reverse();
            return itemsMoveData;
        }

        private List<ItemMoveData> GetColumnItemsMoveData(IGameBoard<IUnityItem> gameBoard, int columnIndex)
        {
            var itemsMoveData = new List<ItemMoveData>();

            for (var rowIndex = gameBoard.RowCount - 1; rowIndex >= 0; rowIndex--)
            {
                var itemMoveData = GetItemMoveData(gameBoard, rowIndex, columnIndex);
                if (itemMoveData != null)
                {
                    itemsMoveData.Add(itemMoveData);
                }
            }

            itemsMoveData.Reverse();
            return itemsMoveData;
        }

        private ItemMoveData GetItemMoveData(IGameBoard<IUnityItem> gameBoard, int rowIndex, int columnIndex)
        {
            var gridSlot = gameBoard[rowIndex, columnIndex];
            if (IsMovableSlot(gridSlot) == false)
            {
                return null;
            }

            var dropPositions = FilterPositions(gridSlot.GridPosition, GetDropPositions(gameBoard, gridSlot));
            if (dropPositions.Count == 0)
            {
                return null;
            }

            var item = gridSlot.Item;
            gridSlot.Clear();
            gameBoard[dropPositions.Last()].SetItem(item);

            return new ItemMoveData(item, dropPositions.Select(GetWorldPosition).ToArray());
        }

        private bool CanDropFromTop(IGameBoard<IUnityItem> gameBoard, GridPosition gridPosition)
        {
            return CanDropFromTop(gameBoard, gridPosition.RowIndex, gridPosition.ColumnIndex);
        }

        private bool CanDropFromTop(IGameBoard<IUnityItem> gameBoard, int rowIndex, int columnIndex)
        {
            while (rowIndex >= 0)
            {
                if (IsAvailableSlot(gameBoard[rowIndex, columnIndex]) == false)
                {
                    return false;
                }

                rowIndex--;
            }

            return true;
        }

        private List<GridPosition> GetDropPositions(IGameBoard<IUnityItem> gameBoard, GridSlot<IUnityItem> gridSlot)
        {
            var dropGridPositions = new List<GridPosition>();

            while (gameBoard.CanMoveInDirection(gridSlot, GridPosition.Down, out var downGridPosition))
            {
                gridSlot = gameBoard[downGridPosition];
                dropGridPositions.Add(downGridPosition);
            }

            if (CanDropDiagonally(gameBoard, gridSlot, out var diagonalGridPosition) == false) return dropGridPositions;

            dropGridPositions.Add(diagonalGridPosition);
            dropGridPositions.AddRange(GetDropPositions(gameBoard, gameBoard[diagonalGridPosition]));

            return dropGridPositions;
        }

        private bool CanDropDiagonally(IGameBoard<IUnityItem> gameBoard, GridSlot<IUnityItem> gridSlot,
            out GridPosition gridPosition)
        {
            return CanDropDiagonally(gameBoard, gridSlot, GridPosition.Left, out gridPosition) ||
                   CanDropDiagonally(gameBoard, gridSlot, GridPosition.Right, out gridPosition);
        }

        private bool CanDropDiagonally(IGameBoard<IUnityItem> gameBoard, GridSlot<IUnityItem> gridSlot,
            GridPosition direction, out GridPosition gridPosition)
        {
            var sideGridSlot = gameBoard.GetSideGridSlot(gridSlot, direction);
            if (sideGridSlot == null || IsAvailableSlot(sideGridSlot))
            {
                gridPosition = GridPosition.Zero;
                return false;
            }

            return gameBoard.CanMoveInDirection(sideGridSlot, GridPosition.Down, out gridPosition);
        }

        private List<GridPosition> FilterPositions(GridPosition currentGridPosition, List<GridPosition> gridPositions)
        {
            if (gridPositions.Count == 0 || gridPositions.Count == 1)
            {
                return gridPositions;
            }

            var startColumnIndex = currentGridPosition.ColumnIndex;
            var filteredGridPositions = new HashSet<GridPosition>();

            for (var i = 0; i < gridPositions.Count; i++)
            {
                var gridPosition = gridPositions[i];

                if (startColumnIndex == gridPosition.ColumnIndex)
                {
                    if (i == gridPositions.Count - 1)
                    {
                        filteredGridPositions.Add(gridPosition);
                    }

                    continue;
                }

                if (i > 0)
                {
                    filteredGridPositions.Add(gridPositions[i - 1]);
                }

                filteredGridPositions.Add(gridPosition);

                startColumnIndex = gridPosition.ColumnIndex;
            }

            return filteredGridPositions.ToList();
        }
    }
}