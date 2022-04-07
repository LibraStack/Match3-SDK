using Common.Interfaces;
using UnityEngine;

namespace Common.GridTiles
{
    public class StatefulGridTile : SpriteGridTile, IStatefulTile
    {
        [Space]
        [SerializeField] private SpriteRenderer _stateSpriteRenderer;
        [SerializeField] private string _stateSpriteName;

        protected override void Start()
        {
            base.Start();
            _stateSpriteRenderer.sprite = GetSprite(_stateSpriteName);
        }

        public bool NextState()
        {
            if (_stateSpriteRenderer.enabled)
            {
                _stateSpriteRenderer.enabled = false;
            }

            return false;
        }

        public void ResetState()
        {
            throw new System.NotImplementedException();
        }
    }
}