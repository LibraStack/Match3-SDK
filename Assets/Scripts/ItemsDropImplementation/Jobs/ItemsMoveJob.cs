using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ItemsDropImplementation.Models;

namespace ItemsDropImplementation.Jobs
{
    public class ItemsMoveJob : DropJob
    {
        private const float IntervalDuration = 0.25f;

        private readonly IEnumerable<ItemMoveData> _itemsData;

        public ItemsMoveJob(IEnumerable<ItemMoveData> items, int priority = 0) : base(priority)
        {
            _itemsData = items;
        }

        public override async UniTask ExecuteAsync()
        {
            var itemsSequence = DOTween.Sequence();

            foreach (var itemData in _itemsData)
            {
                var itemDropSequence = CreateItemMoveSequence(itemData);
                _ = itemsSequence.Join(itemDropSequence)
                    .PrependInterval(itemDropSequence.Duration() * IntervalDuration);
            }

            await itemsSequence.SetEase(Ease.Flash);
        }
    }
}