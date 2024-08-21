using UnityEngine;
using System.Collections.Generic;

public class PickupAndStack : MonoBehaviour
{
	public enum EPickUpStatus
	{
		NOTHING,
		HOLDING_PIZZA,
		HOLDING_SODA,
		HOLIDNG_TRASH
	}

	[SerializeField]
	Transform handTransform;
	[SerializeField]
	float stackOffset = 0.1f;
	Stack<GameObject> stack = new Stack<GameObject>();
	[SerializeField]
	PlayerTouchMovement Player;

	EPickUpStatus pickUpStatus = EPickUpStatus.NOTHING;

	float YPositoin = 0.0f;

	uint MaxPizza = 3;

	private void Start()
	{
		enabled = false;
	}

	void Update()
	{
		if (Player.scaledMovement.magnitude > 0)
		{
			ApplyStackOffset();
		}
		else
		{
			ResetPositions();
		}
	}

	public GameObject GetLastHoldingObject()
	{
		if (stack.Count > 0)
		{
			return stack.Pop();
		}
		return null;
	}

	public void SetPickUpStatus(EPickUpStatus newStatus)
	{
		pickUpStatus = newStatus;
	}

	public bool CanPickUp()
	{
		return stack.Count <= MaxPizza;
	}

	public EPickUpStatus GetPickUpStatus()
	{
		return pickUpStatus;
	}

	public void PickupItem(GameObject item)
	{
		item.transform.SetParent(handTransform);
		Vector3 newPosition = Vector3.zero;
		newPosition.y += YPositoin;
		item.transform.localPosition = newPosition;
		item.transform.localRotation = Quaternion.identity;
		stack.Push(item);
		YPositoin += 0.2f;

		if(!enabled && stack.Count > 1)
		{
			enabled = true;
		}
	}

	void ApplyStackOffset()
	{
		int i = stack.Count-1;
		foreach (var item in stack)
		{
			Vector3 offset = item.transform.localPosition;
			offset.z = -Player.scaledMovement.magnitude * 100 * stackOffset * i;
			item.transform.localPosition = offset;
			i--;
		}
	}

	void ResetPositions()
	{
		foreach (var item in stack)
		{
			Vector3 offset = item.transform.localPosition;
			offset.z = 0;
			item.transform.localPosition = offset;
		}
	}
}
