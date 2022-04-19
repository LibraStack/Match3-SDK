using System;

namespace Match3.Template.Interfaces
{
    public interface IItemGenerator : IDisposable
    {
        void CreateItems(int capacity);
    }
}