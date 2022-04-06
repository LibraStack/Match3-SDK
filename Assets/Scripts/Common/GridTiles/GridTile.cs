using Common.Enums;
using Common.Interfaces;
using UnityEngine;

namespace Common.GridTiles
{
    public class GridTile : MonoBehaviour, IGridTile
    {
        [SerializeField] private TileGroup _group;

        private bool _isDestroyed;

        public TileGroup Group => _group;

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