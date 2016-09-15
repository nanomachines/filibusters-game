using UnityEngine;
using System.Collections;

public class MotionState : Photon.MonoBehaviour
{
    private Vector3 mAccuratePosition;

    [HideInInspector] public bool mFacingRight;
    [HideInInspector] public float mVelX = 0f;
    [HideInInspector] public float mVelY = 0f;
    [HideInInspector] public float mVelXMult = 0f;
    [HideInInspector] public bool mGrounded = false;

	// Use this for initialization
	void Awake()
	{
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(mVelXMult);
            stream.SendNext(mVelX);
            stream.SendNext(mVelY);
            stream.SendNext(mGrounded);
            stream.SendNext(mFacingRight);
            stream.SendNext(transform.position);
        }
        else
        {
            mVelXMult = (float)stream.ReceiveNext();
            mVelX = (float)stream.ReceiveNext();
            mVelY = (float)stream.ReceiveNext();
            mGrounded = (bool)stream.ReceiveNext();
            mFacingRight = (bool)stream.ReceiveNext();
            mAccuratePosition = (Vector3)stream.ReceiveNext();
        }
    }

    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, mAccuratePosition, Time.deltaTime * 10);
        }
    }
}
