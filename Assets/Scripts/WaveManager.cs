using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	[System.Serializable]
	public class Wave
	{
		public string waveName;
		public GameObject[] enemyPrefabs;
		public Transform[] spawns;
	}

	public Wave[] waves;
	private GameObject[] enemies;
	[SerializeField]private int waveNum;


	//TODO - remove when done implementing something that spawns waves when the previous one is finished
	public bool spawnWave = false;	// During play, tick the checkbox in the inspector to spawn the next wave

    // Start is called before the first frame update
    void Start()
    {
		waveNum = 0;	// waveNum is 0 aligned, despite whatever you see in the inspector
    }

	// Update is called once per frame
	void Update()
	{
		if (spawnWave)
		{
			if(SpawnWave(waveNum))
				waveNum++;

			spawnWave = false;
		}
    }

	public bool SpawnWave(int waveNum)	// Return true if successfully spawns the wave of enemies
	{
		// Dump previous wave's enemies if they are alive; can take this out later
		KillPreviousWave();

		// Spawn all enemies in waves[waveNum]
		if (waveNum < waves.Length)
		{
			int waveAmountOfEnemies = waves[waveNum].enemyPrefabs.Length;
			int waveAmountOfSpawns = waves[waveNum].spawns.Length;

			// TODO - is there a better way to let scripters/other devs set spawn points + enemies to spawn?
			if (waveAmountOfEnemies == waveAmountOfSpawns)	// Sanity check - make sure amt of enemy prefabs matchs amt of spawns
			{
				enemies = new GameObject[waveAmountOfEnemies];

				// For each prefab enemy/spawn defined in the current wave, Instantiate those enemies
				for (int i = 0; i < waveAmountOfEnemies; ++i)
				{
					if (waves[waveNum].enemyPrefabs[i] != null && waves[waveNum].spawns[i] != null)
					{
						enemies[i] = Instantiate(waves[waveNum].enemyPrefabs[i],
									waves[waveNum].spawns[i], false);
					}
					else
					{
						Debug.Log("Error on wave: " + waves[waveNum].waveName + ". Either an enemy prefab isn't defined, or not all spawnpoints are defined");
						return false;
					}
				}

				// Successfully spawned all enemies
				return true;
			}
			else
				Debug.Log("Error on wave: \"" + waves[waveNum].waveName + "\", mismatch amount of EnemyPrefabs and Spawns.");

		}
		else
			Debug.Log("No wave to spawn next. \"waveNum\" would exceed how many waves given.");

		return false;
	}

	public void KillPreviousWave()
	{
		if (enemies != null && enemies.Length > 0)
		{
			foreach (GameObject enemy in enemies)
			{
				Destroy(enemy);
			}
		}
		else
			Debug.Log("No enemies to destroy from the previous wave.");
	}
}
