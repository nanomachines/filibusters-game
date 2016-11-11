﻿using Photon;
using UnityEngine.UI;
using UnityEngine;

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
            mSessionNameField.onValidateInput += delegate(string input, int charIndex, char addedChar) { return ValidateChar(addedChar); };
            mErrorToaster = GetComponent<ErrorToast>();
        }

        void Update()
        {
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == mSessionNameField.gameObject &&
                InputWrapper.Instance.SubmitPressed &&
                InputWrapper.AnyJoysticksConnected())
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(mSessionLaunchButton);
            }
        }

        char ValidateChar(char newChar)
        {
            if (char.IsWhiteSpace(newChar))
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
