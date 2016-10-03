using UnityEngine;
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

        public bool JumpPressed
        {
            get { return mJumpInput; }
        }

        public bool FallPressed
        {
            get { return mFallInput; }
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
        }

        // Private Fields
        private static readonly string LeftXInputName = "Left-X";
        private static readonly string LeftYInputName = "Left-Y";
        private static readonly string Xbox360AButtonName= "X360-A";
        // The Xbox360 LT is mapped to different axises depending on your OS
        private static readonly string Xbox360LeftTriggerName =
            Application.platform == RuntimePlatform.OSXPlayer ? "X360-OSX-LT" : "X360-Win-LT"; 
        private static readonly float FullXInputThreshold = Mathf.Sqrt(2) / 2f;

        private float mLeftXInput = 0f;
        private float mLeftYInput = 0f;
        private bool mJumpInput = false;
        private bool mFallInput = false;

    }
}
