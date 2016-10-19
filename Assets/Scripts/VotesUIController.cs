using UnityEngine;
using System.Collections;
using Photon;

namespace Filibusters
{
	public class VotesUIController : PunBehaviour
	{
        [SerializeField]
        GameObject[] mVoteUIElements;

		// Use this for initialization
		void Start()
		{
            var activePlayers = NetworkManager.GetActivePlayerNumbers();
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                mVoteUIElements[i].SetActive(activePlayers[i]);
            }
		}
		
		// Update is called once per frame
		void Update()
		{
		}

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            int playerNumberToFree = NetworkManager.GetPlayerNumber(otherPlayer);
            mVoteUIElements[playerNumberToFree].SetActive(false);
        }
	}
}
