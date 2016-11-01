using UnityEngine;
using Photon;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

namespace Filibusters
{
    public class CharacterSelectManager : PunBehaviour
    {
        public static readonly string IS_READY_KEY = "IsReady";
        public static readonly string IS_NEW_KEY = "IsNew";

        private bool mAllPlayersReady = false;

        public GameObject[] mReadyRooms;

        private int mPlayersReady;

        // Use this for initialization
        void Start()
        {
            // alert the other players in the room that a new player has joined that is not ready to start
            PhotonNetwork.SetPlayerCustomProperties(new PhotonHashtable { { IS_READY_KEY, false }, { IS_NEW_KEY, true } });
            if (PhotonNetwork.isMasterClient)
            {
                NetworkManager.Instance.OnPhotonPlayerConnected(PhotonNetwork.player);
            }

            // If we are in offline mode we need to explicitly call the properties changed callback
            if (PhotonNetwork.offlineMode)
            {
                OfflineReadyUpdate(false, true);
            }

            // initialize the counter for number of players ready
            mPlayersReady = 0;
            foreach (var player in PhotonNetwork.playerList)
            {
                bool isReady = player.customProperties.ContainsKey(IS_READY_KEY) ?
                    (bool)player.customProperties[IS_READY_KEY] : false;

                if (isReady)
                {
                    ++mPlayersReady;
                }
            }
        }

        void Update()
        {
            if (!mAllPlayersReady && PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length == mPlayersReady)
            {
                mAllPlayersReady = true;
                EventSystem.OnAllPlayersReady();
            }
            else if (mAllPlayersReady && PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length != mPlayersReady)
            {
                mAllPlayersReady = false;
                EventSystem.OnAllPlayersNotReady();
            }
        }

        public void OnStartGame()
        {
            GetComponent<PhotonView>().RPC("LaunchGame", PhotonTargets.All);
        }

        [PunRPC]
        public void LaunchGame()
        {
            PhotonNetwork.LoadLevel(Scenes.MAIN);
        }
    
        private void MarkLocalPlayerReadyState(bool isReady)
        {
            PhotonNetwork.SetPlayerCustomProperties(
                new ExitGames.Client.Photon.Hashtable{ { IS_READY_KEY, isReady}, {IS_NEW_KEY, false} });
    
            // If we are in offline mode we need to explicitly call the properties changed callback
            if (PhotonNetwork.offlineMode)
            {
                OfflineReadyUpdate(isReady, false);
            }
        }

        public override void OnPhotonCustomRoomPropertiesChanged(PhotonHashtable propertiesThatChanged)
        {
            if (NetworkManager.HasPlayerNumberPropertyChangedInRoom(propertiesThatChanged))
            {
                var activePlayers = NetworkManager.GetActivePlayerNumbers();
                for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
                {
                    Utility.GetChildWithTag(mReadyRooms[i], Tags.INACTIVE_OVERLAY).SetActive(!activePlayers[i]);
                }
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            if (PhotonNetwork.isMasterClient)
            {
                int playerNumberToFree = NetworkManager.GetPlayerNumber(otherPlayer);
                mReadyRooms[playerNumberToFree].GetComponentInChildren<CoinSpawner>().RespawnOverNetwork();
            }
        }

        public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            var player = playerAndUpdatedProps[0] as PhotonPlayer;
            var properties = playerAndUpdatedProps[1] as PhotonHashtable;

            // if a player has marked themselves as ready then update the count
            if (properties.ContainsKey(IS_READY_KEY) && (bool)properties[IS_READY_KEY])
            {
                ++mPlayersReady;
            }

            // if a player has marked themselves as not ready and they are not just joining
            // the room, decrement their player count
            else if (properties.ContainsKey(IS_NEW_KEY) && !(bool)properties[IS_NEW_KEY])
            {
                --mPlayersReady;
            }

            // when a player is allocated a player number, locally spawn the player in the
            // appropriate ready room
            if (player.isLocal && NetworkManager.HasPlayerNumberPropertyChangedForPlayer(properties))
            {
                var localPlayerNum = NetworkManager.LocalPlayerNumber;
                var localPlayer = PhotonNetwork.Instantiate(Utility.PlayerNumberToPrefab(localPlayerNum),
                    Utility.GetChildWithTag(mReadyRooms[localPlayerNum], Tags.RESPAWN).transform.position,
                    Quaternion.identity, 0);
                localPlayer.GetComponent<SimplePhysics>().enabled = true;
                localPlayer.GetComponent<AimingController>().enabled = true;
                var mLocalDepositManager = mReadyRooms[localPlayerNum].GetComponentInChildren<DepositManager>();
                mLocalDepositManager.LocalDepositEvent += () => { MarkLocalPlayerReadyState(true);  };
                InputWrapper.Instance.mLocalReadyRoomCharacter = localPlayer;
            }
        }

        public override void OnJoinedRoom()
        {
            InitRoomsFromNetworkPlayerAllocator();
        }

        private void OfflineReadyUpdate(bool isReady, bool isNew)
        {
            object[] playerAndUpdatedProps = new object[2];
            playerAndUpdatedProps[0] = PhotonNetwork.player;
            playerAndUpdatedProps[1] = new PhotonHashtable { { IS_READY_KEY, isReady }, {IS_NEW_KEY, isNew} };
            OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
        }

        private void InitRoomsFromNetworkPlayerAllocator()
        {
            var activePlayers = NetworkManager.GetActivePlayerNumbers();
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                Utility.GetChildWithTag(mReadyRooms[i], Tags.INACTIVE_OVERLAY).SetActive(!activePlayers[i]);
            }
        }

    }
}
