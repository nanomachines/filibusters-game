using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class NetPlayerAnimationController : MonoBehaviour
    {
        private SpriteRenderer mRenderer;
        private Animator mAnimator;
        private PlayerState mPlayerState;
        private bool mGrounded
        {
            get { return mPlayerState.mGrounded; }
            set { mPlayerState.mGrounded = value; }
        }
        private bool mFacingRight
        {
            get { return mPlayerState.mFacingRight; }
            set { mPlayerState.mFacingRight = value; }
        }
        private float mVelX
        {
            get { return mPlayerState.mVelX; }
            set { mPlayerState.mVelX = value; }
        }
        private float mVelY
        {
            get { return mPlayerState.mVelY; }
            set { mPlayerState.mVelY = value; }
        }
        private float mVelXMult
        {
            get { return mPlayerState.mVelXMult; }
            set { mPlayerState.mVelXMult = value; }
        }

        // Use this for initialization
        void Start()
        {
            mRenderer = GetComponent<SpriteRenderer>();
            mAnimator = GetComponent<Animator>();
            mPlayerState = GetComponent<PlayerState>();
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
