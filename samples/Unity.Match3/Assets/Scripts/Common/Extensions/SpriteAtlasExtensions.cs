using UnityEngine;
using UnityEngine.U2D;

namespace Common.Extensions
{
    public static class SpriteAtlasExtensions
    {
        public static Sprite[] GetSprites(this SpriteAtlas spriteAtlas)
        {
            var sprites = new Sprite[spriteAtlas.spriteCount];
            spriteAtlas.GetSprites(sprites);

            return sprites;
        }
    }
}