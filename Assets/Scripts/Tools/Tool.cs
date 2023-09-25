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
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }
    }
}