using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public event Action<int> OnCoinsAdded;
    private int coinAmount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        coinAmount = 0;
        AddCoins(coinAmount);
    }

    public void AddCoins(int coinAmount)
    {
        this.coinAmount += coinAmount;
        OnCoinsAdded?.Invoke(this.coinAmount);
    }
}
