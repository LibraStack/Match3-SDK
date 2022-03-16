using System.Collections.Generic;
using Common.Extensions;
using Common.Interfaces;
using Common.Models;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ItemsScaleImplementation.Jobs
{
    public class ItemsShowJob : Job
    {
        private const float ScaleDuration = 0.5f;

        private readonly IEnumerable<IItem> _items;

        public ItemsShowJob(IEnumerable<IItem> items, int priority = 0) : base(priority)
        {
            _items = items;
        }

        public override async UniTask ExecuteAsync()
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