using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsVisualizer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI CointText;

    [SerializeField]
    CoinCollection CoinObject;

	private void Start()
	{
		CoinObject.OnChangedCoins += new CoinCollection.PickedCoinDelegate(OnCoinPicked);
	}

	void OnCoinPicked(uint TotalCoins)
	{
		CointText.text = "Coins: " + TotalCoins.ToString();
	}
}
