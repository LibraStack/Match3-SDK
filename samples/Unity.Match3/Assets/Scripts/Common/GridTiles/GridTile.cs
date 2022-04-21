using Common.Interfaces;
using UnityEngine;

namespace Common.GridTiles
{
    public abstract class GridTile : MonoBehaviour, IGridTile
    {
        private bool _isDestroyed;

        public abstract int GroupId { get; }
        public abstract bool IsLocked { get; }
        public abstract bool CanContainItem { get; }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void SetWorldPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
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