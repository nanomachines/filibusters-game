using UnityEngine;
using System.Collections;

public class OutOfBounds : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<LifeManager>().Die();
        }
    }
}
