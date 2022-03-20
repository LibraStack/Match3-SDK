using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Implementation.ItemsDrop.Models;

namespace Implementation.ItemsDrop.Jobs
{
    public class ItemsMoveJob : DropJob
    {
        private const float DelayDuration = 0.25f;
        private const float IntervalDuration = 0.25f;

        private readonly float _delay;
        private readonly IEnumerable<ItemMoveData> _itemsData;

        public ItemsMoveJob(IEnumerable<ItemMoveData> items, int delayMultiplier = 0, int executionOrder = 0)
            : base(executionOrder)
        {
            _itemsData = items;
            _delay = delayMultiplier * DelayDuration;
        }

        public override async UniTask ExecuteAsync()
        {
            var itemsSequence = DOTween.Sequence();

            foreach (var itemData in _itemsData)
            {
                var itemDropSequence = CreateItemMoveSequence(itemData);
                _ = itemsSequence
                    .Join(itemDropSequence)
                    .PrependInterval(itemDropSequence.Duration() * IntervalDuration);
            }

            await itemsSequence.SetDelay(_delay, false).SetEase(Ease.Flash);
        }
    }
}