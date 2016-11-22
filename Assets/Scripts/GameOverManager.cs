using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Filibusters
{
    public class GameOverManager : Photon.PunBehaviour
    {
        [SerializeField]
        private float mGameOverSeconds;
        [SerializeField]
        private GameObject mResultsOverlay;
        [SerializeField]
        private GameObject[] mPotentialWinners;
        [SerializeField]
        private GameObject[] mInactivePanels;
        [SerializeField]
        private Text[] mDepositTexts;
        [SerializeField]
        private Text[] mCoinTexts;
        [SerializeField]
        private Text[] mKillTexts;
        [SerializeField]
        private Text[] mDeathTexts;

        private bool[] mActivePlayers;
        private GameObject mVotesCanvas;
        private GameStatMonitor mStatMonitor;

        private bool mGameEnded;
        private PhotonView mPhotonView;

        void Start()
        {
            mVotesCanvas = GameObject.Find("Votes Canvas");
            mStatMonitor = GetComponent<GameStatMonitor>();

            mGameEnded = false;
            mPhotonView = GetComponent<PhotonView>();
            EventSystem.OnGameOverEvent += OnGameOver;
        }

        void OnDestroy()
        {
            EventSystem.OnGameOverEvent -= OnGameOver;
        }
        
        void OnGameOver(int winningActorId)
        {
            // Ensures that stats are not updated after the game ends
            if (!mGameEnded)
            {
                mGameEnded = true;
                mPhotonView.RPC("ShowGameOverScreen", PhotonTargets.All, winningActorId - 1);
            }
        }

        [PunRPC]
        public void ShowGameOverScreen(int winningActorId)
        {
            mVotesCanvas.SetActive(false);
            mResultsOverlay.SetActive(true);
            mPotentialWinners[winningActorId].SetActive(true);

            mActivePlayers = NetworkManager.GetActivePlayerNumbers();
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; i++)
            {
                if (mActivePlayers[i])
                {
                    mInactivePanels[i].SetActive(false);
                    mDepositTexts[i].text = mStatMonitor.GetDepositCount(i).ToString();
                    mCoinTexts[i].text = mStatMonitor.GetCollectionCount(i).ToString();
                    mKillTexts[i].text = mStatMonitor.GetKillCount(i).ToString();
                    mDeathTexts[i].text = mStatMonitor.GetDeathCount(i).ToString();
                }
            }
            StartCoroutine(WaitAndLoad());
            EventSystem.OnGameOverJiggle(winningActorId + 1 == PhotonNetwork.player.ID);
        }

        IEnumerator WaitAndLoad()
        {
            yield return new WaitForSeconds(mGameOverSeconds);
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.START_MENU);
        }
    }
}
