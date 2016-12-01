using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class AudioManager : MonoBehaviour
    {
        enum PhotonEvents { FIRE_EVT = 0 };

        public static AudioManager Instance = null;
        private AudioSource mSource;

        [SerializeField]
        private AudioClip mMainGameBackgroundMusic;
        [SerializeField]
        private AudioClip mMenuMusic;
        [SerializeField]
        private AudioClip mYouLoseMusic;
        [SerializeField]
        private AudioClip mYouWinMusic;

        private float mTime = 0f;
        private bool mGameOver = false;

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

        public AudioClip[] mMaleGrunts;
        public AudioClip[] mFemaleGrunts;

        public AudioClip[] mLeadChangeCalls;
        public AudioClip[] mTrumpingCalls;
        public AudioClip[] mAboutToWinCalls;
        private int mLeadingPlayer = -1;
        private static readonly int TRUMPING_CALL_LIMIT = (int)(GameConstants.AMOUNT_OF_COINS_TO_WIN * .75f);

        // Use this for initialization
        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                RegisterEvents();
                mSource = GetComponent<AudioSource>();
                mSource.loop = true;
                DontDestroyOnLoad(this);

                // initialize scene background music
                PlayBackgroundMusicForScene(SceneManager.GetActiveScene());
                // when scene changes, update background music
                SceneManager.sceneLoaded += (Scene s, LoadSceneMode lsm) => { PlayBackgroundMusicForScene(s); };
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void PlayBackgroundMusicForScene(Scene s)
        {
            mGameOver = false;
            mSource.loop = true;
            if (Utility.AreSceneNamesEqual(s.name, Scenes.MAIN))
            {
                StartCoroutine(FadeAndPlayMusic(mMainGameBackgroundMusic));
            }
            else if (mSource.clip != mMenuMusic)
            {
                StartCoroutine(FadeAndPlayMusic(mMenuMusic));
            }
        }

        private IEnumerator FadeAndPlayMusic(AudioClip clip)
        {
            mTime = 0f;
            while (mSource.volume > 0f)
            {
                yield return new WaitForFixedUpdate();
                mTime += Time.fixedDeltaTime;
                float volume = Mathf.Lerp(1f, 0f, mTime);
                mSource.volume = volume;
            }
            mSource.volume = 1f;
            mSource.clip = clip;
            mSource.Play();
        }

        private void RegisterEvents()
        {
            EventSystem.OnDeathEvent += (int playerViewId, Vector3 pos) =>
            {
                AudioSource.PlayClipAtPoint(mPlayerDeath, pos);
            };

            EventSystem.OnJumpEvent += (Vector3 pos) =>
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

            EventSystem.OnCoinDepositedEvent += (int ownerId, int newDepositBalance, Vector3 pos) =>
            {
                AudioSource.PlayClipAtPoint(mCoinDeposited, pos);
                int playerNumber = NetworkManager.GetPlayerNumber(PhotonPlayer.Find(ownerId));
                if (newDepositBalance == GameConstants.AMOUNT_OF_COINS_TO_WIN - 1)
                {
                    mSource.PlayOneShot(mAboutToWinCalls[playerNumber]);
                }
            };

            EventSystem.OnEquipWeaponEvent += (int actorId, WeaponId weaponId) =>
            {
                if (actorId == PhotonNetwork.player.ID)
                {
                    AudioClip pickup = null;
                    switch (weaponId)
                    {
                        case WeaponId.DARK_HORSE:
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

            EventSystem.OnWeaponFiredEvent += (WeaponId weaponId, Vector3 pos) =>
            {
                object[] content = new object[] { weaponId, pos };
                PhotonNetwork.RaiseEvent((byte)PhotonEvents.FIRE_EVT, content, false, null);
                WeaponFireCallback(weaponId, pos);
            };

            EventSystem.OnWeaponMisfiredEvent += (WeaponId weaponId) =>
            {
                AudioClip misfire = null;
                switch (weaponId)
                {
                    case WeaponId.DARK_HORSE:
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
                mSource.PlayOneShot(misfire, 0.5f);
            };

            PhotonNetwork.OnEventCall += (byte evtCode, object contents, int senderId) =>
            {
                object[] objects = (object[])contents;
                var weaponId = (WeaponId)objects[0];
                var pos = (Vector3)objects[1];
                if ((PhotonEvents)evtCode == PhotonEvents.FIRE_EVT)
                {
                    WeaponFireCallback(weaponId, pos);
                }
            };

            EventSystem.OnPlayerHitEvent += (int playerViewId, Vector3 pos) =>
            {
                int playerNumber = NetworkManager.GetPlayerNumber(PhotonView.Find(playerViewId).owner);
                AudioClip grunt = null;
                switch (playerNumber)
                {
                    case 0:
                    case 2:
                        grunt = mFemaleGrunts[Random.Range(0, mFemaleGrunts.Length)];
                        break;
                    case 1:
                    case 3:
                        grunt = mMaleGrunts[Random.Range(0, mMaleGrunts.Length)];
                        break;
                }
                AudioSource.PlayClipAtPoint(grunt, pos);
            };

            EventSystem.OnGameOverJiggleEvent += (bool isWinner) =>
            {
                if (!mGameOver)
                {
                    mGameOver = true;
                    mSource.loop = false;
                    AudioClip clip = mYouLoseMusic;
                    if (isWinner)
                    {
                        clip = mYouWinMusic;
                    }
                    StartCoroutine(FadeAndPlayMusic(clip));
                }
            };

            EventSystem.OnLeadingPlayerUpdatedEvent += (int leadingPlayer) =>
            {
                if (leadingPlayer != mLeadingPlayer)
                {
                    mLeadingPlayer = leadingPlayer;
                    if (Random.Range(0f, 1f) < .5f)
                    {
                        mSource.PlayOneShot(mLeadChangeCalls[leadingPlayer]);
                    }
                    else
                    {
                        mSource.PlayOneShot(mTrumpingCalls[leadingPlayer]);
                    }
                }
            };
        }

        void WeaponFireCallback(WeaponId weaponId, Vector3 pos)
        {
            AudioClip clip = null;
            switch (weaponId)
            {
                case WeaponId.DARK_HORSE:
                    clip = mUseDarkhorse;
                    break;
                case WeaponId.VETO:
                    clip = mUseVeto;
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
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }
}
