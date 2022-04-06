using UnityEngine;
using UnityEngine.U2D;

namespace Common.GridTiles
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteGridTile : GridTile
    {
        [SerializeField] private SpriteAtlas _spriteAtlas;
        [SerializeField] private string _spriteName;

        private void Start()
        {
            GetComponent<SpriteRenderer>().sprite = _spriteAtlas.GetSprite(_spriteName);
        }
    }
}