using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class InputWrapper : MonoBehaviour
    {

        public static InputWrapper Instance = null;

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

        public void ForcePollInput()
        {
            Update();
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
            get
            {
                return (Mathf.Sign(mLeftYInput) == 1f &&
                    mLeftYInput > Mathf.Epsilon) || mJumpInput;
            }
        }

        public bool FallPressed
        {
            get { return mFallInput; }
        }

        // Private Fields
        private static readonly string LeftXInputName = "LeftX";
        private static readonly string LeftYInputName = "LeftY";
        private static readonly float FullXInputThreshold = Mathf.Sqrt(2) / 2f;

        private float mLeftXInput = 0f;
        private float mLeftYInput = 0f;
        private bool mJumpInput = false;
        private bool mFallInput = false;

	}
}
