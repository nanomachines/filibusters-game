using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class DepositCoinAnimatorController : MonoBehaviour
    {
        Animator mDepositCoinAnimator;
        [SerializeField]
        AnimationCurve mMotionCurve;
        Vector3 mOrigin;
        Vector3 mYDisplacement;
        int mOwnerId;

        void Start()
        {
            mDepositCoinAnimator = GetComponent<Animator>();
            EventSystem.OnCoinDepositedEvent += StartAnimation;

            mOrigin = transform.localPosition;
            mYDisplacement = new Vector3(0, 1f, 0);

            mOwnerId = transform.parent.gameObject.GetComponent<PhotonView>().owner.ID;
        }

        void OnDestroy()
        {
            EventSystem.OnCoinDepositedEvent -= StartAnimation;
        }

        void StartAnimation(int ownerId, int newDepositBalance, Vector3 pos)
        {
            if (mOwnerId == ownerId)
            {
                StartCoroutine(RunAnimation());
            }
        }

        IEnumerator RunAnimation()
        {
            mDepositCoinAnimator.SetBool("DepositCoinAnimation", true);

            float time = 0f;
            while (time < 1f)
            {
                var displacementRatio = mMotionCurve.Evaluate(time);
                transform.localPosition = mYDisplacement * displacementRatio + mOrigin;
                time += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            mDepositCoinAnimator.SetBool("DepositCoinAnimation", false);
        }
    }
}
