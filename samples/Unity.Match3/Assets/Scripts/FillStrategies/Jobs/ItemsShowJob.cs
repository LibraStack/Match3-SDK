using System.Collections.Generic;
using System.Threading;
using Common.Extensions;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.App;
using UnityEngine;

namespace FillStrategies.Jobs
{
    public class ItemsShowJob : Job
    {
        private const float ScaleDuration = 0.5f;

        private readonly IEnumerable<IUnityItem> _items;

        public ItemsShowJob(IEnumerable<IUnityItem> items, int executionOrder = 0) : base(executionOrder)
        {
            _items = items;
        }

        public override async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var itemsSequence = DOTween.Sequence();

            foreach (var item in _items)
            {
                item.SpriteRenderer.SetAlpha(1);
                item.SetScale(0);
                item.Show();

                _ = itemsSequence.Join(item.Transform.DOScale(Vector3.one, ScaleDuration));
            }

            await itemsSequence.SetEase(Ease.OutBounce).WithCancellation(cancellationToken);
        }
    }
}