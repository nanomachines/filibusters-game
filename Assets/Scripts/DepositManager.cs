using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Filibusters
{
	public class DepositManager : Photon.PunBehaviour 
	{
        // Used to know when a specific deposit box has a coin deposited into it
        // This is useful for events such as readying up in the tutorial room
        // and running visual effects specific to the deposit box that was deposited into
        public delegate void DepositListener();
        public event DepositListener LocalDepositEvent;

		[SerializeField]
		private bool mDepositing;
		private HashSet<int> mPlayersInZone;
		private Dictionary<int, int> mPlayerDepositCounts;
		private Dictionary<int, int> mActorIdtoViewId;
		private int mNumPlayersInZone;
		[SerializeField]
		private float mDepositTime;
		[SerializeField]
		private float mTimeSinceDeposit;

		private PhotonView mPhotonView;

		void Start()
		{
            EventSystem.OnDeathEvent += ClearDeadPlayer;
			mPlayersInZone = new HashSet<int>();
			mPlayerDepositCounts = new Dictionary<int, int>();
			mNumPlayersInZone = 0;
			mTimeSinceDeposit = 0f;
			mDepositing = false;
			mPhotonView = GetComponent<PhotonView>();
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == Tags.PLAYER)
			{
				// add the player to the zone
				mPlayersInZone.Add(other.gameObject.GetComponent<PhotonView>().viewID);
				++mNumPlayersInZone;

				if (mNumPlayersInZone != 1)
				{
					mTimeSinceDeposit = 0f;
				}
			}
		}

		void OnTriggerExit2D(Collider2D other)
		{
			// Put this inside OnExitZone
			if (other.tag == Tags.PLAYER)
			{
				OnZoneExit(other.gameObject.GetComponent<PhotonView>().viewID);
			}
		}


		void Update()
		{
			if (PhotonNetwork.isMasterClient && mNumPlayersInZone == 1)
			{
				mDepositing = true;
				mTimeSinceDeposit += Time.deltaTime;
				if (mTimeSinceDeposit >= mDepositTime)
				{
					// deposit player coin
					var itr = mPlayersInZone.GetEnumerator();
					itr.MoveNext();
					int viewId = itr.Current;
					mPhotonView.RPC("DepositCoin", PhotonTargets.All, viewId);
					// reset the time
					mTimeSinceDeposit = 0f;
				}
			}
			else
			{
				mDepositing = false;
			}
		}

		[PunRPC]
		public void DepositCoin(int viewId)
		{
			if (!mPlayerDepositCounts.ContainsKey(viewId))
			{
				mPlayerDepositCounts.Add(viewId, 0);
			}

			if (PhotonView.Find(viewId).gameObject.GetComponent<CoinInventory>().DepositCoin())
			{
                EventSystem.OnCoinDeposited(transform.position);
				int newDepositBalance = ++mPlayerDepositCounts[viewId];
                if (LocalDepositEvent != null)
                {
                    LocalDepositEvent();
                }
				if (PhotonNetwork.isMasterClient &&
                    newDepositBalance >= GameConstants.AMOUNT_OF_COINS_TO_WIN)
				{
					Debug.Log("GAME OVER: " + PhotonView.Find(viewId).owner);
                    EventSystem.OnGameOver(PhotonView.Find(viewId).owner.ID);
				}
			}
		}

		public void ClearDeadPlayer(int viewId)
		{
			if (mPlayersInZone.Contains(viewId))
			{
				OnZoneExit(viewId);
			}
		}

		public void OnZoneExit(int viewId)
		{
			// remove player from deposit zone
			mPlayersInZone.Remove(viewId);
			--mNumPlayersInZone;

			// eset deposit timer if more or less than one player in zone
			if (mNumPlayersInZone != 1)
			{
				mTimeSinceDeposit = 0f;
			}
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer p)
		{
			foreach (var viewId in mPlayersInZone)
			{
				if (PhotonView.Find(viewId).owner == null)
				{
					OnZoneExit(viewId);
					break;
				}
			}
		}

	}
}

