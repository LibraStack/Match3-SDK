using System;
using Match3.Core.Interfaces;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IUnityItem : IItem, IDisposable
    {
        Transform Transform { get; }
        SpriteRenderer SpriteRenderer { get; }

        void SetSprite(int spriteId, Sprite sprite);
        void SetWorldPosition(Vector3 worldPosition);
        Vector3 GetWorldPosition();
        void SetScale(float value);
    }
}