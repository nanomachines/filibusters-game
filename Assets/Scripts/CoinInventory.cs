using UnityEngine;
using System.Collections;

public class CoinInventory : MonoBehaviour
{
    [SerializeField]
    private int mCoinCount;

	// Use this for initialization
	void Start()
	{
        mCoinCount = 0;
	}

    public void AddCoin()
    {
        ++mCoinCount;
    }
}
