using TNRD;
using UnityEngine;

public class InvisibleFloorTrigger : MonoBehaviour, IPlayerTriggerable
{
    public bool IsScalingEnabled = false;

    [SerializeField] private SerializableInterface<IPlayerTriggerable> Machine;

    public void OnTriggerEnter(Collider other)
    {
        Machine.Value.OnPlayerTrigger(other.GetComponent<PickupAndStack>());
    }

    public void OnTriggerExit(Collider other)
    {
        Machine.Value.OnPlayerStopTrigger();
	}

}
