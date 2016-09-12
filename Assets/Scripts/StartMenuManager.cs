using UnityEngine;
using System.Collections;

public class StartMenuManager : MonoBehaviour
{
    private static GUIStyle TitleStyle;
    private static float TitleWidthPercentage = .3f;
    private static float TitleHeightPercentage = .2f;
    private static float TitleRelativeXPos = .3f;
    private static float TitleRelativeYPos = .2f;

    private static GUIStyle ButtonStyle;
    private static float ButtonWidthPercentage = .3f;
    private static float ButtonHeightPercentage = .2f;

    #region Unity Methods
    // Use this for initialization
    void Start ()
	{
        TitleStyle = new GUIStyle();
        TitleStyle.fontSize = 32;
        ButtonStyle = new GUIStyle();
        ButtonStyle.fontSize = 20;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
    #endregion

    #region Callbacks
    void OnGUI()
    {
        float titleWidth = Screen.width * TitleWidthPercentage;
        float titleHeight = Screen.height * TitleHeightPercentage;
        float titleX = Screen.width * TitleRelativeXPos;
        float titleY = Screen.height * TitleRelativeYPos;
        GUI.Label(new Rect(titleX, titleY, titleWidth, titleHeight), "Filibusters", TitleStyle);

        float hostBtnWidth = Screen.width * ButtonWidthPercentage;
        float hostBtnHeight = Screen.height * ButtonHeightPercentage;
        float hostBtnX = titleX;
        float hostBtnY = Screen.height * TitleRelativeYPos * 2.5f;
        if (GUI.Button(
                new Rect(hostBtnX, hostBtnY, hostBtnWidth, hostBtnHeight),
                "Host Game",
                ButtonStyle
           ))
        {
            Debug.Log("Host Game!!!!");
        }

        float joinBtnWidth = Screen.width * ButtonWidthPercentage;
        float joinBtnHeight = Screen.height * ButtonHeightPercentage;
        float joinBtnX = titleX;
        float joinBtnY = Screen.height * TitleRelativeYPos * 3.5f;
        if (GUI.Button(
                new Rect(joinBtnX, joinBtnY, joinBtnWidth, joinBtnHeight),
                "Join Game",
                ButtonStyle
           ))
        {
            Debug.Log("Join Game!!!!");
        }
    }
    #endregion
}
