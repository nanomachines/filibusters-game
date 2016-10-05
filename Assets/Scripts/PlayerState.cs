using UnityEngine;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class PlayerState : Photon.MonoBehaviour
    {
        [HideInInspector]
        public bool mFacingRight;
        [HideInInspector]
        public float mVelX = 0f;
        [HideInInspector]
        public float mVelY = 0f;
        [HideInInspector]
        public float mVelXMult = 0f;
        [HideInInspector]
        public bool mGrounded = false;
        //[HideInInspector]
        public WeaponId mWeaponId = WeaponId.FISTS;
        public Aim mAimingDir = Aim.RIGHT;

        /*
         * Fields used to linearly interpolate
         * between position updates for the other
         * clients
         */
        private Vector3 mAccuratePosition;
        private Vector3 mPreviousPosition;
        private float mPositionLerpTime;
        private float mPositionUpdateRate = 0.1f;
        private int mNumUpdates;
        private float mTotalTime;

        // Use this for initialization
        void Awake()
        {
            mAccuratePosition = transform.position;
            mPreviousPosition = transform.position;
            mPositionLerpTime = 0;
            mNumUpdates = 1;
            mTotalTime = .1f;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(mVelXMult);
                stream.SendNext(mVelX);
                stream.SendNext(mVelY);
                stream.SendNext(mGrounded);
                stream.SendNext(mWeaponId);
                stream.SendNext(mAimingDir);
                stream.SendNext(mFacingRight);
                stream.SendNext(transform.position);
            }
            else
            {
                mVelXMult = (float)stream.ReceiveNext();
                mVelX = (float)stream.ReceiveNext();
                mVelY = (float)stream.ReceiveNext();
                mGrounded = (bool)stream.ReceiveNext();
                mWeaponId = (WeaponId)stream.ReceiveNext();
                mAimingDir = (Aim)stream.ReceiveNext();
                mFacingRight = (bool)stream.ReceiveNext();
                mPreviousPosition = mAccuratePosition;
                mAccuratePosition = (Vector3)stream.ReceiveNext();

                mPositionLerpTime = 0;
                ++mNumUpdates;
                mPositionUpdateRate = mTotalTime / mNumUpdates;
            }
        }

        void Update()
        {
            if (!photonView.isMine)
            {
                mTotalTime += Time.deltaTime;
                mPositionLerpTime += Time.deltaTime;
                transform.position = Vector3.Lerp(mPreviousPosition, mAccuratePosition, mPositionLerpTime / mPositionUpdateRate);
            }
        }

        public void ResetPosition()
        {
            mAccuratePosition = transform.position;
            mPreviousPosition = transform.position;
        }
    }
}
