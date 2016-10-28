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
            NetworkManager.CreateAndJoinGameSession(mSessionNameField.text.ToLower());
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(Scenes.READY_MENU);
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
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
