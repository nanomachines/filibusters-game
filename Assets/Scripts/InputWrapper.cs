﻿using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class InputWrapper : MonoBehaviour
    {

        public static InputWrapper Instance = null;

        // Public Methods
        public void ForcePollInput()
        {
            Update();
        }

        // Public Properties
        public float LeftXInput
        {
            get { return mLeftXInput; }
        }

        public float LeftYInput
        {
            get { return mLeftYInput; }
        }

        public float RightXInput
        {
            get { return mRightXInput;  }
        }

        public float RightYInput
        {
            get { return mRightYInput;  }
        }

        public bool JumpPressed
        {
            get { return mJumpInput; }
        }

        public bool FallPressed
        {
            get { return mFallInput; }
        }

        public bool FirePressed
        {
            get { return mFirePressed; }
        }

        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            mLeftXInput = Input.GetAxis(LeftXInputName);
            if (mLeftXInput > FullXInputThreshold)
            {
                mLeftXInput = 1f;
            }
            else if (mLeftXInput < -FullXInputThreshold)
            {
                mLeftXInput = -1f;
            }

            mLeftYInput = Input.GetAxis(LeftYInputName);
            mFallInput = Mathf.Sign(mLeftYInput) == -1f;
            mJumpInput = (mLeftYInput > Mathf.Epsilon) || Input.GetButton(Xbox360AButtonName) ||
                Input.GetAxis(Xbox360LeftTriggerName) > Mathf.Epsilon;

            bool joysticksConnected = false;
            foreach (var joystickName in Input.GetJoystickNames())
            {
                if (joystickName.Length != 0)
                {
                    joysticksConnected = true;
                    break;
                }
            }
            if (joysticksConnected)
            {
                var x = Input.GetAxis(Xbox360RightXInputName);
                var y = Input.GetAxis(Xbox360RightYInputName);
                var dir = new Vector2(x, y);
                dir.Normalize();
                mRightXInput = dir.x;
                mRightYInput = dir.y;
            }
            else
            {
                Vector2 mousePos = GetMouseInput();
                mousePos.Normalize();
                mRightXInput = mousePos.x;
                mRightYInput = mousePos.y;
            }

            mFirePressed = Input.GetAxis(FireAxis) > Mathf.Epsilon;
        }

        Vector2 GetMouseInput()
        {
            Vector3 worldMousePos = Input.mousePosition;
            return new Vector2(worldMousePos.x - mPlayerPos.x, worldMousePos.y - mPlayerPos.y);
        }

        // Private Fields
        private static readonly string FireAxis = "Fire";
        private static readonly string LeftXInputName = "Left-X";
        private static readonly string LeftYInputName = "Left-Y";
        private static readonly string Xbox360AButtonName= "X360-A";
        // The Xbox360 LT is mapped to different axises depending on your OS
        private static readonly string Xbox360LeftTriggerName =
            Application.platform == RuntimePlatform.OSXPlayer ? "X360-OSX-LT" : "X360-Win-LT";
        private static readonly string Xbox360RightXInputName =
            Application.platform == RuntimePlatform.OSXPlayer ? "X360-OSX-Right-X" : "X360-Win-Right-X";
        private static readonly string Xbox360RightYInputName =
            Application.platform == RuntimePlatform.OSXPlayer ? "X360-OSX-Right-Y" : "X360-Win-Right-Y";
        private static readonly float FullXInputThreshold = Mathf.Sqrt(2) / 2f;

        private float mLeftXInput = 0f;
        private float mLeftYInput = 0f;
        [SerializeField]
        private float mRightXInput = 0f;
        [SerializeField]
        private float mRightYInput = 0f;
        private bool mJumpInput = false;
        private bool mFallInput = false;
        private bool mFirePressed = false;

        private readonly Vector3 mPlayerPos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

    }
}
