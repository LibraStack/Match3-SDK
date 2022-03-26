using System;
using UnityEngine;

namespace Implementation.Common.Interfaces
{
    public interface IItemGenerator : IDisposable
    {
        void CreateItems(Sprite[] sprites, int capacity);
    }
}