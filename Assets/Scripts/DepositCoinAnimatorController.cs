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

        void Start()
        {
            mDepositCoinAnimator = GetComponent<Animator>();
            EventSystem.OnCoinDepositedEvent += StartAnimation;

            mOrigin = transform.position;
            mYDisplacement = new Vector3(0, 1f, 0);
        }

        void OnDestroy()
        {
            EventSystem.OnCoinDepositedEvent -= StartAnimation;
        }

        void StartAnimation(int ownerId, int newDepositBalance)
        {
            StartCoroutine(RunAnimation());
        }

        IEnumerator RunAnimation()
        {
            mDepositCoinAnimator.SetBool("DepositCoinAnimation", true);

            float time = 0f;
            while (time < 1f)
            {
                var displacementRatio = mMotionCurve.Evaluate(time);
                transform.position = mYDisplacement * displacementRatio + mOrigin;
                time += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            mDepositCoinAnimator.SetBool("DepositCoinAnimation", false);
        }
    }
}
