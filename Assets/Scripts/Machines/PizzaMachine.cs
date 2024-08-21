using System.Collections.Generic;
using UnityEngine;

public class Spawner : MachineBase, IPlayerTriggerable
{
	[SerializeField] GameObject objectToSpawn;
	[SerializeField] Transform PositionToSpawn = null;

	Stack<GameObject> SpanwedObjects = new Stack<GameObject>();

	CoinCollection PlayerCollection;
	PickupAndStack PlayerPickUp;

	float YPositoin;

	void Start()
	{
		InvokeRepeating("SpawnObject", GetCurrentLevelData().spawnInterval, GetCurrentLevelData().spawnInterval);
	}

	void SpawnObject()
	{
		if (SpanwedObjects.Count < GetCurrentLevelData().maxSpawnCount)
		{
			Vector3 postionSpawn = PositionToSpawn.position;
			postionSpawn.y = postionSpawn.y + YPositoin;
			SpanwedObjects.Push(Instantiate(objectToSpawn, postionSpawn, Quaternion.identity));
			YPositoin += 0.2f;
		}
		else
		{
			CancelInvoke("SpawnObject");
		}
	}

	public override void StartLevelUpdate(CoinCollection Collection) 
	{
		PlayerCollection = Collection;
		InvokeRepeating("LevelUpdate", 0.7f, 0.2f);
	}

	protected override void LevelUpdate()
	{
		if (PlayerCollection.NumOfCurrentCoins > 0 && CoinsSpended < GetCurrentLevelData().coinsToUpdate)
		{
			CoinsSpended++;
			PlayerCollection.NumOfCurrentCoins--;
		}
		else
		{
			UpdateLevelData();
			CancelInvoke();
			CoinsSpended = 0;
			Invoke("CheckIfPlayerStanding", 0.7f);
		}
	}

	public override void StopLevelUpdate()
	{
		CancelInvoke();
	}

	public void OnPlayerTrigger(PickupAndStack other)
	{
		if (other && (other.GetPickUpStatus() == PickupAndStack.EPickUpStatus.NOTHING || 
			other.GetPickUpStatus() == PickupAndStack.EPickUpStatus.HOLDING_PIZZA))
		{
			PlayerPickUp = other;
			InvokeRepeating("PickUpPizzaByPlayer", 0, 0.1f);
        }
	}

	void CheckIfPlayerStanding()
	{
		InvokeRepeating("LevelUpdate", 0.7f, 0.2f);
	}

	void PickUpPizzaByPlayer()
	{
		if (SpanwedObjects.Count > 0 && PlayerPickUp.CanPickUp())
		{
			PlayerPickUp.PickupItem(SpanwedObjects.Pop());
			PlayerPickUp.SetPickUpStatus(PickupAndStack.EPickUpStatus.HOLDING_PIZZA);
			YPositoin -= 0.2f;
			InvokeRepeating("SpawnObject", GetCurrentLevelData().spawnInterval, GetCurrentLevelData().spawnInterval);
		}
		else
		{
			CancelInvoke("PickUpPizzaByPlayer");
		}
	}

    public void OnPlayerStopTrigger()
    {
        
    }
}
