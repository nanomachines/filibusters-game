using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class WeaponBob : MonoBehaviour
    {
        [SerializeField]
        AnimationCurve mAnimationCurve;

        float mTime;
        Vector3 mOrigin;
        Vector3 mYDisplacement; 

        void Start()
        {
            mTime = 0f;
            mOrigin = transform.position;
            mYDisplacement = new Vector3(0, 0.5f, 0);
        }

        void Update()
        {
            var displacementRatio = mAnimationCurve.Evaluate(mTime);
            transform.position = mYDisplacement * displacementRatio + mOrigin;
            mTime += Time.deltaTime;
        }
    }
}
