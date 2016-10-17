using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class ProjectileState : Photon.MonoBehaviour
    {
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
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                mPreviousPosition = mAccuratePosition;
                mAccuratePosition = (Vector3)stream.ReceiveNext();
                transform.rotation = (Quaternion)stream.ReceiveNext();

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
    }
}
