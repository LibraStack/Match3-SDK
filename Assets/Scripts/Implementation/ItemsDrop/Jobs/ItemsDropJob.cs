using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Implementation.Common.Extensions;
using Implementation.ItemsDrop.Models;
using UnityEngine;

namespace Implementation.ItemsDrop.Jobs
{
    public class ItemsDropJob : DropJob
    {
        private const float FadeDuration = 0.15f;
        private const float DelayDuration = 0.45f;
        private const float IntervalDuration = 0.25f;
        // private const float IntervalDuration = 1.25f;

        private readonly float _delay;
        private readonly IEnumerable<ItemMoveData> _itemsData;

        public ItemsDropJob(IEnumerable<ItemMoveData> items, int delayMultiplier = 0, int executionOrder = 0) 
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
                itemData.Item.SpriteRenderer.SetAlpha(0);
                itemData.Item.Transform.localScale = Vector3.one;
                itemData.Item.Show();

                var itemDropSequence = CreateItemMoveSequence(itemData);
                _ = itemsSequence
                    .Join(itemData.Item.SpriteRenderer.DOFade(1, FadeDuration))
                    .Join(itemDropSequence).PrependInterval(itemDropSequence.Duration() * IntervalDuration);
            }

            await itemsSequence.SetDelay(_delay, false).SetEase(Ease.Flash);
        }
    }
}