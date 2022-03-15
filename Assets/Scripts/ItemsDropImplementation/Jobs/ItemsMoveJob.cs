using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ItemsDropImplementation.Models;

namespace ItemsDropImplementation.Jobs
{
    public class ItemsMoveJob : DropJob
    {
        private const float IntervalDuration = 0.25f;

        private readonly List<ItemMoveData> _itemsData;

        public ItemsMoveJob(List<ItemMoveData> items)
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