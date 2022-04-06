using Common.Interfaces;
using UnityEngine;

namespace Common.GridTiles
{
    public class IceGridTile : SpriteGridTile, IStatefulTile
    {
        [Space]
        [SerializeField] private SpriteRenderer _iceSpriteRenderer;

        public bool NextState()
        {
            if (_iceSpriteRenderer.enabled)
            {
                _iceSpriteRenderer.enabled = false;
            }

            return false;
        }
    }
}