using UnityEngine;

public class MachineBase : MonoBehaviour
{
	[System.Serializable]
	public struct MachineData
	{
		[SerializeField]
		public float spawnInterval;
		[SerializeField]
		public uint maxSpawnCount;

		[SerializeField]
		public uint coinsToUpdate;
	}

	[SerializeField] MachineData[] LevelsData = new MachineData[7];
	uint currentIndex = 0;

	uint coinsSpended = 0;

	public uint CoinsSpended
	{
		get { return coinsSpended; }
		set { coinsSpended = value; }
	}

	protected MachineData GetCurrentLevelData() { return LevelsData[currentIndex]; }

	protected void UpdateLevelData()
	{
		if (currentIndex < LevelsData.Length-1)
		{
			currentIndex++;
		}
	}

	public uint GetCoinsSpended() {  return coinsSpended; }

	public virtual void StartLevelUpdate(CoinCollection Collection) { }

    public virtual void StopLevelUpdate() { }

	protected virtual void LevelUpdate()
	{

	}
}
