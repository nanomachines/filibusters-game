using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
	// Use this for initialization
	void Awake()
	{
        DontDestroyOnLoad(this);
	}
}
