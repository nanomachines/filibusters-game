using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class LifeManager : MonoBehaviour {

		[SerializeField]
		private float SecondsToRespawn;
		private bool mIsDead;
		private PhotonView mPhotonView;
        private NetPlayerAnimationController mAnimController;
		private BoxCollider2D mCollider;
		private SimplePhysics mPhysics;
        private PlayerAttack mAttackScript;
        private WeaponInventory mWeaponInventory;
		private bool mIsLocalPlayer;

		// Use this for initialization
		void Start()
		{
			mIsDead = false;
			mPhotonView = GetComponent<PhotonView>();
			mAnimController = GetComponent<NetPlayerAnimationController>();
			mCollider = GetComponent<BoxCollider2D>();
			mPhysics = GetComponent<SimplePhysics>();
            mAttackScript = GetComponent<PlayerAttack>();
            mWeaponInventory = GetComponent<WeaponInventory>();
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
            EventSystem.OnDeath(GetComponent<PhotonView>().viewID);
			Despawn();
			if (PhotonNetwork.isMasterClient)
			{
				StartCoroutine(RespawnTimer());
			}
		}

        private void Despawn()
        {
            mAnimController.SetRenderersEnabled(false);
            mCollider.enabled = false;
            mPhysics.enabled = false;
            mAttackScript.enabled = false;
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
            mAnimController.SetRenderersEnabled(true);
            mAttackScript.enabled = true;
            mCollider.enabled = true;
            mWeaponInventory.EquipWeapon(GameConstants.WeaponId.FISTS);
        }
	}
}
