using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class SpikeBoundary : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == Tags.PLAYER)
            {
                other.gameObject.GetComponent<LifeManager>().Die();
            }
        }
    }
}
