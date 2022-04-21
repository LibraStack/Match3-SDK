using Match3.Core.Interfaces;
using UnityEngine;

namespace Common.GridTiles
{
    public abstract class StatefulGridTile : SpriteGridTile, IStatefulSlot
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

            if (_currentStateIndex < _stateSpriteNames.Length)
            {
                _stateSpriteRenderer.sprite = GetStateSprite(_currentStateIndex);
                return true;
            }

            _stateSpriteRenderer.enabled = false;
            OnComplete();

            return false;
        }

        public void ResetState()
        {
            _currentStateIndex = 0;
            _stateSpriteRenderer.enabled = true;
            _stateSpriteRenderer.sprite = GetStateSprite(0);

            OnReset();
        }

        protected abstract void OnComplete();
        protected abstract void OnReset();

        private Sprite GetStateSprite(int index)
        {
            return GetSprite(_stateSpriteNames[index]);
        }
    }
}