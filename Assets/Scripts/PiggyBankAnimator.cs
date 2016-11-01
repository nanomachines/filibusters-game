using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class PiggyBankAnimator : MonoBehaviour
    {
        [SerializeField]
        protected AnimationCurve mJumpCurve;
        [SerializeField]
        protected AnimationCurve mRotateCurve;

        protected float mTime;
        protected Vector3 mOrigin;
        protected Vector3 mYDisplacement;

        protected Quaternion mOriginRot;
        protected Vector3 mBaseRotation;

        protected bool mDancing;

        void Start()
        {
            mTime = 0f;
            mOrigin = transform.position;
            mYDisplacement = new Vector3(0, 0.5f, 0);
            mOriginRot = transform.rotation;
            mBaseRotation = new Vector3(0, 0, 30);

            mDancing = false;
            EventSystem.OnDepositBeginEvent += StartDancing;
            EventSystem.OnDepositEndEvent += StopDancing;
        }

        void OnDestroy()
        {
            EventSystem.OnDepositBeginEvent -= StartDancing;
            EventSystem.OnDepositEndEvent -= StopDancing;
        }

        void Update()
        {
            if (mDancing)
            {
                var jumpRatio = mJumpCurve.Evaluate(mTime);
                transform.position = mYDisplacement * jumpRatio + mOrigin;
                var rotateRatio = mRotateCurve.Evaluate(mTime);
                transform.rotation = mOriginRot;
                transform.Rotate(rotateRatio * mBaseRotation);
                mTime += Time.deltaTime;
            }
        }

        void StartDancing(int viewId)
        {
            mDancing = true;
        }

        void StopDancing()
        {
            mDancing = false;
            mTime = 0f;
            transform.position = mOrigin;
            transform.rotation = mOriginRot;
        }
    }
}
