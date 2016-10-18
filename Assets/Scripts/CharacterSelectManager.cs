using UnityEngine;
using Photon;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.Assertions;

namespace Filibusters
{
    public class CharacterSelectManager : PunBehaviour
    {
        public static readonly string PLAYER_NUMBER_KEY = "PlayerNumber";
        public static readonly string PLAYER_ACTIVE_KEY = "PlayerNumberActive";
        public static readonly string IS_READY_KEY = "IsReady";
        public static readonly string IS_NEW_KEY = "IsNew";
        public static readonly string PLAYER_CHARACTER_RESOURCE_NAME = "NetPlayer";

        private bool mAllPlayersReady = false;

        public GameObject[] mReadyRooms;

        private int mPlayersReady;
        private int mLocalPlayerNum;

        // Use this for initialization
        void Start()
        {
            // alert the other players in the room that a new player has joined that is not ready to start
            PhotonNetwork.SetPlayerCustomProperties(new PhotonHashtable { { IS_READY_KEY, false }, { IS_NEW_KEY, true } });
            if (PhotonNetwork.isMasterClient)
            {
                InitPlayerAllocator();
                OnPhotonPlayerConnected(PhotonNetwork.player);
            }
            else
            {
                InitRoomsFromNetworkPlayerAllocator();
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
    
        // TODO: delete this method if we never need toggling
        public void ToggleLocalPlayerReady()
        {
            bool isReady = !(bool)PhotonNetwork.player.customProperties[IS_READY_KEY];
            PhotonNetwork.SetPlayerCustomProperties(
                new ExitGames.Client.Photon.Hashtable{ { IS_READY_KEY, isReady}, {IS_NEW_KEY, false} });
    
            // If we are in offline mode we need to explicitly call the properties changed callback
            if (PhotonNetwork.offlineMode)
            {
                OfflineReadyUpdate(isReady, false);
            }
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

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            // Only allocate Player numbers on the master client to avoid redundancy and ensure consistency
            if (PhotonNetwork.isMasterClient)
            {
                int nextOpenPlayerNumber = FindNextPlayerNumber(PhotonNetwork.room.customProperties);
                Assert.IsTrue(nextOpenPlayerNumber != -1);

                // Set this player's custom properties so that if they leave the room we will have a record
                // of what player number they had
                newPlayer.SetCustomProperties(new PhotonHashtable{ { PLAYER_NUMBER_KEY, nextOpenPlayerNumber } });

                // Alert that the player number is taken so that the room can be activated and no one else
                // will take the same number
                PhotonNetwork.room.SetCustomProperties(
                    new PhotonHashtable { { PLAYER_NUMBER_KEY + nextOpenPlayerNumber, true } });
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            if (PhotonNetwork.isMasterClient)
            {
                int playerNumberToFree = (int)otherPlayer.customProperties[PLAYER_NUMBER_KEY];
                PhotonNetwork.room.SetCustomProperties(
                    new PhotonHashtable { { PLAYER_NUMBER_KEY + playerNumberToFree, false} });
                mReadyRooms[playerNumberToFree].GetComponentInChildren<CoinSpawner>().RespawnOverNetwork();
            }
        }

        public override void OnPhotonCustomRoomPropertiesChanged(PhotonHashtable propertiesThatChanged)
        {
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                // if this player number has be allocated or released, update the appropriate ready room
                if (propertiesThatChanged.ContainsKey(PLAYER_NUMBER_KEY + i))
                {
                    var playerIsActive = (bool)propertiesThatChanged[PLAYER_NUMBER_KEY + i];
                    Utility.GetChildWithTag(mReadyRooms[i], Tags.INACTIVE_OVERLAY).SetActive(!playerIsActive);
                }
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
            if (player.isLocal && properties.ContainsKey(PLAYER_NUMBER_KEY))
            {
                mLocalPlayerNum = (int)PhotonNetwork.player.customProperties[PLAYER_NUMBER_KEY];
                var localPlayer = PhotonNetwork.Instantiate(PLAYER_CHARACTER_RESOURCE_NAME,
                    Utility.GetChildWithTag(mReadyRooms[mLocalPlayerNum], Tags.RESPAWN).transform.position,
                    Quaternion.identity, 0);
                localPlayer.GetComponent<SimplePhysics>().enabled = true;
                localPlayer.GetComponent<AimingController>().enabled = true;
                var mLocalDepositManager = mReadyRooms[mLocalPlayerNum].GetComponentInChildren<DepositManager>();
                mLocalDepositManager.LocalDepositEvent += () => { MarkLocalPlayerReadyState(true);  };
                InputWrapper.Instance.mLocalReadyRoomCharacter = localPlayer;
            }
        }
    
        private void OfflineReadyUpdate(bool isReady, bool isNew)
        {
            object[] playerAndUpdatedProps = new object[2];
            playerAndUpdatedProps[0] = PhotonNetwork.player;
            playerAndUpdatedProps[1] = new PhotonHashtable { { IS_READY_KEY, isReady }, {IS_NEW_KEY, isNew} };
            OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
        }

        private void InitPlayerAllocator()
        {
            var playerNumberAllocator = new PhotonHashtable();
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                playerNumberAllocator.Add(PLAYER_NUMBER_KEY + i, false);
            }
            PhotonNetwork.room.SetCustomProperties(playerNumberAllocator);
        }
        
        private void InitRoomsFromNetworkPlayerAllocator()
        {
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                bool isReadyRoomUnused = !(bool)PhotonNetwork.room.customProperties[PLAYER_NUMBER_KEY + i];
                Utility.GetChildWithTag(mReadyRooms[i], Tags.INACTIVE_OVERLAY).SetActive(isReadyRoomUnused);
            }
        }

        private int FindNextPlayerNumber(PhotonHashtable playerNumberMap)
        {
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                bool playerNumberTaken = (bool)playerNumberMap[PLAYER_NUMBER_KEY + i];
                if (!playerNumberTaken)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
