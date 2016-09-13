using UnityEngine;
using System.Collections.Generic;
using Photon;

public class CharacterSelectManager : PunBehaviour
{
    [HideInInspector] public static CharacterSelectManager instance = null;

    private int mPlayersReady;

	// Use this for initialization
	void Awake()
	{
        if (instance == null)
        {
            instance = this;
            PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable{ { "IsReady", false}, {"IsNew", true } });
            mPlayersReady = 0;
            foreach (var player in PhotonNetwork.playerList)
            {
                bool isReady = player.customProperties.ContainsKey("IsReady") ? (bool)player.customProperties["IsReady"] : false;
                if (isReady)
                {
                    ++mPlayersReady;
                }
            }
        }
	}

    void OnGUI()
    {
        var labelBuilder = new System.Text.StringBuilder(90);
        labelBuilder.AppendLine("PlayersReady: " + mPlayersReady);
        foreach (var player in PhotonNetwork.playerList)
        {
            var isReady = player.customProperties.ContainsKey("IsReady") ? player.customProperties["IsReady"] : false;
            labelBuilder.AppendLine(player.ID + ": " + isReady);
        }
        GUILayout.Label(labelBuilder.ToString());
    }

    public void ToggleLocalPlayerReady()
    {
        bool isReady = !(bool)PhotonNetwork.player.customProperties["IsReady"];
        PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable{ { "IsReady", isReady} });
    }

    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        var properties = playerAndUpdatedProps[1] as ExitGames.Client.Photon.Hashtable;
        if (properties.ContainsKey("IsReady"))
        {
            if ((bool)properties["IsReady"])
            {
                ++mPlayersReady;
            }
            else if (!properties.ContainsKey("IsNew"))
            {
                --mPlayersReady;
            }
        }
    }
}
