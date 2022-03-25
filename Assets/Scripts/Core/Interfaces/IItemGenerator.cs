using System;

namespace Match3.Core.Interfaces
{
    public interface IItemGenerator<TItem> : IDisposable where TItem : IItem
    {
        void InitItemsPool(int capacity);
        TItem GetItem();
        void ReturnItem(TItem item);
    }
}