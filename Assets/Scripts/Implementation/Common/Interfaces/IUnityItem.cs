using System;
using Match3.Core.Interfaces;
using UnityEngine;

namespace Implementation.Common.Interfaces
{
    public interface IUnityItem : IItem, IDisposable
    {
        Transform Transform { get; }
        SpriteRenderer SpriteRenderer { get; }
        
        void SetSprite(int index, Sprite sprite);
        void SetWorldPosition(Vector3 position);
        Vector3 GetWorldPosition();
    }
}