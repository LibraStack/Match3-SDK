using Common.Interfaces;
using UnityEngine;

namespace Common
{
    public class UnityItem : MonoBehaviour, IUnityItem
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private bool _isDestroyed;

        public int ContentId { get; private set; }
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

        public void SetSprite(int spriteId, Sprite sprite)
        {
            ContentId = spriteId;
            _spriteRenderer.sprite = sprite;
        }

        public void SetWorldPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }

        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }

        public void SetScale(float value)
        {
            transform.localScale = new Vector3(value, value, value);
        }

        private void OnDestroy()
        {
            _isDestroyed = true;
        }

        public void Dispose()
        {
            if (_isDestroyed == false)
            {
                Destroy(gameObject);
            }
        }
    }
}