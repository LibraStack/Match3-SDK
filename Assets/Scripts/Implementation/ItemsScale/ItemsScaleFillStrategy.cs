using System.Collections.Generic;
using Implementation.Common.Interfaces;
using Implementation.ItemsScale.Jobs;
using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Models;

namespace Implementation.ItemsScale
{
    public class ItemsScaleFillStrategy : IBoardFillStrategy<IUnityItem>
    {
        private readonly IGameBoard<IUnityItem> _gameBoard;
        private readonly IItemGenerator<IUnityItem> _itemGenerator;

        public string Name => "Scale Fill Strategy";

        public ItemsScaleFillStrategy(IGameBoard<IUnityItem> gameBoard, IItemGenerator<IUnityItem> itemGenerator)
        {
            _gameBoard = gameBoard;
            _itemGenerator = itemGenerator;
        }

        public IEnumerable<IJob> GetFillJobs()
        {
            var itemsToShow = new List<IUnityItem>();

            for (var rowIndex = 0; rowIndex < _gameBoard.RowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _gameBoard.ColumnCount; columnIndex++)
                {
                    var gridSlot = _gameBoard[rowIndex, columnIndex];
                    if (gridSlot.State != GridSlotState.Free)
                    {
                        continue;
                    }

                    var item = _itemGenerator.GetItem();
                    item.SetWorldPosition(_gameBoard.GetWorldPosition(rowIndex, columnIndex));

                    gridSlot.SetItem(item);
                    itemsToShow.Add(item);
                }
            }

            return new[] { new ItemsShowJob(itemsToShow) };
        }

        public IEnumerable<IJob> GetSolveJobs(IEnumerable<ItemSequence<IUnityItem>> sequences)
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
                    _itemGenerator.ReturnItem(oldItem);

                    var newItem = _itemGenerator.GetItem();
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