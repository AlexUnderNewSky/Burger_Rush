using UnityEngine;

public interface IPlayerTriggerable
{
    void OnPlayerTrigger(PickupAndStack other) { }
    void OnPlayerStopTrigger() { }
}