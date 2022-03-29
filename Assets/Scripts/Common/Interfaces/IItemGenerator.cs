using System;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IItemGenerator : IDisposable
    {
        void CreateItems(Sprite[] sprites, int capacity);
    }
}