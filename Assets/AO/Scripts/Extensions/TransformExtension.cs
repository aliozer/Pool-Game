
using UnityEngine;

namespace AO.Extensions
{
    public static class TransformExtension
    {
        public static void Clear(this Transform transform)
        {
            int i = 0;

            GameObject[] allChildren = new GameObject[transform.childCount];

            foreach (Transform child in transform)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }

            foreach (GameObject child in allChildren)
            {
                Object.DestroyImmediate(child.gameObject);
            }

        }
    }
}
