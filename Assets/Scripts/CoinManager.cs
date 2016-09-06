using UnityEngine;
using System.Collections;

public class CoinManager : MonoBehaviour {

    private int coins = 3;
    private int deposited = 0;
    private string playerName = "";

    public void AddCoin()
    {
        coins++;
        print(playerName + " has " + coins + " coin(s)");
    }

    public int Deposit()
    {
        if (coins > 0)
        {
            coins--;
            deposited++;
            print(playerName + " deposited " + deposited + " coin(s)");
        }
        print(playerName + " has " + coins + " coin(s)");
        return deposited;
    }

    public void ResetCoins()
    {
        coins = 0;
        print(playerName + " has " + coins + " coin(s)");
    }

    void Start()
    {
        playerName = gameObject.name;
    }
}
