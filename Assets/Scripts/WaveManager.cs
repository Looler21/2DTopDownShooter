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
	public int enemiesAlive;
	public int waveNum;

	[HideInInspector]private bool debug_gameOverAntiSpam = false;


	//TODO - remove when done implementing something that spawns waves when the previous one is finished
	public bool manuallySpawnNextWave = false;	// During play, tick the checkbox in the inspector to spawn the next wave

    // Start is called before the first frame update
    void Start()
    {
		enemiesAlive = 0;
		waveNum = 0;	// waveNum is 0 aligned, despite whatever you see in the inspector
    }

	// Update is called once per frame
	void Update()
	{
		if (manuallySpawnNextWave || enemiesAlive == 0)
		{
			if(SpawnWave(waveNum))
				waveNum++;

			manuallySpawnNextWave = false;
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
			if (waveAmountOfEnemies == waveAmountOfSpawns)  // Sanity check - make sure amt of enemy prefabs matchs amt of spawns
			{
				//Update how many enemies in wave
				enemiesAlive = waveAmountOfEnemies;

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
						Debug.LogError("ERROR on wave: " + waves[waveNum].waveName + ". Either an enemy prefab isn't defined, or not all spawnpoints are defined");
						enemiesAlive = 0;
						return false;
					}
				}

				// Successfully spawned all enemies
				return true;
			}
			else
				Debug.LogWarning("WARNING: Mismatch amount of EnemyPrefabs and Spawns on wave: \"" + waves[waveNum].waveName + "\"");

		}
		else
		{
			if (waveNum == waves.Length)
			{
				GameOver();
			}
			else
			{ 
				Debug.LogError("ERROR: waveNum is too high. [waveNum: " + waveNum + "], [waves.length: " + waves.Length +"]");
				GameOver();
			}
		}

		enemiesAlive = 0;
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

	private void GameOver()
	{
		if(!debug_gameOverAntiSpam)
		{
			Debug.Log("All waves complete!");
			debug_gameOverAntiSpam = true;
		}
		
	}
}
