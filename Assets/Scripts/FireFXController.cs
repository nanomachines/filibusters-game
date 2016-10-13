using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class FireFXController : MonoBehaviour
    {
        private Animator mAnim;

        void Start()
        {
            mAnim = GetComponent<Animator>();
            mAnim.SetTrigger("Fire");
        }
    }
}
