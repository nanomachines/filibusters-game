using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

namespace Filibusters
{
    public class ReadyScreenNetworkController : PunBehaviour
    {
        private static readonly string IS_READY_KEY = "IsReady";
        private static readonly float COUNTDOWN_TIME = 5f;

        [SerializeField]
        GameObject mUICountdown; //UICountdown
        Text mCountdownText;
        bool mCountdownStarted = false;
        float mTimer = COUNTDOWN_TIME;

        [SerializeField]
        private ToggleVisualReady[] mReadyToggles;
        private ToggleVisualReady mLocalToggle;
        private PhotonView mPhotonView;

        private bool mPlayerReady;
        int mNumReadyPlayers = 0;

        void Start()
        {
            mPhotonView = GetComponent<PhotonView>();
            mCountdownText = mUICountdown.GetComponent<Text>();

            mLocalToggle = null;
            if (PhotonNetwork.isMasterClient)
            {
                NetworkManager.Instance.OnPhotonPlayerConnected(PhotonNetwork.player);
            }
        }

        public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            var properties = (PhotonHashtable)playerAndUpdatedProps[1];
            if (NetworkManager.HasPlayerNumberPropertyChangedForPlayer(properties))
            {
                mLocalToggle = mReadyToggles[NetworkManager.LocalPlayerNumber];
                mLocalToggle.active = true;
            }
        }

        void Update()
        {
            /*
             * If we have been assigned our local toggle,
             * update it based on user input
             */
            if (mLocalToggle != null)
            {
                if (InputWrapper.Instance.SubmitPressed && !mPlayerReady)
                {
                    mLocalToggle.ToggleReadyPlayer(true);
                    mPhotonView.RPC("IncrementReadyCount", PhotonTargets.AllBuffered);
                    PhotonNetwork.SetPlayerCustomProperties(
                        new PhotonHashtable { { IS_READY_KEY, true } });
                    mPlayerReady = true;
                }
                if (InputWrapper.Instance.CancelPressed)
                {
                    if (mPlayerReady)
                    {
                        mLocalToggle.ToggleReadyPlayer(false);
                        mPhotonView.RPC("DecrementReadyCount", PhotonTargets.AllBuffered);
                        PhotonNetwork.SetPlayerCustomProperties(
                            new PhotonHashtable { { IS_READY_KEY, false } });
                        mPlayerReady = false;
                    }

                    else
                    {
                        Utility.BackToStartMenu();
                    }
                }
            }

            /*
             * Manage the countdown timer based on the number of players that have readied up
             */
            if (mNumReadyPlayers == PhotonNetwork.playerList.Length)
            {
                mUICountdown.SetActive(true);
                if (mTimer <= 0)
                {
                    StartGame();
                    return;
                }
                mCountdownText.text = "Game starting in... " + ((int)Mathf.Ceil(mTimer)).ToString();
                mTimer -= Time.deltaTime;
            }
            else
            {
                mUICountdown.SetActive(false);
                mCountdownStarted = false;
                mTimer = COUNTDOWN_TIME;
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            if (PhotonNetwork.isMasterClient)
            {
                var deadToggle = mReadyToggles[NetworkManager.GetPlayerNumber(otherPlayer)];
                deadToggle.active = false;
                if (otherPlayer.customProperties.ContainsKey(IS_READY_KEY)
                    && (bool)otherPlayer.customProperties[IS_READY_KEY])
                {
                    deadToggle.ToggleReadyPlayer(false);
                    // this call should not be buffered because if the player quits unexpectedly
                    // then their buffered IncrementReadyCount call will fall through
                    mPhotonView.RPC("DecrementReadyCount", PhotonTargets.All);
                }
            }
        }


        [PunRPC]
        void IncrementReadyCount()
        {
            ++mNumReadyPlayers;
        }

        [PunRPC]
        void DecrementReadyCount()
        {
            --mNumReadyPlayers;
        }

        public void StartGame()
        {
            GetComponent<PhotonView>().RPC("LaunchGame", PhotonTargets.MasterClient);
        }

        [PunRPC]
        public void LaunchGame()
        {
            PhotonNetwork.LoadLevel(Scenes.MAIN);
            PhotonNetwork.room.visible = false;
            PhotonNetwork.room.open = false;
        }
    }
}

