using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class GameStatMonitor : MonoBehaviour
    {
        int[] mTotalCoins;
        int[] mDeposits;
        int[] mKills;
        int[] mDeaths;

        bool mDisplayGUI;

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
        }

        void IncrementKillCount(int killerOwnerId)
        {
            int playerNum = NetworkManager.GetPlayerNumber(
                PhotonPlayer.Find(killerOwnerId));
            ++mKills[playerNum];
        }

        void IncrementDeathCount(int deadPlayerViewId, Vector3 pos)
        {
            int playerNum = NetworkManager.GetPlayerNumber(
                PhotonView.Find(deadPlayerViewId).owner);
            ++mDeaths[playerNum];
        }
    }
}
