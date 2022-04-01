using System;

namespace Common.Interfaces
{
    public interface IGridTile : IDisposable
    {
        void SetActive(bool value);
    }
}