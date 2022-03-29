using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Implementation.Common.Extensions;
using Implementation.Common.Interfaces;
using Match3.Core.Models;
using UnityEngine;

namespace Implementation.ItemsScale.Jobs
{
    public class ItemsShowJob : Job
    {
        private const float ScaleDuration = 0.5f;

        private readonly IEnumerable<IUnityItem> _items;

        public ItemsShowJob(IEnumerable<IUnityItem> items, int executionOrder = 0) : base(executionOrder)
        {
            _items = items;
        }

        public override async UniTask ExecuteAsync()
        {
            var itemsSequence = DOTween.Sequence();

            foreach (var item in _items)
            {
                item.Transform.localScale = Vector3.zero;
                item.SpriteRenderer.SetAlpha(1);
                item.Show();

                _ = itemsSequence.Join(item.Transform.DOScale(Vector3.one, ScaleDuration));
            }

            await itemsSequence.SetEase(Ease.OutBounce);
        }
    }
}