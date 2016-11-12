using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class Origin : MonoBehaviour
    {
        public Vector3 position;
        void Awake()
        {
            position = transform.position;
            GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(transform.localScale.y * 1000f);
        }
    }
}
