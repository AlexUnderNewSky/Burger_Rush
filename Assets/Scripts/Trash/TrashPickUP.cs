using UnityEngine;

public class TrashPickUP : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			PickupAndStack PickUpComponent = other.GetComponent<PickupAndStack>();
			if (PickUpComponent.GetPickUpStatus() == PickupAndStack.EPickUpStatus.NOTHING || PickUpComponent.GetPickUpStatus() == PickupAndStack.EPickUpStatus.HOLIDNG_TRASH)
            {
				PickUpComponent.PickupItem(this.gameObject);
				PickUpComponent.SetPickUpStatus(PickupAndStack.EPickUpStatus.HOLIDNG_TRASH);
			}
		}
	}
}
