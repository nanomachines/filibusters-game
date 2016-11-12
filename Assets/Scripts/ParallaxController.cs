using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class ParallaxController : MonoBehaviour
    {
        [SerializeField]
        float zFactor;
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
            /*
            foreach (Transform zGroupTransform in transform)
            {
                var origin = zGroupTransform.gameObject.GetComponent<Origin>().position;
                var z = 1 / (zGroupTransform.localScale.y * .5f) * zFactor;
                var xOffset = (camX - (camX / z)) + origin.x;
                var yOffset = (camY - (camY / z)) + origin.y;
                zGroupTransform.position = new Vector3(xOffset, yOffset, zGroupTransform.position.z);
            }
            */
        }
    }
}
