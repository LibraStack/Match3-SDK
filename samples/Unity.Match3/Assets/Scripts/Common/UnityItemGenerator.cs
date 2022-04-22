using Common.Extensions;
using Common.Interfaces;
using Match3.Infrastructure;
using UnityEngine;
using Random = System.Random;

namespace Common
{
    public class UnityItemGenerator : ItemGenerator<IUnityItem>, IUnityItemGenerator
    {
        private readonly Random _random;
        private readonly Transform _container;
        private readonly GameObject _itemPrefab;

        private Sprite[] _sprites;

        public UnityItemGenerator(GameObject itemPrefab, Transform container)
        {
            _random = new Random();
            _container = container;
            _itemPrefab = itemPrefab;
        }

        public void SetSprites(Sprite[] sprites)
        {
            _sprites = sprites;
        }

        protected override IUnityItem CreateItem()
        {
            var item = _itemPrefab.CreateNew<IUnityItem>(parent: _container);
            item.Hide();

            return item;
        }

        protected override IUnityItem ConfigureItem(IUnityItem item)
        {
            var index = _random.Next(0, _sprites.Length);
            item.SetSprite(index, _sprites[index]);

            return item;
        }
    }
}