using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Common.Extensions;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FillStrategies.Models;

namespace FillStrategies.Jobs
{
    public class ItemsFallJob : MoveJob
    {
        private const float FadeDuration = 0.15f;
        private const float DelayDuration = 0.35f;
        private const float IntervalDuration = 0.25f;

        private readonly float _delay;
        private readonly IEnumerable<ItemMoveData> _itemsData;

        public ItemsFallJob(IEnumerable<ItemMoveData> items, int delayMultiplier = 0, int executionOrder = 0)
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
                var itemMoveTween = CreateItemMoveTween(itemData);
                _ = itemsSequence
                    .Join(CreateItemFadeInTween(itemData.Item))
                    .Join(itemMoveTween).PrependInterval(itemMoveTween.Duration() * IntervalDuration);
            }

            await itemsSequence.SetDelay(_delay, false).SetEase(Ease.Flash);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Tween CreateItemFadeInTween(IUnityItem item)
        {
            item.SpriteRenderer.SetAlpha(0);
            item.SetScale(1);
            item.Show();

            return item.SpriteRenderer.DOFade(1, FadeDuration);
        }
    }
}