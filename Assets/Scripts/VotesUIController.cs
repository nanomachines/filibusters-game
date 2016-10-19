using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon;

namespace Filibusters
{
	public class VotesUIController : PunBehaviour
	{
        [SerializeField]
        GameObject[] mVoteUIElements;
        Text[] mVoteTextElements;

		// Use this for initialization
		void Start()
        {
            EventSystem.OnCoinDepositedEvent += UpdateDepositCounts;

            mVoteTextElements = new Text[mVoteUIElements.Length];
            for (int i = 0; i < mVoteUIElements.Length; ++i)
            {
                mVoteTextElements[i] = mVoteUIElements[i].GetComponentInChildren<Text>();
            }

            var activePlayers = NetworkManager.GetActivePlayerNumbers();
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                mVoteUIElements[i].SetActive(activePlayers[i]);
            }
        }

        public void UpdateDepositCounts(int ownerId, int newDepositBalance)
        {
            var playerNum = NetworkManager.GetPlayerNumber(PhotonPlayer.Find(ownerId));
            mVoteTextElements[playerNum].text = newDepositBalance.ToString();
        }
		
        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            int playerNumberToFree = NetworkManager.GetPlayerNumber(otherPlayer);
            mVoteUIElements[playerNumberToFree].SetActive(false);
        }
	}
}
