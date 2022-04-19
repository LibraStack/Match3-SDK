using System.Collections.Generic;
using System.Threading;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.App;
using UnityEngine;

namespace FillStrategies.Jobs
{
    public class ItemsHideJob : Job
    {
        private const float FadeDuration = 0.15f;
        private const float ScaleDuration = 0.25f;

        private readonly IEnumerable<IUnityItem> _items;

        public ItemsHideJob(IEnumerable<IUnityItem> items, int executionOrder = 0) : base(executionOrder)
        {
            _items = items;
        }

        public override async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var itemsSequence = DOTween.Sequence();

            foreach (var item in _items)
            {
                _ = itemsSequence
                    .Join(item.Transform.DOScale(Vector3.zero, ScaleDuration))
                    .Join(item.SpriteRenderer.DOFade(0, FadeDuration));
            }

            await itemsSequence.WithCancellation(cancellationToken);

            foreach (var item in _items)
            {
                item.Hide();
            }
        }
    }
}