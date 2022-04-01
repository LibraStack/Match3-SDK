using System;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IItemGenerator : IDisposable
    {
        void CreateItems(int capacity);
        void SetSprites(Sprite[] sprites);
    }
}