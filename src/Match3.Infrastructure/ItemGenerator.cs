using System;
using System.Collections.Generic;
using Match3.Infrastructure.Interfaces;

namespace Match3.Infrastructure
{
    public abstract class ItemGenerator<TItem> : IItemGenerator, IItemsPool<TItem>
    {
        private Queue<TItem> _itemsPool;

        public void CreateItems(int capacity)
        {
            if (_itemsPool != null)
            {
                throw new InvalidOperationException("Items have already been created.");
            }

            _itemsPool = new Queue<TItem>(capacity);

            for (var i = 0; i < capacity; i++)
            {
                _itemsPool.Enqueue(CreateItem());
            }
        }

        public TItem GetItem()
        {
            return ConfigureItem(_itemsPool.Dequeue());
        }

        public void ReturnItem(TItem item)
        {
            _itemsPool.Enqueue(item);
        }

        public virtual void Dispose()
        {
            if (_itemsPool == null)
            {
                return;
            }

            foreach (var item in _itemsPool)
            {
                if (item is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                else
                {
                    break;
                }
            }

            _itemsPool.Clear();
            _itemsPool = null;
        }

        protected abstract TItem CreateItem();
        protected abstract TItem ConfigureItem(TItem item);
    }
}