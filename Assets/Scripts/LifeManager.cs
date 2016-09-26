﻿using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class LifeManager : MonoBehaviour {

		[SerializeField]
		private float SecondsToRespawn;
		private bool mIsDead;
		private PhotonView mPhotonView;
		private SpriteRenderer mRenderer;
		private BoxCollider2D mCollider;
		private SimplePhysics mPhysics;
		private bool mIsLocalPlayer;

		// TODO: replace with event system
		private DepositManager mDepositManager;

		// Use this for initialization
		void Start()
		{
			mIsDead = false;
			mPhotonView = GetComponent<PhotonView>();
			mRenderer = GetComponent<SpriteRenderer>();
			mCollider = GetComponent<BoxCollider2D>();
			mPhysics = GetComponent<SimplePhysics>();
			mIsLocalPlayer = GetComponent<PhotonView>().ownerId == PhotonNetwork.player.ID;
			// TODO: Replace with event system
			mDepositManager = GameObject.Find("DepositBox").GetComponent<DepositManager>();
		}
		
		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.P) && GetComponent<PhotonView>().owner.ID == PhotonNetwork.player.ID)
			{
				Die();
			}
		}

		public void Die()
		{
			mPhotonView.RPC("OnDeath", PhotonTargets.MasterClient);
		}

		[PunRPC]
		public void OnDeath()
		{
			if (!mIsDead)
			{
				mIsDead = true;
				Debug.Log("Master confirmed Player dead");
				mPhotonView.RPC("OnPlayerDeathVerified", PhotonTargets.All);
			}
		}


		[PunRPC]
		public void OnPlayerDeathVerified()
		{
			Debug.Log("Death Verified");
			mDepositManager.OnDeath(GetComponent<PhotonView>().viewID);
			Despawn();
			if (PhotonNetwork.isMasterClient)
			{
				StartCoroutine(RespawnTimer());
			}

		}

        private void Despawn()
        {
            mRenderer.enabled = false;
            mCollider.enabled = false;
            mPhysics.enabled = false;
            mIsDead = true;
        }

        private IEnumerator RespawnTimer()
        {
            yield return new WaitForSeconds(SecondsToRespawn);
            mPhotonView.RPC("Respawn", PhotonTargets.All);
        }

        [PunRPC]
        public void Respawn()
        {
            transform.position = SpawnManager.Instance.GetRandomSpawnPoint();
            mIsDead = false;
            mPhysics.enabled = mIsLocalPlayer;
			mPhysics.ResetPhysicsState();
            mRenderer.enabled = true;
            mCollider.enabled = true;
        }
	}
}
