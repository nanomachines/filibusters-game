﻿using UnityEngine;
using UnityEngine.SceneManagement;
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
        private AudioClip mEquipDarkhorse;
        [SerializeField]
        private AudioClip mEquipVeto;
        [SerializeField]
        private AudioClip mEquipMagicBullet;
        [SerializeField]
        private AudioClip mEquipAnarchy;
        [SerializeField]
        private AudioClip mEquipLibelAndSlander;
        [SerializeField]
        private AudioClip mUseDarkhorse;
        [SerializeField]
        private AudioClip mUseVeto;
        [SerializeField]
        private AudioClip mUseMagicBullet;
        [SerializeField]
        private AudioClip mUseAnarchy;
        [SerializeField]
        private AudioClip mUseLibelAndSlander;
        [SerializeField]
        private AudioClip mMisfireDarkhorse;
        [SerializeField]
        private AudioClip mMisfireVeto;
        [SerializeField]
        private AudioClip mMisfireMagicBullet;
        [SerializeField]
        private AudioClip mMisfireAnarchy;
        [SerializeField]
        private AudioClip mMisfireLibelAndSlander;

        // Use this for initialization
        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                RegisterEvents();
                mSource = GetComponent<AudioSource>();
                DontDestroyOnLoad(this);

                SceneManager.sceneLoaded += (Scene s, LoadSceneMode lsm) =>
                {
                    if (s.name == Scenes.MAIN)
                    {
                        mSource.Play();
                    }
                };
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

            EventSystem.OnCoinDepositedEvent += (int ownerId, int newDepositBalance) =>
            {
                mSource.PlayOneShot(mCoinDeposited);
            };

            EventSystem.OnEquipWeaponEvent += (int actorId, WeaponId weaponId) =>
            {
                if (actorId == PhotonNetwork.player.ID)
                {
                    AudioClip pickup = null;
                    switch (weaponId)
                    {
                        case WeaponId.FISTS:
                            pickup = mEquipDarkhorse;
                            break;
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

            EventSystem.OnWeaponFiredEvent += (int actorId, WeaponId weaponId) =>
            {
                if (actorId == PhotonNetwork.player.ID)
                {
                    AudioClip clip = null;
                    float volume = 1.0f;
                    switch (weaponId)
                    {
                        case WeaponId.FISTS:
                            clip = mUseDarkhorse;
                            break;
                        case WeaponId.VETO:
                            clip = mUseVeto;
                            volume = 0.5f;
                            break;
                        case WeaponId.MAGIC_BULLET:
                            clip = mUseMagicBullet;
                            break;
                        case WeaponId.ANARCHY:
                            clip = mUseAnarchy;
                            break;
                        case WeaponId.LIBEL_AND_SLANDER:
                            clip = mUseLibelAndSlander;
                            break;
                    }
                    mSource.PlayOneShot(clip, volume);
                }
            };

            EventSystem.OnWeaponMisfiredEvent += (int actorId, GameConstants.WeaponId weaponId) =>
            {
                if (actorId == PhotonNetwork.player.ID)
                {
                    AudioClip misfire = null;
                    switch (weaponId)
                    {
                        case WeaponId.FISTS:
                            misfire = mMisfireDarkhorse;
                            break;
                        case WeaponId.VETO:
                            misfire = mMisfireVeto;
                            break;
                        case WeaponId.MAGIC_BULLET:
                            misfire = mMisfireMagicBullet;
                            break;
                        case WeaponId.ANARCHY:
                            misfire = mMisfireAnarchy;
                            break;
                        case WeaponId.LIBEL_AND_SLANDER:
                            misfire = mMisfireLibelAndSlander;
                            break;
                    }
                    mSource.PlayOneShot(misfire);
                }
            };
        }
    }
}
