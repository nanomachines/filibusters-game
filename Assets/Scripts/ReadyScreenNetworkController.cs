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
            }
        }

        void Update()
        {
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
                        PhotonNetwork.LeaveRoom();
                        SceneManager.LoadScene(Scenes.START_MENU);
                    }
                }
            }

            //mUICountdown.GetComponent<UnityEngine.UI.Text>().text = mNumReadyPlayers.ToString();
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
                if (otherPlayer.customProperties.ContainsKey(IS_READY_KEY)
                    && (bool)otherPlayer.customProperties[IS_READY_KEY])
                {
                    // this call should not be buffered because if the player quits unexpectedly
                    // then their buffered IncrementReadyCount call will fall through
                    mPhotonView.RPC("DecrementReadyCount", PhotonTargets.All);
                    mReadyToggles[NetworkManager.GetPlayerNumber(otherPlayer)].ToggleReadyPlayer(false);
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
            GetComponent<PhotonView>().RPC("LaunchGame", PhotonTargets.All);
        }

        [PunRPC]
        public void LaunchGame()
        {
            PhotonNetwork.LoadLevel(Scenes.MAIN);
        }
    }
}

