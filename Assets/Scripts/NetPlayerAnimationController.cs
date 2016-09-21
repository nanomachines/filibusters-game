using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class NetPlayerAnimationController : MonoBehaviour
    {
        private SpriteRenderer mRenderer;
        private Animator mAnimator;
        private MotionState mMotionState;
        private bool mGrounded
        {
            get { return mMotionState.mGrounded; }
            set { mMotionState.mGrounded = value; }
        }
        private bool mFacingRight
        {
            get { return mMotionState.mFacingRight; }
            set { mMotionState.mFacingRight = value; }
        }
        private float mVelX
        {
            get { return mMotionState.mVelX; }
            set { mMotionState.mVelX = value; }
        }
        private float mVelY
        {
            get { return mMotionState.mVelY; }
            set { mMotionState.mVelY = value; }
        }
        private float mVelXMult
        {
            get { return mMotionState.mVelXMult; }
            set { mMotionState.mVelXMult = value; }
        }

        // Use this for initialization
        void Start()
        {
            mRenderer = GetComponent<SpriteRenderer>();
            mAnimator = GetComponent<Animator>();
            mMotionState = GetComponent<MotionState>();
        }

        // Update is called once per frame
        void Update()
        {
            mAnimator.SetFloat("VelocityY", mVelY);
            mAnimator.SetBool("Grounded", mGrounded);
            mAnimator.SetBool("Moving", Mathf.Abs(mVelX) > 0.001f);
            mAnimator.SetBool("FacingRight", mFacingRight);
            mRenderer.flipX = !mFacingRight;
        }
    }
}
