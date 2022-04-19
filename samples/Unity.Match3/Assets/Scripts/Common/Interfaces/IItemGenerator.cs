using Match3.Template.Interfaces;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IUnityItemGenerator : IItemGenerator
    {
        void SetSprites(Sprite[] sprites);
    }
}