using System;

namespace Interfaces
{
    public interface IItemGenerator : IDisposable
    {
        void InitPool(int capacity);
        IItem GetItem();
        void ReturnItem(IItem item);
    }
}