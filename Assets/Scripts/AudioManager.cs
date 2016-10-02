using UnityEngine;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance = null;
        private AudioSource mSource;

        [SerializeField]
        private AudioClip mPlayerDeath;
        [SerializeField]
        private AudioClip mPlayerJump;
        [SerializeField]
        private AudioClip mCoinCollection;
        [SerializeField]
        private AudioClip mCoinDeposited;
        [SerializeField]
        private AudioClip mEquipVeto;
        [SerializeField]
        private AudioClip mEquipMagicBullet;
        [SerializeField]
        private AudioClip mEquipAnarchy;
        [SerializeField]
        private AudioClip mEquipLibelAndSlander;

        // Use this for initialization
        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                RegisterEvents();
                mSource = GetComponent<AudioSource>();
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void RegisterEvents()
        {
            EventSystem.OnDeathEvent += (int playerViewId) =>
            {
                mSource.PlayOneShot(mPlayerDeath);
            };

            EventSystem.OnJumpEvent += () =>
            {
                mSource.PlayOneShot(mPlayerJump);
            };

            EventSystem.OnCoinCollectedEvent += (int actorId) =>
            {
                if (actorId == PhotonNetwork.player.ID)
                {
                    mSource.PlayOneShot(mCoinCollection);
                }
            };

            EventSystem.OnCoinDepositedEvent += (Vector3 depositBoxPos) =>
            {
                // TODO: Play as 3D sound
                mSource.PlayOneShot(mCoinDeposited);
            };

            EventSystem.OnEquipWeaponEvent += (int actorId, WeaponId weaponId) =>
            {
                if (actorId == PhotonNetwork.player.ID)
                {
                    AudioClip pickup = null;
                    switch (weaponId)
                    {
                        case WeaponId.VETO:
                            pickup = mEquipVeto;
                            break;
                        case WeaponId.MAGIC_BULLET:
                            pickup = mEquipMagicBullet;
                            break;
                        case WeaponId.ANARCHY:
                            pickup = mEquipAnarchy;
                            break;
                        case WeaponId.LIBEL_AND_SLANDER:
                            pickup = mEquipLibelAndSlander;
                            break;
                    }
                    mSource.PlayOneShot(pickup);
                }
            };
        }
    }
}
