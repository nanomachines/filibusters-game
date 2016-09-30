using UnityEngine;
using System.Collections;

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
                Destroy(this);
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
        }
    }
}
