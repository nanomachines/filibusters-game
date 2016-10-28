using Photon;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Filibusters
{
    public class JoinSession : PunBehaviour
    {
        InputField mSessionNameField;

        [SerializeField]
        GameObject mErrorText;

        [SerializeField]
        float mToastTime;

        void Start()
        {
            mSessionNameField = GetComponent<InputField>();
            mSessionNameField.onEndEdit.AddListener(delegate { OnHostGameNameEntered(); });
        }

        public void OnHostGameNameEntered()
        {
            NetworkManager.JoinGameSession(mSessionNameField.text.ToLower());
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(Scenes.READY_MENU);
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            mErrorText.GetComponent<Text>().text = (string)codeAndMsg[1];
            StartCoroutine(ToastErrorText());
        }

        private IEnumerator ToastErrorText()
        {
            mErrorText.SetActive(true);
            yield return new WaitForSeconds(mToastTime);
            mErrorText.SetActive(false);
        }
    }
}

