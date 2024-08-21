using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    uint numOfCurrentCoins = 0;

	public uint NumOfCurrentCoins
	{
		get { return numOfCurrentCoins; }
		set 
		{ 
			numOfCurrentCoins = value;
			OnChangedCoins?.Invoke(numOfCurrentCoins);
		}
	}

	public delegate void PickedCoinDelegate(uint TotalCoins);

	public PickedCoinDelegate OnChangedCoins;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Coin")
		{
			NumOfCurrentCoins += 1;
			Destroy(other.gameObject);
		}
	}
}
