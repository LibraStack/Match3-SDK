using System.Collections.Generic;
using Common.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ItemsDropImplementation.Models;
using UnityEngine;

namespace ItemsDropImplementation.Jobs
{
    public class ItemsDropJob : DropJob
    {
        private const float FadeDuration = 0.15f;
        private const float DelayDuration = 0.45f;
        private const float IntervalDuration = 0.25f;

        private readonly float _delay;
        private readonly IEnumerable<ItemMoveData> _itemsData;

        public ItemsDropJob(IEnumerable<ItemMoveData> items, int executionOrder = 0) : base(0)
        {
            _itemsData = items;
            _delay = executionOrder == 0 ? 0 : DelayDuration * executionOrder;
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