using System.Collections.Generic;
using Implementation.Common.Interfaces;
using Match3.Core.Interfaces;
using UnityEngine;
using Random = System.Random;

namespace Implementation.Common
{
    public class ItemGenerator : MonoBehaviour, IItemGenerator, IItemsPool<IUnityItem>
    {
        [SerializeField] private GameObject _itemPrefab;

        private Random _random;
        private Sprite[] _sprites;
        private Queue<IUnityItem> _itemsPool;

        public void CreateItems(Sprite[] sprites, int capacity)
        {
            if (_itemsPool != null)
            {
                Debug.LogError("Items have already been created.");
                return;
            }

            _sprites = sprites;
            _random = new Random();
            _itemsPool = new Queue<IUnityItem>(capacity);

            for (var i = 0; i < capacity; i++)
            {
                _itemsPool.Enqueue(CreateItem());
            }
        }

        public IUnityItem GetItem()
        {
            var item = _itemsPool.Dequeue();
            var (index, sprite) = GetRandomSprite();

            item.SetSprite(index, sprite);

            return item;
        }

        public void ReturnItem(IUnityItem item)
        {
            _itemsPool.Enqueue(item);
        }

        public void Dispose()
        {
            foreach (var item in _itemsPool)
            {
                item.Dispose();
            }

            _itemsPool.Clear();
        }

        private IUnityItem CreateItem()
        {
            var item = Instantiate(_itemPrefab, transform).GetComponent<IUnityItem>();
            item.Hide();

            return item;
        }

        private (int, Sprite) GetRandomSprite()
        {
            var index = _random.Next(0, _sprites.Length);
            return (index, _sprites[index]);
        }
    }
}