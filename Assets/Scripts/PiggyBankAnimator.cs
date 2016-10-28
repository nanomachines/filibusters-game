using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class PiggyBankAnimator : MonoBehaviour
    {
        [SerializeField]
        AnimationCurve mJumpCurve;
        [SerializeField]
        AnimationCurve mRotateCurve;

        float mTime;
        Vector3 mOrigin;
        Vector3 mYDisplacement;

        Quaternion mOriginRot;
        Vector3 mBaseRotation;

        bool mDancing;

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
