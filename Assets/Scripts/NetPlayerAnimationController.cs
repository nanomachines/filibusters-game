using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class NetPlayerAnimationController : MonoBehaviour
    {
        private SpriteRenderer[] mRenderers;
        [SerializeField]
        private Animator mHeadTorsoAnim;
        [SerializeField]
        private Animator mLegsAnim;
        [SerializeField]
        private Animator mFrontArmAnim;
        [SerializeField]
        private Animator mBackArmAnim;

        private PlayerState mPlayerState;

        private bool mGrounded
        {
            get { return mPlayerState.mGrounded; }
        }
        private bool mFacingRight
        {
            get { return mPlayerState.mFacingRight; }
        }
        private float mVelX
        {
            get { return mPlayerState.mVelX; }
        }
        private float mVelY
        {
            get { return mPlayerState.mVelY; }
        }
        private int mAimingDir
        {
            get { return (int)mPlayerState.mAimingDir; }
        }
        private int mWeaponId
        {
            get { return (int)mPlayerState.mWeaponId; }
        }

        // Use this for initialization
        void Start()
        {
            mRenderers = GetComponentsInChildren<SpriteRenderer>();
            mPlayerState = GetComponent<PlayerState>();
        }

        // Update is called once per frame
        void Update()
        {
            bool moving = Mathf.Abs(mVelX) > Mathf.Epsilon;
            bool runningForward = moving &&
                ((Mathf.Sign(mVelX) == 1f && mFacingRight) || (Mathf.Sign(mVelX) == -1f && !mFacingRight));
            bool backpeddling = moving && !runningForward;

            mHeadTorsoAnim.SetBool("FacingRight", mFacingRight);
            mHeadTorsoAnim.SetInteger("Aim", mAimingDir);

            mLegsAnim.SetBool("RunForward", runningForward);
            mLegsAnim.SetBool("BackPeddle", backpeddling);
            mLegsAnim.SetFloat("JumpBlend", -mVelY);
            mLegsAnim.SetBool("Grounded", mGrounded);

            mBackArmAnim.SetBool("FacingRight", mFacingRight);
            mBackArmAnim.SetInteger("Aim", mAimingDir);
            mBackArmAnim.SetInteger("WeaponId", mWeaponId);

            mFrontArmAnim.SetBool("FacingRight", mFacingRight);
            mFrontArmAnim.SetInteger("Aim", mAimingDir);
            mFrontArmAnim.SetInteger("WeaponId", mWeaponId);

            foreach (var renderer in mRenderers)
            {
                renderer.flipX = !mFacingRight;
            }
        }

        public void SetRenderersEnabled(bool enabled)
        {
            foreach (var renderer in mRenderers)
            {
                renderer.enabled = enabled;
            }
        }
    }
}
