using UnityEngine;
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
        // TODO: delete once we get the event system up and running
		public DepositManager mDepositManager { private get; set; }

		// Use this for initialization
		void Start()
		{
			mIsDead = false;
			mPhotonView = GetComponent<PhotonView>();
			mRenderer = GetComponent<SpriteRenderer>();
			mCollider = GetComponent<BoxCollider2D>();
			mPhysics = GetComponent<SimplePhysics>();
			mIsLocalPlayer = GetComponent<PhotonView>().ownerId == PhotonNetwork.player.ID;
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
            Vector3 respawnPos = SpawnManager.Instance.GetRandomSpawnPoint();
            mPhotonView.RPC("Respawn", PhotonTargets.All, respawnPos);
        }

        [PunRPC]
        public void Respawn(Vector3 respawnPos)
        {
            mIsDead = false;
            mPhysics.enabled = mIsLocalPlayer;
			mPhysics.ResetPhysicsState(respawnPos);
            mRenderer.enabled = true;
            mCollider.enabled = true;
        }
	}
}
