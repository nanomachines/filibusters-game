using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class OutcomeTextScript : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            GetComponent<UnityEngine.UI.Text>().text = GameGlobals.LocalPlayerWonGame ?
                "You Win!" : "You Lose!";
        }
    }
}
