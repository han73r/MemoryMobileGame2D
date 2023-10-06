using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    // any static tools should be here
    public static class Tool
    {
        public static void RemoveChildObjects(Transform container)
        {
            foreach (Transform child in container)
            {
                Object.Destroy(child.gameObject);
            }
        }
        public static void SetTransformActive(Transform targetTransform, bool isActive)
        {
            if (targetTransform != null) { targetTransform.gameObject.SetActive(isActive); }
            else { Debug.LogWarning("Target Transform is null."); }
        }
        public static void ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            System.Random rng = new System.Random();

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}