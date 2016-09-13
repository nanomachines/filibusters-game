using Photon;
using UnityEngine;

public class CharacterSelectController : PunBehaviour 
{
	void Start()
	{
	}
	
	void Update()
	{
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CharacterSelectManager.instance.ToggleLocalPlayerReady();
        }
	}
}
