using System;
using System.Collections.Generic;
using Match3.App;
using Match3.App.Interfaces;
using Match3.Infrastructure.Interfaces;
using Terminal.Match3.FillStrategies.Jobs;
using Terminal.Match3.Interfaces;
using Terminal.Match3.Extensions;

namespace Terminal.Match3.FillStrategies
{
    public class SimpleFillStrategy : IBoardFillStrategy<ITerminalGridSlot>
    {
        private readonly TerminalItem _notAvailableItem;
        private readonly IItemsPool<ITerminalItem> _itemsPool;
        private readonly ITerminalGameBoardRenderer _gameBoardRenderer;

        public SimpleFillStrategy(ITerminalGameBoardRenderer terminalGameBoardRenderer,
            IItemsPool<ITerminalItem> itemsPool)
        {
            _itemsPool = itemsPool;
            _notAvailableItem = new TerminalItem(' ', ConsoleColor.DarkGray);
            _gameBoardRenderer = terminalGameBoardRenderer;
        }

        public string Name => "Simple Fill Strategy";

        public IEnumerable<IJob> GetFillJobs(IGameBoard<ITerminalGridSlot> gameBoard)
        {
            var itemsToShow = new List<ITerminalItem>();

            for (var rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
                {
                    var gridSlot = gameBoard[rowIndex, columnIndex];
                    ITerminalItem item;

                    if (gridSlot.CanSetItem)
                    {
                        item = _itemsPool.GetItem();
                        itemsToShow.Add(item);
                    }
                    else
                    {
                        item = _notAvailableItem;
                    }

                    gridSlot.SetItem(item);
                }
            }

            return new[] { new ItemsShowJob(_gameBoardRenderer, itemsToShow) };
        }

        public IEnumerable<IJob> GetSolveJobs(IGameBoard<ITerminalGridSlot> gameBoard,
            SolvedData<ITerminalGridSlot> solvedData)
        {
            var itemsToShow = new List<ITerminalItem>();

            foreach (var solvedGridSlot in solvedData.GetUniqueSolvedGridSlots(true))
            {
                var newItem = _itemsPool.GetItem();
                var currentItem = solvedGridSlot.Item;

                solvedGridSlot.SetItem(newItem);
                itemsToShow.Add(newItem);

                _itemsPool.ReturnItem(currentItem);
            }

            return new Job[]
            {
                new ItemsShowJob(_gameBoardRenderer, itemsToShow)
            };
        }
    }
}