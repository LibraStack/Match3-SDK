using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Interfaces;
using FillStrategies.Jobs;
using FillStrategies.Models;
using Match3.App;
using Match3.App.Interfaces;
using Match3.Core.Structs;

namespace FillStrategies
{
    public class SlideDownFillStrategy : BaseFillStrategy
    {
        public SlideDownFillStrategy(IAppContext appContext) : base(appContext)
        {
        }

        public override string Name => "Slide Down Fill Strategy";

        public override IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityGridSlot> gameBoard,
            SolvedData<IUnityGridSlot> solvedData)
        {
            var jobs = new List<IJob>();
            var itemsToHide = new List<IUnityItem>();
            var solvedGridSlots = new HashSet<IUnityGridSlot>();

            foreach (var solvedGridSlot in solvedData.GetSolvedGridSlots())
            {
                if (solvedGridSlot.IsMovable == false)
                {
                    continue;
                }

                if (solvedGridSlots.Add(solvedGridSlot) == false)
                {
                    continue;
                }

                var currentItem = solvedGridSlot.Item;
                itemsToHide.Add(currentItem);
                solvedGridSlot.Clear();

                ReturnItemToPool(currentItem);
            }

            foreach (var specialItemGridSlot in solvedData.GetSpecialItemGridSlots())
            {
                solvedGridSlots.Add(specialItemGridSlot);
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

        private IEnumerable<IJob> GetFillJobs(IGameBoard<IUnityGridSlot> gameBoard, int delayMultiplier, int executionOrder)
        {
            var jobs = new List<IJob>();

            for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
            {
                var gridSlot = gameBoard[0, columnIndex];
                if (gridSlot.CanSetItem == false)
                {
                    continue;
                }

                jobs.Add(new ItemsFallJob(GetGenerateJobs(gameBoard, columnIndex), delayMultiplier, executionOrder));
            }

            return jobs;
        }

        private IEnumerable<ItemMoveData> GetGenerateJobs(IGameBoard<IUnityGridSlot> gameBoard, int columnIndex)
        {
            var gridSlot = gameBoard[0, columnIndex];
            var itemsDropData = new List<ItemMoveData>();

            while (gridSlot.HasItem == false)
            {
                var item = GetItemFromPool();
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

        private IEnumerable<IJob> GetRollDownJobs(IGameBoard<IUnityGridSlot> gameBoard, int delayMultiplier, int executionOrder)
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

        private List<ItemMoveData> GetRowItemsMoveData(IGameBoard<IUnityGridSlot> gameBoard, int rowIndex)
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

        private List<ItemMoveData> GetColumnItemsMoveData(IGameBoard<IUnityGridSlot> gameBoard, int columnIndex)
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

        private ItemMoveData GetItemMoveData(IGameBoard<IUnityGridSlot> gameBoard, int rowIndex, int columnIndex)
        {
            var gridSlot = gameBoard[rowIndex, columnIndex];
            if (gridSlot.IsMovable == false)
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

        private bool CanDropFromTop(IGameBoard<IUnityGridSlot> gameBoard, GridPosition gridPosition)
        {
            return CanDropFromTop(gameBoard, gridPosition.RowIndex, gridPosition.ColumnIndex);
        }

        private bool CanDropFromTop(IGameBoard<IUnityGridSlot> gameBoard, int rowIndex, int columnIndex)
        {
            while (rowIndex >= 0)
            {
                var gridSlot = gameBoard[rowIndex, columnIndex];
                if (gridSlot.NotAvailable)
                {
                    return false;
                }

                rowIndex--;
            }

            return true;
        }

        private List<GridPosition> GetDropPositions(IGameBoard<IUnityGridSlot> gameBoard, IUnityGridSlot gridSlot)
        {
            var dropGridPositions = new List<GridPosition>();

            while (gameBoard.CanMoveDown(gridSlot, out var downGridPosition))
            {
                gridSlot = gameBoard[downGridPosition];
                dropGridPositions.Add(downGridPosition);
            }

            if (CanDropDiagonally(gameBoard, gridSlot, out var diagonalGridPosition) == false)
            {
                return dropGridPositions;
            }

            dropGridPositions.Add(diagonalGridPosition);
            dropGridPositions.AddRange(GetDropPositions(gameBoard, gameBoard[diagonalGridPosition]));

            return dropGridPositions;
        }

        private bool CanDropDiagonally(IGameBoard<IUnityGridSlot> gameBoard, IUnityGridSlot gridSlot,
            out GridPosition gridPosition)
        {
            return CanDropDiagonally(gameBoard, gridSlot, GridPosition.Left, out gridPosition) ||
                   CanDropDiagonally(gameBoard, gridSlot, GridPosition.Right, out gridPosition);
        }

        private bool CanDropDiagonally(IGameBoard<IUnityGridSlot> gameBoard, IUnityGridSlot gridSlot,
            GridPosition direction, out GridPosition gridPosition)
        {
            var sideGridSlot = gameBoard.GetSideGridSlot(gridSlot, direction);
            var downGridSlot = gameBoard.GetSideGridSlot(gridSlot, GridPosition.Down);

            if (sideGridSlot is { NotAvailable: true } &&
                downGridSlot != null && downGridSlot.State.IsLocked == false)
            {
                return gameBoard.CanMoveDown(sideGridSlot, out gridPosition);
            }

            gridPosition = GridPosition.Zero;
            return false;
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