using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class FlipBulletSprite : MonoBehaviour
    {
        void Start()
        {
            int zRot = Mathf.RoundToInt(transform.rotation.eulerAngles.z);
            while (zRot < 0) // if zRot is a negative angle, make it its positive equivalent
            {
                zRot += 360;
            }
            zRot = zRot % 360; // ensure that zRot is between 0 and 360

            if (zRot > 90 && zRot <= 270)
            {
                GetComponent<SpriteRenderer>().flipY = true;
            }
        }
    }
}
