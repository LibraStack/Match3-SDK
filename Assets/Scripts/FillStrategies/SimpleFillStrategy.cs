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
        public SimpleFillStrategy(IAppContext appContext) : base(appContext)
        {
        }

        public override string Name => "Simple Fill Strategy";

        public override IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityItem> gameBoard,
            IEnumerable<ItemSequence<IUnityItem>> sequences)
        {
            var itemsToHide = new List<IUnityItem>();
            var itemsToShow = new List<IUnityItem>();

            foreach (var solvedGridSlot in sequences.GetUniqueGridSlots(true))
            {
                var newItem = GetItemFromPool();
                var currentItem = solvedGridSlot.Item;

                newItem.SetWorldPosition(currentItem.GetWorldPosition());
                solvedGridSlot.SetItem(newItem);

                itemsToHide.Add(currentItem);
                itemsToShow.Add(newItem);

                ReturnItemToPool(currentItem);
            }

            return new IJob[] { new ItemsHideJob(itemsToHide), new ItemsShowJob(itemsToShow) };
        }
    }
}