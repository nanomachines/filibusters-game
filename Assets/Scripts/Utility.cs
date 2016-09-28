using UnityEngine;

namespace Filibusters
{
    public static class Utility
    {
        public static GameObject GetChildWithTag(GameObject parent, string tag)
        {
            foreach (Transform childTransform in parent.transform)
            {
                if (childTransform.gameObject.tag.Equals(tag))
                {
                    return childTransform.gameObject;
                }
            }
            return null;
        }
    }
}
