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
        private GameObject mPlayerUI;

        private PlayerState mPlayerState;
        private int mMaxHealth;
        private int mCurHealth;

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
            mPlayerUI = Utility.GetChildWithTag(gameObject, Tags.PLAYER_UI);
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
            if (mPhotonView.isMine)
            {
                EventSystem.OnUpdateHealthBar(mCurHealth);
                mPhotonView.RPC("OnDamaged", PhotonTargets.All);
            }
            if (mCurHealth == 0)
            {
                Die();
            }
        }


        [PunRPC]
        public void OnDamaged()
        {
            EventSystem.OnPlayerHit(GetComponent<PhotonView>().viewID);
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
            if (PhotonNetwork.isMasterClient)
            {
                StartCoroutine(RespawnTimer());
            }
        }

        private void Despawn()
        {
            EventSystem.OnDeath(GetComponent<PhotonView>().viewID);
            mAnimController.SetRenderersEnabled(false);
            mCollider.enabled = false;
            mPhysics.enabled = false;
            mAttackScript.enabled = false;
            mPlayerUI.SetActive(false);
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
            mWeaponInventory.ResetWeapon();
            mCurHealth = mMaxHealth;

            if (mIsLocalPlayer)
            {
                mPlayerUI.SetActive(true);
                EventSystem.OnUpdateHealthBar(mCurHealth);
            }
        }
    }
}
