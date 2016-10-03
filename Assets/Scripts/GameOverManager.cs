using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class GameOverManager : MonoBehaviour
	{
        private PhotonView mPhotonView;

		// Use this for initialization
		void Start()
		{
            mPhotonView = GetComponent<PhotonView>();
            EventSystem.OnGameOverEvent += OnGameOver;
		}
		
        void OnGameOver(int winningActorId)
        {
            mPhotonView.RPC("ShowGameOver", PhotonTargets.All, winningActorId);
        }

        [PunRPC]
        public void ShowGameOver(int winningActorId)
        {
            GameGlobals.LocalPlayerWonGame = winningActorId == PhotonNetwork.player.ID;
            PhotonNetwork.LoadLevel(Scenes.SCENE_DIR + Scenes.GAME_OVER);
        } 
	}
}
