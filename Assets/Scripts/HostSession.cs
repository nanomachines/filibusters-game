using Photon;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Filibusters
{
    public class HostSession : PunBehaviour
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
                NetworkManager.CreateAndJoinGameSession(sessionName);
            }
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(Scenes.READY_MENU);
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
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
