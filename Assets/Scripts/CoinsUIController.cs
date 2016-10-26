﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon;

namespace Filibusters
{
	public class CoinsUIController : PunBehaviour
	{
        [SerializeField]
        GameObject[] mVoteUIElements;
        Text[] mVoteTextElements;
        Text[] mCoinTextElements;

		void Start()
        {
            EventSystem.OnCoinDepositedEvent += UpdateDepositCounts;
            EventSystem.OnCoinCountUpdatedEvent += UpdateCoinCounts;

            mVoteTextElements = new Text[mVoteUIElements.Length];
            mCoinTextElements = new Text[mVoteUIElements.Length];
            for (int i = 0; i < mVoteUIElements.Length; ++i)
            {
                var textElements = mVoteUIElements[i].GetComponentsInChildren<Text>();
                mVoteTextElements[i] = mVoteUIElements[i].GetComponentInChildren<Text>();
                if (textElements[0].gameObject.tag == "CoinText")
                {
                    mCoinTextElements[i] = textElements[0];
                    mVoteTextElements[i] = textElements[1];
                }
                else
                {
                    mCoinTextElements[i] = textElements[1];
                    mVoteTextElements[i] = textElements[0];
                }
            }

            var activePlayers = NetworkManager.GetActivePlayerNumbers();
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                if (!activePlayers[i])
                {
                    DisableUI(i);
                }
            }

        }

        public void UpdateDepositCounts(int ownerId, int newDepositBalance)
        {
            var playerNum = NetworkManager.GetPlayerNumber(PhotonPlayer.Find(ownerId));
            mVoteTextElements[playerNum].text = newDepositBalance.ToString();
        }

        public void UpdateCoinCounts(int ownerId, int newCoinBalance)
        {
            var playerNum = NetworkManager.GetPlayerNumber(PhotonPlayer.Find(ownerId));
            mCoinTextElements[playerNum].text = newCoinBalance.ToString();
        }
		
        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            int playerNumberToFree = NetworkManager.GetPlayerNumber(otherPlayer);
            DisableUI(playerNumberToFree);
        }

        private void EnableUI(int playerNum)
        {
            foreach (Image img in mVoteUIElements[playerNum].GetComponentsInChildren<Image>())
            {
                Color c = img.color;
                c.a = 1F;
                img.color = c;
            }
            foreach (Text text in mVoteUIElements[playerNum].GetComponentsInChildren<Text>())
            {
                Color c = text.color;
                c.a = 1F;
                text.color = c;
            }
        }

        private void DisableUI(int playerNum)
        {
            foreach (Image img in mVoteUIElements[playerNum].GetComponentsInChildren<Image>())
            {
                Color c = img.color;
                c.a = 0.2F;
                img.color = c;
            }
            foreach (Text text in mVoteUIElements[playerNum].GetComponentsInChildren<Text>())
            {
                Color c = text.color;
                c.a = 0.2F;
                text.color = c;
            }
        }
	}
}
