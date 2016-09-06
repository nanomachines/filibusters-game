using UnityEngine;
using System.Collections;

public class DepositManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] players;
    private GameObject curPlayer = null;

    private bool player1 = false;
    private bool player2 = false;

    private float elapsedTime = 0f;

    [SerializeField]
    private float depositTime = 2f;
    [SerializeField]
    private int winCondition = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player1")
        {
            player1 = true;
        }
        else if (other.gameObject.name == "Player2")
        {
            player2 = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player1")
        {
            player1 = false;
        }
        else if (other.gameObject.name == "Player2")
        {
            player2 = false;
        }
    }

    void Update()
    {
        bool contested = player1 && player2;
        if (!contested && (player1 || player2))
        {
            // Wait deposit time secs
            elapsedTime += Time.deltaTime;
            if (elapsedTime > depositTime)
            {
                // Set the current player
                if (player1)
                    curPlayer = players[0];
                else if (player2)
                    curPlayer = players[1];
                else
                    curPlayer = null;

                elapsedTime = 0f;
                // Deposit money
                CoinManager coinManager = curPlayer.GetComponent<CoinManager>();
                if (coinManager.Deposit() >= winCondition)
                {
                    print(curPlayer.name + " WINS!!!");
                }
            }
        }
    }
}
