using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

        public static bool AnyJoysticksConnected()
        {
            foreach (var joystickName in Input.GetJoystickNames())
            {
                if (joystickName.Length != 0)
                {
                    return true;
                }
            }
            return false;
        }

        // Public Properties
        public float LeftXInput { get { return mLeftXInput; } }
        public float LeftYInput { get { return mLeftYInput; } }
        public float RightXInput { get { return mRightXInput; } }
        public float RightYInput { get { return mRightYInput; } }
        public bool JumpPressed { get { return mJumpInput; } }
        public bool FallPressed { get { return mFallInput; } }
        public bool FirePressed { get { return mFirePressed; } }
        public bool EquipWeaponPressed { get { return mEquipWeaponPressed; } }
        public bool CancelPressed { get { return Input.GetButtonDown(CancelAxis); } }
        public bool SubmitPressed
        {
            get
            {
                return Input.GetButtonDown(SubmitAxis) ||
                    (AnyJoysticksConnected() && Input.GetButtonDown(Xbox360SubmitAxis));
            }
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
            /*
             * Get Left "Stick" 
             */
            mLeftXInput = Input.GetAxis(LeftXInputName);
            if (mLeftXInput > FullXInputThreshold)
            {
                mLeftXInput = 1f;
            }
            else if (mLeftXInput < -FullXInputThreshold)
            {
                mLeftXInput = -1f;
            }

            mLeftYInput = AnyJoysticksConnected() ? Input.GetAxis(LeftYJoystickInputName) : Input.GetAxis(LeftYInputName);
            mFallInput = Mathf.Sign(mLeftYInput) == -1f;
            mJumpInput = Input.GetAxis(JumpInputName) > Mathf.Epsilon;

            /*
             * Get Right "Stick"
             */
            if (AnyJoysticksConnected())
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

            /*
             * Get attack input
             */
            mFirePressed = Input.GetAxis(FireAxis) > Mathf.Epsilon ||
                Input.GetAxis(Xbox360FireAxis) > Mathf.Epsilon;

            /*
             * Get Weapon Equip input
             */
            mEquipWeaponPressed = Input.GetAxis(EquipAxis) > Mathf.Epsilon;
        }

        private Vector2 GetMouseInput()
        {
            Vector3 mousePos = Input.mousePosition;
            return new Vector2(mousePos.x - Screen.width / 2f, mousePos.y - Screen.height / 2f);
        }

        public GameObject mLocalReadyRoomCharacter { private get; set; }

        // Private Fields
        public static readonly string JumpInputName = "Jump";
        public static readonly string LeftXInputName = "Left-X";
        public static readonly string LeftYInputName = "Left-Y";
        public static readonly string LeftYJoystickInputName = "Left-Y-Joystick";
        public static readonly string Xbox360AButtonName= "X360-A";
        // The Xbox360 LT is mapped to different axises depending on your OS
        public static readonly string Xbox360LeftTriggerName =
            Application.platform == RuntimePlatform.OSXPlayer || 
            Application.platform == RuntimePlatform.OSXEditor ? "X360-OSX-LT" : "X360-Win-LT";
        public static readonly string Xbox360RightXInputName =
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.OSXEditor ? "X360-OSX-Right-X" : "X360-Win-Right-X";
        public static readonly string Xbox360RightYInputName =
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.OSXEditor ? "X360-OSX-Right-Y" : "X360-Win-Right-Y";
        public static readonly string FireAxis = "Fire";
        public static readonly string Xbox360FireAxis =
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.OSXEditor ? "X360-OSX-Fire" : "X360-Win-Fire";
        public static readonly string EquipAxis = "Equip-Weapon";
        public static readonly string CancelAxis = "Cancel";

        public static readonly string SubmitAxis = "Submit";
        public static readonly string Xbox360SubmitAxis =
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.OSXEditor ? "X360-OSX-Submit" : "X360-Win-Submit";

        public static readonly float FullXInputThreshold = Mathf.Sqrt(2) / 2f;

        private float mLeftXInput = 0f;
        private float mLeftYInput = 0f;
        private float mRightXInput = 0f;
        private float mRightYInput = 0f;
        private bool mJumpInput = false;
        private bool mFallInput = false;
        private bool mFirePressed = false;
        private bool mEquipWeaponPressed = false;
    }
}
