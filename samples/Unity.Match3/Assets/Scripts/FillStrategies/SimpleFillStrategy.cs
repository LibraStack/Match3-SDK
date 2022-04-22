using System.Collections.Generic;
using Common.Interfaces;
using FillStrategies.Jobs;
using Match3.App;
using Match3.App.Interfaces;
using Match3.Infrastructure.Extensions;

namespace FillStrategies
{
    public class SimpleFillStrategy : BaseFillStrategy
    {
        public SimpleFillStrategy(IAppContext appContext) : base(appContext)
        {
        }

        public override string Name => "Simple Fill Strategy";

        public override IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityGridSlot> gameBoard,
            SolvedData<IUnityGridSlot> solvedData)
        {
            var itemsToHide = new List<IUnityItem>();
            var itemsToShow = new List<IUnityItem>();

            foreach (var solvedGridSlot in solvedData.GetUniqueSolvedGridSlots(true))
            {
                var newItem = GetItemFromPool();
                var currentItem = solvedGridSlot.Item;

                newItem.SetWorldPosition(currentItem.GetWorldPosition());
                solvedGridSlot.SetItem(newItem);

                itemsToHide.Add(currentItem);
                itemsToShow.Add(newItem);

                ReturnItemToPool(currentItem);
            }

            foreach (var specialItemGridSlot in solvedData.GetSpecialItemGridSlots(true))
            {
                var item = GetItemFromPool();
                item.SetWorldPosition(GetWorldPosition(specialItemGridSlot.GridPosition));

                specialItemGridSlot.SetItem(item);
                itemsToShow.Add(item);
            }

            return new IJob[] { new ItemsHideJob(itemsToHide), new ItemsShowJob(itemsToShow) };
        }
    }
}