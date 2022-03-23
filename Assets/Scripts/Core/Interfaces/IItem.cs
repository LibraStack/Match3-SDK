using UnityEngine;

namespace Match3.Core.Interfaces
{
    public interface IItem
    {
        int SpriteId { get; }
        bool IsDestroyed { get; }
        Transform Transform { get; }
        SpriteRenderer SpriteRenderer { get; }

        void Show();
        void Hide();
        void SetSprite(int index, Sprite sprite);
        void SetWorldPosition(Vector3 position);
        Vector3 GetWorldPosition();
    }
}