using System;

namespace Match3.Infrastructure.Interfaces
{
    public interface IItemGenerator : IDisposable
    {
        void CreateItems(int capacity);
    }
}