using UnityEngine;
using UnityEngine.Assertions;
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

        private PlayerState mPlayerState;
        private int mMaxHealth;
        private int mCurHealth
        {
            get { return mPlayerState.mCurHealth; }
            set { mPlayerState.mCurHealth = value; }
        }

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

            mPlayerState = GetComponent<PlayerState>();
            mMaxHealth = GameConstants.MAX_PLAYER_HEALTH;
            mCurHealth = mMaxHealth;
		}
		
		public void Die()
		{
            Despawn();
			mPhotonView.RPC("OnDeath", PhotonTargets.MasterClient);
		}

        public void InflictDamage(int damageAmount)
        {
            Assert.IsTrue(damageAmount >= 0);
            mCurHealth = Mathf.Max(0, mCurHealth - damageAmount);

            mPhotonView.RPC("UpdateLocalHealthBar", PhotonTargets.All, mCurHealth);

            if (mCurHealth == 0)
            {
                Die();
            }
        }

        [PunRPC]
        public void UpdateLocalHealthBar(int health)
        {
            if (mIsLocalPlayer)
            {
                EventSystem.OnUpdateHealthBar(health);
            }
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
            mIsDead = true;
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

            if (mIsLocalPlayer)
            {
                mCurHealth = mMaxHealth;
                EventSystem.OnUpdateHealthBar(mCurHealth);
            }
        }
    }
}
