using Photon;
using UnityEngine.UI;

using System.Collections.Generic;

namespace Filibusters
{
    public abstract class SessionInputHandler : PunBehaviour
    {
        InputField mSessionNameField;
        ErrorToast mErrorToaster;

        private static readonly Dictionary<int, string> ErrorCodeMap = new Dictionary<int, string>()
        {
            { ErrorCode.GameIdAlreadyExists, "Game id already exists" },
            { ErrorCode.GameFull , "Game is full" },
            { ErrorCode.GameClosed, "Game is closed" },
            { ErrorCode.GameDoesNotExist, "Game does not exist" }
        };

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
            PhotonNetwork.LoadLevel(Scenes.READY_SCREEN);
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            short errorCode = (short)codeAndMsg[0];
            if (ErrorCodeMap.ContainsKey(errorCode))
            {
                mErrorToaster.ToastError(ErrorCodeMap[errorCode]);
            }
            else
            {
                mErrorToaster.ToastError("Unabled to create room. Error code: " + errorCode);
            }
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            short errorCode = (short)codeAndMsg[0];
            if (ErrorCodeMap.ContainsKey(errorCode))
            {
                mErrorToaster.ToastError(ErrorCodeMap[errorCode]);
            }
            else
            {
                mErrorToaster.ToastError("Unabled to join room. Error code: " + errorCode);
            }
        }
    }
}
