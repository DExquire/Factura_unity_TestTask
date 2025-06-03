using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public TMP_Text coinText;

    private void Start()
    {
        CoinManager.Instance.OnCoinsAdded += UpdateCoinUI;
    }

    private void UpdateCoinUI(int coinAmount)
    {
        coinText.text = coinAmount.ToString();
    }
}
