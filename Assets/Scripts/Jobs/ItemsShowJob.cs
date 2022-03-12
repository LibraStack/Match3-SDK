using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using Interfaces;
using UnityEngine;

namespace Jobs
{
    public class ItemsShowJob : IJob
    {
        private const float ScaleDuration = 0.5f;

        private readonly IEnumerable<IItem> _items;

        public ItemsShowJob(IEnumerable<IItem> items)
        {
            _items = items;
        }

        public async UniTask ExecuteAsync()
        {
            var itemsSequence = DOTween.Sequence();

            foreach (var item in _items)
            {
                item.SpriteRenderer.SetAlpha(1);
                item.Transform.localScale = Vector3.zero;
                item.Show();

                _ = itemsSequence.Join(item.Transform.DOScale(Vector3.one, ScaleDuration));
            }

            await itemsSequence.SetEase(Ease.OutBounce);
        }
    }
}