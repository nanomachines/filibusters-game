using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class ParallaxController : MonoBehaviour
    {
        Transform mCameraTransform;

        void Start()
        {
            mCameraTransform = GameObject.FindGameObjectWithTag(Tags.MAIN_CAMERA).transform;
        }

        void Update()
        {
            Vector2 cameraPos = mCameraTransform.position;
            var camX = mCameraTransform.position.x;
            var camY = mCameraTransform.position.y;
            foreach (Transform zGroupTransform in transform)
            {
                var groupZ = zGroupTransform.position.z;
                var xOffset = camX - (camX / groupZ);
                var yOffset = camY - (camY / groupZ);
                zGroupTransform.position = new Vector3(xOffset, yOffset, groupZ);
            }
        }
    }
}
