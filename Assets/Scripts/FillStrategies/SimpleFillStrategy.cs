using System.Collections.Generic;
using Common.Extensions;
using Common.Interfaces;
using FillStrategies.Jobs;
using Match3.App.Interfaces;
using Match3.App.Models;

namespace FillStrategies
{
    public class SimpleFillStrategy : BaseFillStrategy
    {
        private readonly IItemsPool<IUnityItem> _itemsPool;

        public SimpleFillStrategy(IUnityGameBoardRenderer gameBoardRenderer, IItemsPool<IUnityItem> itemsPool) 
            : base(gameBoardRenderer, itemsPool)
        {
            _itemsPool = itemsPool;
        }

        public override string Name => "Simple Fill Strategy";

        public override IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityItem> gameBoard,
            IEnumerable<ItemSequence<IUnityItem>> sequences)
        {
            var itemsToHide = new List<IUnityItem>();
            var itemsToShow = new List<IUnityItem>();

            foreach (var solvedGridSlot in sequences.GetUniqueGridSlots())
            {
                var newItem = _itemsPool.GetItem();
                var currentItem = solvedGridSlot.Item;

                newItem.SetWorldPosition(currentItem.GetWorldPosition());
                solvedGridSlot.SetItem(newItem);

                itemsToHide.Add(currentItem);
                itemsToShow.Add(newItem);

                _itemsPool.ReturnItem(currentItem);
            }

            return new IJob[] { new ItemsHideJob(itemsToHide), new ItemsShowJob(itemsToShow) };
        }
    }
}