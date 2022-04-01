using UnityEngine;

namespace Common.Extensions
{
    public static class GameObjectExtensions
    {
        public static T CreateNew<T>(this GameObject gameObject, Vector3 position = default, Transform parent = null)
        {
            return gameObject.CreateNew(position, parent).GetComponent<T>();
        }

        public static GameObject CreateNew(this GameObject gameObject, Vector3 position, Transform parent = null)
        {
            return parent == null
                ? Object.Instantiate(gameObject, position, Quaternion.identity)
                : Object.Instantiate(gameObject, position, Quaternion.identity, parent);
        }
    }
}