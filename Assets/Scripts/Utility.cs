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

        /*
         * This function strips scene names of any prepending paths
         * and returns the comparison of the actual scene names.
         * 
         * this fixes an issue where scene names are different depending on whether
         * we start them from a build or from the Unity editor
         */
        public static bool AreSceneNamesEqual(string sceneNameA, string sceneNameB)
        {
            var strippedNameA = stripPath(sceneNameA);
            var strippedNameB = stripPath(sceneNameB);
            return strippedNameA == strippedNameB;
        }

        private static string stripPath(string str)
        {
            int final_path_delimiter_pos = -1;
            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] == '/' || str[i] == '\\')
                {
                    final_path_delimiter_pos = i;
                }
            }
            return str.Substring(final_path_delimiter_pos + 1);
        }

        public static string PlayerNumberToPrefab(int num)
        {
            switch (num)
            {
                case 0:
                    return "FemaleRedPlayer";
                case 1:
                    return "MaleBluePlayer";
                case 2:
                    return "FemaleYellowPlayer";
                case 3:
                    return "MaleGreenPlayer";
            }

            return null;
        }
    }
}
