using Match3.Infrastructure.Interfaces;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IUnityItemGenerator : IItemGenerator
    {
        void SetSprites(Sprite[] sprites);
    }
}