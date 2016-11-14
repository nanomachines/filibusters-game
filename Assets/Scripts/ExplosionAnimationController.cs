using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class ExplosionAnimationController : MonoBehaviour
    {
        int mPlayerViewId;
        Animator mAnimator;

        void Start()
        {
            EventSystem.OnDeathEvent += TriggerExplosion;
            mPlayerViewId = GetComponentInParent<PhotonView>().viewID;
            mAnimator = GetComponent<Animator>();
        }

        void OnDestroy()
        {
            EventSystem.OnDeathEvent -= TriggerExplosion;
        }

        void TriggerExplosion(int playerViewId, Vector3 pos)
        {
            if (playerViewId == mPlayerViewId)
            {
                mAnimator.SetTrigger("ExplosionTrigger");
            }
        }
    }
}
