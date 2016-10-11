using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class StartGameButtonInteractivity : MonoBehaviour
	{
        private UnityEngine.UI.Button mStartButton;
        private UnityEngine.UI.Image mStartButtonImg;
		void Start()
		{
            mStartButton = GetComponent<UnityEngine.UI.Button>();
            mStartButton.interactable = false;

            mStartButtonImg = GetComponent<UnityEngine.UI.Image>();
            mStartButtonImg.enabled = false;

            EventSystem.OnAllPlayersReadyEvent += () =>
            {
                mStartButtonImg.enabled = true;
                mStartButton.interactable = true;
            };
            EventSystem.OnAllPlayersNotReadyEvent += () =>
            {
                mStartButtonImg.enabled = false;
                mStartButton.interactable = false;
            };
		}
	}
}
