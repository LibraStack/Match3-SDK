using Common.Interfaces;
using UnityEngine;

namespace Common.GridTiles
{
    public class StatefulGridTile : SpriteGridTile, IStatefulSlot
    {
        [Space]
        [SerializeField] private SpriteRenderer _stateSpriteRenderer;
        [SerializeField] private string[] _stateSpriteNames;

        private int _currentStateIndex;

        protected override void Start()
        {
            base.Start();
            _stateSpriteRenderer.sprite = GetStateSprite(_currentStateIndex);
        }

        public bool NextState()
        {
            _currentStateIndex++;

            if (_currentStateIndex >= _stateSpriteNames.Length)
            {
                _stateSpriteRenderer.enabled = false;
                return false;
            }

            _stateSpriteRenderer.sprite = GetStateSprite(_currentStateIndex);
            return true;
        }

        public void ResetState()
        {
            _currentStateIndex = 0;
            _stateSpriteRenderer.enabled = true;
            _stateSpriteRenderer.sprite = GetStateSprite(0);
        }

        private Sprite GetStateSprite(int index)
        {
            return GetSprite(_stateSpriteNames[index]);
        }
    }
}