using System;

namespace Interfaces
{
    public interface IItemGenerator : IDisposable
    {
        IItem GetItem();
        void ReturnItem(IItem item);
    }
}