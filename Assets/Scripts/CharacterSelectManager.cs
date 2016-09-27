using UnityEngine;
using System.Collections;
using Photon;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.Assertions;
using ExitGames.Client.Photon;

namespace Filibusters
{
    public class CharacterSelectManager : PunBehaviour
    {
        public static readonly string PLAYER_NUMBER_KEY = "PlayerNumber";
        public static readonly string PLAYER_ACTIVE_KEY = "PlayerNumberActive";
        public static readonly string IS_READY_KEY = "IsReady";
        public static readonly string IS_NEW_KEY = "IsNew";

        [HideInInspector]
        public static CharacterSelectManager instance = null;

        public GameObject[] mReadyRooms;

        private int mPlayersReady;
        private BitArray mPlayerNumberAllocator;

        // Use this for initialization
        void Start()
        {
            if (instance == null)
            {
                instance = this;
                mPlayerNumberAllocator = new BitArray(GameConstants.MAX_ONLINE_PLAYERS_IN_GAME);
                PhotonNetwork.SetPlayerCustomProperties(new PhotonHashtable { { IS_READY_KEY, false }, { IS_NEW_KEY, true } });
                if (PhotonNetwork.isMasterClient)
                {
                    ResetActivePlayers();
                    OnPhotonPlayerConnected(PhotonNetwork.player);
                }
                else
                {
                    ResetPlayerNumberAllocatorFromRoomSettings();
                    ResetRooms();
                }
                // If we are in offline mode we need to explicitly call the properties changed callback
                if (PhotonNetwork.offlineMode)
                {
                    OfflineReadyUpdate(false, true);
                }
    
                mPlayersReady = 0;
                foreach (var player in PhotonNetwork.playerList)
                {
                    bool isReady = player.customProperties.ContainsKey(IS_READY_KEY) ? (bool)player.customProperties[IS_READY_KEY] : false;
                    if (isReady)
                    {
                        ++mPlayersReady;
                    }
                }
            }
        }

        void OnGUI()
        {
            var labelBuilder = new System.Text.StringBuilder(90);
            if (PhotonNetwork.isMasterClient)
            {
                labelBuilder.AppendLine("Host/Master Client");
            }
            labelBuilder.AppendLine("PlayersReady: " + mPlayersReady);
            foreach (var player in PhotonNetwork.playerList)
            {
                var isReady = player.customProperties.ContainsKey(IS_READY_KEY) ? player.customProperties[IS_READY_KEY] : false;
                labelBuilder.AppendLine(player.ID + ": " + isReady);
            }
            GUILayout.Label(labelBuilder.ToString());

            if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length == mPlayersReady)
            {
                if (GUILayout.Button("Start Game"))
                {
                    OnStartGame();
                }
            }
        }

        public void OnStartGame()
        {
            GetComponent<PhotonView>().RPC("LaunchGame", PhotonTargets.All);
//            NetworkManager.Instance.CloseGameSession();
        }

        [PunRPC]
        public void LaunchGame()
        {
            PhotonNetwork.LoadLevel("Scenes/MainGame");
        }
    
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

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            if (PhotonNetwork.isMasterClient)
            {
                int nextOpenPlayerNumber = FindNextPlayerNumber(mPlayerNumberAllocator);
                Assert.IsTrue(nextOpenPlayerNumber != -1);
                mPlayerNumberAllocator.Set(nextOpenPlayerNumber, true);
                newPlayer.SetCustomProperties(new PhotonHashtable{ { PLAYER_NUMBER_KEY, nextOpenPlayerNumber } });
                PhotonNetwork.room.SetCustomProperties(
                    new PhotonHashtable { { PLAYER_NUMBER_KEY + nextOpenPlayerNumber, true } });
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            if (PhotonNetwork.isMasterClient)
            {
                int playerNumberToFree = (int)otherPlayer.customProperties[PLAYER_NUMBER_KEY];
                mPlayerNumberAllocator.Set(playerNumberToFree, false);
                PhotonNetwork.room.SetCustomProperties(
                    new PhotonHashtable { { PLAYER_NUMBER_KEY + playerNumberToFree, false} });
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(SerializeBitArray(mPlayerNumberAllocator));
            }
            else
            {
                mPlayerNumberAllocator = DeserializeBitArray((string)stream.ReceiveNext());
            }
        }

        public override void OnPhotonCustomRoomPropertiesChanged(PhotonHashtable propertiesThatChanged)
        {
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                if (propertiesThatChanged.ContainsKey(PLAYER_NUMBER_KEY + i))
                {
                    var playerIsActive = (bool)propertiesThatChanged[PLAYER_NUMBER_KEY + i];
                    GetChildWithTag(mReadyRooms[i], "InactiveIndicator").SetActive(!playerIsActive);
                }
            }
        }

        public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            var properties = playerAndUpdatedProps[1] as PhotonHashtable;
            if (properties.ContainsKey(IS_READY_KEY) && (bool)properties[IS_READY_KEY])
            {
                ++mPlayersReady;
            }
            else if (properties.ContainsKey(IS_NEW_KEY) && !(bool)properties[IS_NEW_KEY])
            {
                --mPlayersReady;
            }
        }
    
        private void OfflineReadyUpdate(bool isReady, bool isNew)
        {
            object[] playerAndUpdatedProps = new object[2];
            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add(IS_READY_KEY, isReady);
            properties.Add(IS_NEW_KEY, isNew);
            playerAndUpdatedProps[1] = properties;
            OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
        }

        private void ResetActivePlayers()
        {
            var resetTable = new PhotonHashtable();
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                resetTable.Add(PLAYER_NUMBER_KEY + i, false);
            }
            PhotonNetwork.room.SetCustomProperties(resetTable);
        }
        
        private void ResetPlayerNumberAllocatorFromRoomSettings()
        {
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                mPlayerNumberAllocator[i] = (bool)PhotonNetwork.room.customProperties[PLAYER_NUMBER_KEY + i];
            }
        }

        private void ResetRooms()
        {
            for (int i = 0; i < mPlayerNumberAllocator.Length; ++i)
            {
                GetChildWithTag(mReadyRooms[i], "InactiveIndicator").SetActive(!mPlayerNumberAllocator[i]);
            }
        }

        private int FindNextPlayerNumber(BitArray playerNumberMap)
        {
            for (int i = 0; i < playerNumberMap.Length; ++i)
            {
                if (!playerNumberMap.Get(i))
                {
                    return i;
                }
            }
            return -1;
        }

        private GameObject GetChildWithTag(GameObject parent, string tag)
        {
            foreach (Transform childTransform in parent.transform)
            {
                if (childTransform.gameObject.tag.Equals(tag))
                {
                    return childTransform.gameObject;
                }
            }
            return null;
        }

        private string SerializeBitArray(BitArray bitArray)
        {
            var strBuilder = new System.Text.StringBuilder(bitArray.Length);
            for (int i = 0; i < bitArray.Length; ++i)
            {
                strBuilder.Append(bitArray[i] ? "1" : "0");
            }
            return strBuilder.ToString();
        }

        private BitArray DeserializeBitArray(string bitStr)
        {
            var bitArray = new BitArray(bitStr.Length);
            for (int i = 0; i < bitStr.Length; ++i)
            {
                bitArray[i] = bitStr[i] == '1';
            }
            return bitArray;
        }
    }
}
