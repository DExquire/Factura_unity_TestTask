using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinModel : MonoBehaviour
{
    public event Action<int> OnCoinsUpdated;
    public int coinsCount;

    public void RefreshCoin(int newCoins)
    {
        coinsCount += newCoins;
        OnCoinsUpdated?.Invoke(coinsCount);
    }
}
