using System;
using UnityEngine;

public class Dumpster : MonoBehaviour, IPlayerTriggerable
{
	PickupAndStack PickUpComponent;

	public void OnPlayerTrigger(PickupAndStack other)
	{
		if(other)
		{
			PickUpComponent = other;
			InvokeRepeating("DeleteHoldingItem", 0f, 0.2f);
		}
	}

	public void OnPlayerStopTrigger()
	{
		CancelInvoke("DeleteHoldingItem");
	}

	private void DeleteHoldingItem()
	{
		GameObject Object = PickUpComponent.GetLastHoldingObject();
		if (Object)
		{
			Destroy(Object);
		}
		else
		{
			CancelInvoke("DeleteHoldingItem");
			PickUpComponent.SetPickUpStatus(PickupAndStack.EPickUpStatus.NOTHING);
		}
	}

}
