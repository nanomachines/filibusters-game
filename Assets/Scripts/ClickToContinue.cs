using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClickToContinue : MonoBehaviour {

	void Update()
    {
	    if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Main");
        }
	}
}
