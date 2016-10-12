using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class FireFXController : MonoBehaviour
	{
        Animator mAnim;
		// Use this for initialization
		void Start()
		{
            mAnim = GetComponent<Animator>();
            mAnim.SetTrigger("Fire");
		}
		
		// Update is called once per frame
		void Update()
		{
		}
	}
}
