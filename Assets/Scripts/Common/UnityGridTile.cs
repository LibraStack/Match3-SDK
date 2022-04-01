using Common.Interfaces;
using UnityEngine;

namespace Common
{
    public class UnityGridTile : MonoBehaviour, IGridTile
    {
        private bool _isDestroyed;

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
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