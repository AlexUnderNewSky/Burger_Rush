using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform spawnPoint;
    public List<Transform> queuePositions = new List<Transform>();
    private List<NPC> npcsInQueue = new List<NPC>();

    void Start()
    {
        InvokeRepeating("SpawnNPC", 0f, 2f);
    }

    void SpawnNPC()
    {
        if (npcsInQueue.Count < queuePositions.Count)
        {
            GameObject newNpc = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
            NPC npcScript = newNpc.GetComponent<NPC>();
            int targetIndex = npcsInQueue.Count; // Индекс цели для нового NPC
            npcsInQueue.Add(npcScript);
            npcScript.StartMovingToQueue(queuePositions[targetIndex], targetIndex);
            npcScript.ReachedQueuePosition += HandleNpcReachedPosition;
        }
    }

    private void HandleNpcReachedPosition(int index)
    {
        if (index == 7) // Если NPC достиг queuePositions[7]
        {
            NPC npc = npcsInQueue[index];
            npcsInQueue.RemoveAt(index);
            npc.LeaveQueue();
        }
    }
}