using Common.Enums;
using Common.Interfaces;
using UnityEngine;

namespace Common
{
    public class UnityGridTile : MonoBehaviour, IGridTile
    {
        private bool _isDestroyed;

        public TileGroup Group { get; private set; }

        public void SetGroup(TileGroup group)
        {
            Group = group;
            gameObject.SetActive(group != TileGroup.Unavailable);
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