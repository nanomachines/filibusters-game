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

        private PlayerState mPlayerState;

        private int mPlayerViewId;
        [SerializeField]
        private float mFlashTime;
        [SerializeField]
        private Color mFlashCol;
        private Color mOriginalCol;

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
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            mRenderers = new SpriteRenderer[renderers.Length - 1];
            int i = 0;
            foreach (SpriteRenderer sr in renderers)
            {
                if (sr.gameObject.tag != Tags.DEATH_EXPLOSION)
                {
                    mRenderers[i++] = sr;
                }
            }
            mPlayerState = GetComponent<PlayerState>();

            mPlayerViewId = GetComponentInParent<PhotonView>().viewID;
            mOriginalCol = new Color(1f, 1f, 1f);
            EventSystem.OnPlayerHitEvent += StartPlayerHitEffect;
        }

        void OnDestroy()
        {
            EventSystem.OnPlayerHitEvent -= StartPlayerHitEffect;
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

        void StartPlayerHitEffect(int playerViewId, Vector3 pos)
        {
            StartCoroutine(PlayerHitEffect(playerViewId));
        }

        IEnumerator PlayerHitEffect(int playerViewId)
        {
            if (playerViewId == mPlayerViewId)
            {
                foreach (var renderer in mRenderers)
                {
                    renderer.color = mFlashCol;
                }

                yield return new WaitForSeconds(mFlashTime);

                foreach (var renderer in mRenderers)
                {
                    renderer.color = mOriginalCol;
                }
            }
        }
    }
}
