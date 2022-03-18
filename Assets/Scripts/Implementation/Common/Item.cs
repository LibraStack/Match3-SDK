using Match3.Core.Interfaces;
using UnityEngine;

namespace Implementation.Common
{
    public class Item : MonoBehaviour, IItem
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public int SpriteIndex { get; private set; }
        public bool IsDestroyed { get; private set; }
        public Transform Transform => transform;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetSprite(int index, Sprite sprite)
        {
            SpriteIndex = index;
            _spriteRenderer.sprite = sprite;
        }

        public void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }

        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }

        private void OnDestroy()
        {
            IsDestroyed = true;
        }
    }
}
