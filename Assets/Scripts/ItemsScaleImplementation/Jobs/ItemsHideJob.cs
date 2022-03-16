using System.Collections.Generic;
using Common.Interfaces;
using Common.Models;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ItemsScaleImplementation.Jobs
{
    public class ItemsHideJob : Job
    {
        private const float FadeDuration = 0.15f;
        private const float ScaleDuration = 0.5f;

        private readonly IEnumerable<IItem> _items;

        public ItemsHideJob(IEnumerable<IItem> items, int priority = 0) : base(priority)
        {
            _items = items;
        }

        public override async UniTask ExecuteAsync()
        {
            var itemsSequence = DOTween.Sequence();

            foreach (var item in _items)
            {
                _ = itemsSequence
                    .Join(item.Transform.DOScale(Vector3.zero, ScaleDuration))
                    .Join(item.SpriteRenderer.DOFade(0, FadeDuration));
            }

            await itemsSequence.SetEase(Ease.OutBounce);

            foreach (var item in _items)
            {
                item.Hide();
            }
        }
    }
}