using Photon;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;
using System.Collections;

namespace Filibusters
{
    public abstract class SessionInputHandler : PunBehaviour
    {
        [SerializeField]
        GameObject mSessionLaunchButton;
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
            mSessionNameField.onValidateInput += delegate(string input, int charIndex, char addedChar) { return ValidateChar(charIndex, addedChar); };
            mErrorToaster = GetComponent<ErrorToast>();
        }

        void Update()
        {
            bool usingController = InputWrapper.AnyJoysticksConnected();
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == mSessionNameField.gameObject &&
                usingController &&
                (InputWrapper.Instance.SubmitPressed || InputWrapper.Instance.LeftYInput < -Mathf.Epsilon))
            {
                var currentEvtSys = UnityEngine.EventSystems.EventSystem.current;
                currentEvtSys.SetSelectedGameObject(mSessionLaunchButton);
                StartCoroutine(TemporarilyDisableDownInput(currentEvtSys));
            }
        }

        IEnumerator TemporarilyDisableDownInput(UnityEngine.EventSystems.EventSystem es)
        {
            es.sendNavigationEvents = false;
            yield return new WaitForSeconds(0.3f);
            es.sendNavigationEvents = true;
        }

        char ValidateChar(int charIndex, char newChar)
        {
            if (charIndex >= 8 || !char.IsLetterOrDigit(newChar))
            {
                return '\0';
            }
            return newChar;
        }

        public void OnHostGameNameEntered()
        {
            var sessionName = mSessionNameField.text;
            sessionName = sessionName.ToLower();
            if (sessionName == "")
            {
                if (!mSessionNameField.wasCanceled)
                {
                    mErrorToaster.ToastError("Invalid session name: session name cannot be empty");
                }
            }
            else
            {
                OnValidSanitizedInput(sessionName);
            }
        }

        protected abstract void OnValidSanitizedInput(string input);

        public override void OnJoinedRoom()
        {
            SceneManager.LoadScene(Scenes.READY_SCREEN);
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
                mErrorToaster.ToastError("Unable to create room. Error code: " + errorCode);
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
                mErrorToaster.ToastError("Unable to join room. Error code: " + errorCode);
            }
        }
    }
}
