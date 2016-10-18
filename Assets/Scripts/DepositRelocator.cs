using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class DepositRelocator : MonoBehaviour
	{
        [SerializeField] 
        private float MoveTime;

        private PhotonView mPhotonView;
        private GameObject[] mLocations;
        private bool mTimerOver = true;

		void Start()
		{
            mPhotonView = GetComponent<PhotonView>();
            mLocations = GameObject.FindGameObjectsWithTag(Tags.DEPOSIT_LOCATION);
		}

        void Update()
        {
            if (mTimerOver)
            {
                StartCoroutine(MoveTimer());
                mTimerOver = false;
            }
        }

		IEnumerator MoveTimer()
		{
            // every client runs the move timer so that if the master client drops out
            // the timer continues to run seamlessly
            yield return new WaitForSeconds(MoveTime);
            mTimerOver = true;
            // only move the deposit box once from the master client to keep the movement
            // authoritative and consistent
            if (PhotonNetwork.isMasterClient)
            {
                Vector3 nextPos = mLocations[Random.Range(0, mLocations.Length)].transform.position;
                mPhotonView.RPC("MoveDeposit", PhotonTargets.All, nextPos); 
            }
		}

        [PunRPC]
        void MoveDeposit(Vector3 newPos)
        {
            transform.position = newPos;
        }
	}
}
