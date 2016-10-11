using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class DynamicUIInputAxis : MonoBehaviour
	{
        private UnityEngine.EventSystems.StandaloneInputModule mInputModule;

        public void Start()
        {
            mInputModule = GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            mInputModule.horizontalAxis = InputWrapper.LeftXInputName;
            mInputModule.verticalAxis = InputWrapper.LeftYInputName;
        }

        public void Update()
        {
            mInputModule.submitButton = InputWrapper.Instance.AnyJoysticksConnected() ?
                InputWrapper.Xbox360SubmitAxis : InputWrapper.SubmitAxis;
        }
	}

}
