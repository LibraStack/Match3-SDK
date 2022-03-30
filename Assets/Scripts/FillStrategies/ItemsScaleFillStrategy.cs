using System.Collections.Generic;
using Common.Interfaces;
using FillStrategies.Jobs;
using Match3.App.Interfaces;
using Match3.App.Models;
using Match3.Core.Enums;
using Match3.Core.Models;

namespace FillStrategies
{
    public class ItemsScaleFillStrategy : IBoardFillStrategy<IUnityItem>
    {
        private readonly IGameBoardRenderer _gameBoardRenderer;
        private readonly IItemsPool<IUnityItem> _itemsPool;

        public ItemsScaleFillStrategy(IGameBoardRenderer gameBoardRenderer, IItemsPool<IUnityItem> itemsPool)
        {
            _itemsPool = itemsPool;
            _gameBoardRenderer = gameBoardRenderer;
        }

        public string Name => "Scale Fill Strategy";

        public IEnumerable<IJob> GetFillJobs(IGameBoard<IUnityItem> gameBoard)
        {
            var itemsToShow = new List<IUnityItem>();

            for (var rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
                {
                    var gridSlot = gameBoard[rowIndex, columnIndex];
                    if (gridSlot.State != GridSlotState.Empty)
                    {
                        continue;
                    }

                    var item = _itemsPool.GetItem();
                    item.SetWorldPosition(_gameBoardRenderer.GetWorldPosition(rowIndex, columnIndex));

                    gridSlot.SetItem(item);
                    itemsToShow.Add(item);
                }
            }

            return new[] { new ItemsShowJob(itemsToShow) };
        }

        public IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityItem> gameBoard,
            IEnumerable<ItemSequence<IUnityItem>> sequences)
        {
            var itemsToHide = new List<IUnityItem>();
            var itemsToShow = new List<IUnityItem>();
            var solvedGridSlots = new HashSet<GridSlot<IUnityItem>>();

            foreach (var sequence in sequences)
            {
                foreach (var solvedGridSlot in sequence.SolvedGridSlots)
                {
                    if (solvedGridSlots.Add(solvedGridSlot) == false)
                    {
                        continue;
                    }

                    var oldItem = solvedGridSlot.Item;
                    _itemsPool.ReturnItem(oldItem);

                    var newItem = _itemsPool.GetItem();
                    newItem.SetWorldPosition(oldItem.GetWorldPosition());
                    solvedGridSlot.SetItem(newItem);

                    itemsToHide.Add(oldItem);
                    itemsToShow.Add(newItem);
                }
            }

            solvedGridSlots.Clear();

            return new IJob[] { new ItemsHideJob(itemsToHide), new ItemsShowJob(itemsToShow) };
        }
    }
}