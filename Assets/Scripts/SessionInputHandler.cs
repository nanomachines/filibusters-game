using Photon;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Filibusters
{
    public abstract class SessionInputHandler : PunBehaviour
    {
        InputField mSessionNameField;
        ErrorToast mErrorToaster;

        void Start()
        {
            mSessionNameField = GetComponent<InputField>();
            mSessionNameField.onEndEdit.AddListener(delegate { OnHostGameNameEntered(); });
            mErrorToaster = GetComponent<ErrorToast>();
        }

        public void OnHostGameNameEntered()
        {
            var sessionName = mSessionNameField.text;
            sessionName = sessionName.Trim().ToLower();
            if (sessionName == "")
            {
                mErrorToaster.ToastError("Invalid session name: session name cannot be empty");
            }
            else
            {
                OnValidSanitizedInput(sessionName); 
            }
        }

        protected abstract void OnValidSanitizedInput(string input);

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(Scenes.READY_MENU);
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            mErrorToaster.ToastError((string)codeAndMsg[1]);
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            mErrorToaster.ToastError((string)codeAndMsg[1]);
        }
    }
}
