using UnityEngine;
using Photon;
using System.Collections;

namespace Filibusters
{
    public class GameStatMonitor : PunBehaviour
    {
        int[] mTotalCoins;
        int[] mDeposits;
        int[] mKills;
        int[] mDeaths;
        bool[] mActivePlayers;

        bool mDisplayGUI;
        int mMostCoinsDeposited;
        int mWinningPlayerNum;

        void Start()
        {
            mTotalCoins = new int[GameConstants.MAX_ONLINE_PLAYERS_IN_GAME];
            mDeposits = new int[GameConstants.MAX_ONLINE_PLAYERS_IN_GAME];
            mKills = new int[GameConstants.MAX_ONLINE_PLAYERS_IN_GAME];
            mDeaths = new int[GameConstants.MAX_ONLINE_PLAYERS_IN_GAME];

            EventSystem.OnCoinCollectedEvent += IncrementTotalCoinCount;
            EventSystem.OnCoinDepositedEvent += UpdateDepositCount;
            EventSystem.OnKilledEvent += IncrementKillCount;
            EventSystem.OnDeathEvent += IncrementDeathCount;

            mDisplayGUI = false;
            mMostCoinsDeposited = 0;
            mWinningPlayerNum = 0;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                mDisplayGUI = !mDisplayGUI;
            }
        }

        void OnGUI()
        {
            if (!mDisplayGUI)
            {
                return;
            }

            var label = new System.Text.StringBuilder();

            label.Append("<size=30><b><color=red>Coins:\t");
            foreach (int i in mTotalCoins)
            {
                label.Append(i.ToString() + "\t");
            }
            label.AppendLine();

            label.Append("Depo:\t");
            foreach (int i in mDeposits)
            {
                label.Append(i.ToString() + "\t");
            }
            label.AppendLine();

            label.Append("Kills:\t");
            foreach (int i in mKills)
            {
                label.Append(i.ToString() + "\t");
            }
            label.AppendLine();

            label.Append("Deaths:\t");
            foreach (int i in mDeaths)
            {
                label.Append(i.ToString() + "\t");
            }
            label.AppendLine("</color></b></size>");

            GUIStyle style = new GUIStyle();
            style.richText = true;
            GUILayout.Label(label.ToString(), style);
        }

        void OnDestroy()
        {
            EventSystem.OnCoinCollectedEvent -= IncrementTotalCoinCount;
            EventSystem.OnCoinDepositedEvent -= UpdateDepositCount;
            EventSystem.OnKilledEvent -= IncrementKillCount;
            EventSystem.OnDeathEvent -= IncrementDeathCount;
        }

        void IncrementTotalCoinCount(int collectingActorId)
        {
            int playerNum = NetworkManager.GetPlayerNumber(
                PhotonPlayer.Find(collectingActorId));
            ++mTotalCoins[playerNum];
        }

        void UpdateDepositCount(int depositingOwnerId, int newDepositBalance)
        {
            int playerNum = NetworkManager.GetPlayerNumber(
                PhotonPlayer.Find(depositingOwnerId));
            mDeposits[playerNum] = newDepositBalance;

            if (newDepositBalance > mMostCoinsDeposited)
            {
                mMostCoinsDeposited = newDepositBalance;
                mWinningPlayerNum = playerNum;
                EventSystem.OnLeadingPlayerUpdated(mWinningPlayerNum);
            }
        }

        void IncrementKillCount(int killerOwnerId)
        {
            int playerNum = NetworkManager.GetPlayerNumber(
                PhotonPlayer.Find(killerOwnerId));
            ++mKills[playerNum];
        }

        void IncrementDeathCount(int deadPlayerViewId)
        {
            int playerNum = NetworkManager.GetPlayerNumber(
                PhotonView.Find(deadPlayerViewId).owner);
            ++mDeaths[playerNum];
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            mActivePlayers = NetworkManager.GetActivePlayerNumbers();

            int leavingPlayerNum = NetworkManager.GetPlayerNumber(otherPlayer);
            if (leavingPlayerNum == mWinningPlayerNum)
            {
                mMostCoinsDeposited = 0;
                for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; i++)
                {
                    if (mActivePlayers[i] && mDeposits[i] > mMostCoinsDeposited)
                    {
                        mWinningPlayerNum = i;
                        mMostCoinsDeposited = mDeposits[i];
                    }
                }
                EventSystem.OnLeadingPlayerUpdated(mWinningPlayerNum);
            }
        }
    }
}
