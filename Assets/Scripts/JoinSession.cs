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
        GameObject mError;
        Text mErrorText;

        [SerializeField]
        float mToastTime;

        void Start()
        {
            mErrorText = mError.GetComponent<Text>();
            mSessionNameField = GetComponent<InputField>();
            mSessionNameField.onEndEdit.AddListener(delegate { OnHostGameNameEntered(); });
        }

        public void OnHostGameNameEntered()
        {
            mError.SetActive(false);
            var sessionName = mSessionNameField.text;
            sessionName = sessionName.Trim().ToLower();
            if (sessionName == "")
            {
                mErrorText.text = "Invalid session name: session name cannot be empty";
                StartCoroutine(ToastErrorText()); 
            }
            else
            {
                NetworkManager.JoinGameSession(sessionName);
            }
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(Scenes.READY_MENU);
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            mErrorText.text = (string)codeAndMsg[1];
            StartCoroutine(ToastErrorText());
        }

        private IEnumerator ToastErrorText()
        {
            mError.SetActive(true);
            yield return new WaitForSeconds(mToastTime);
            mError.SetActive(false);
        }
    }
}

