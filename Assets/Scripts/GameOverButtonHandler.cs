using UnityEngine;
using System.Collections;

public class GameOverButtonHandler : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
	}

    public void OnClicked()
    {
        Application.Quit();
    }
}
