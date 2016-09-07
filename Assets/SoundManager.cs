using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;

    [SerializeField] private AudioSource coinSFX;
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource depositSFX;

    private Dictionary<string, AudioSource> MusicMap = new Dictionary<string, AudioSource>();
	// Use this for initialization
	void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        MusicMap.Add("coin", coinSFX);
        MusicMap.Add("jump", jumpSFX);
        MusicMap.Add("deposit", depositSFX);
	}
	
    public void Play(string sfxName)
    {
        MusicMap[sfxName].Play();
    }
}
