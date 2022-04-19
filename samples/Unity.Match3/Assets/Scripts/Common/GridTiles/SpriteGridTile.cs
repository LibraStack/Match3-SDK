using UnityEngine;
using UnityEngine.U2D;

namespace Common.GridTiles
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class SpriteGridTile : GridTile
    {
        [SerializeField] private SpriteAtlas _spriteAtlas;
        [SerializeField] private string _spriteName;

        protected virtual void Start()
        {
            GetComponent<SpriteRenderer>().sprite = GetSprite(_spriteName);
        }

        protected Sprite GetSprite(string spriteName)
        {
            return _spriteAtlas.GetSprite(spriteName);
        }
    }
}