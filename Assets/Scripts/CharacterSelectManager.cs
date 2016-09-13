using UnityEngine;
using System.Collections.Generic;
using Photon;

struct CharacterSelectOptions
{
    public CharacterSelectOptions(bool isReady = false)
    {
        IsReady = isReady;
    }

    public void ToggleReady()
    {
        IsReady = !IsReady;
    }

    public bool IsReady { get; private set; }
    // other fields go here...
    // character selected
    // character variant selected
    // name
}

public class CharacterSelectManager : PunBehaviour
{
    [HideInInspector] public CharacterSelectManager instance = null;
    Dictionary<int, CharacterSelectOptions> mCharacterSelectInfo;

	// Use this for initialization
	void Awake()
	{
        if (instance == null)
        {
            instance = this;
            mCharacterSelectInfo = new Dictionary<int, CharacterSelectOptions>();
            foreach (var player in PhotonNetwork.playerList)
            {
                mCharacterSelectInfo.Add(player.ID, new CharacterSelectOptions());
            }
        }
	}

    void OnGUI()
    {
        var labelBuilder = new System.Text.StringBuilder(80);
        foreach (var info in mCharacterSelectInfo)
        {
            labelBuilder.Append(info.Key);
            labelBuilder.Append(": ");
            labelBuilder.Append(info.Value.IsReady);
            labelBuilder.Append("\n");
        }
        GUILayout.Label(labelBuilder.ToString());
    }

    public void TogglePlayerReady(int id)
    {
        mCharacterSelectInfo[id].ToggleReady();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        mCharacterSelectInfo.Add(newPlayer.ID, new CharacterSelectOptions());
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        mCharacterSelectInfo.Remove(otherPlayer.ID);
    }
}
