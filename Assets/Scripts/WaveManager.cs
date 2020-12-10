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

	public bool wavesStarted = false;
	public bool spawnNextWave = false;  // During play, can tick the checkbox in the inspector to spawn the next wave

	public Wave[] waves;
	private GameObject[] enemies;
	public int enemiesAlive;
	public int waveNum;

	[HideInInspector]private bool debug_gameOverAntiSpam = false;
	[HideInInspector] private bool debug_waveStartAntiSpam = false;

    void Start()
    {
		enemiesAlive = 0;
		waveNum = 0;	// waveNum is 0 aligned, despite whatever you see in the inspector
    }

	private void OnEnable()
	{
		wavesStarted = true;
		spawnNextWave = true;
	}

	void Update()
	{
		if ((spawnNextWave || enemiesAlive == 0) && wavesStarted)
		{
			if (enemiesAlive > 0 && spawnNextWave)
				Debug.LogWarning("Force spawning next wave, even with enemies alive.");

			if(SpawnWave(waveNum))
				waveNum++;

			spawnNextWave = false;
		}
		else if (!wavesStarted && spawnNextWave && !debug_waveStartAntiSpam)
		{
			Debug.LogWarning("WARNING: Waves have not yet started. Cannot spawn next wave. Use StartWaves() first.");
			debug_waveStartAntiSpam = true;
		}
    }

	public bool SpawnWave(int waveNum)	// Return true if successfully spawns the wave of enemies
	{
		// Dump previous wave's enemies if they are alive; can take this out later
		KillPreviousWave();

		Debug.Log("Spawning a Wave");

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
									waves[waveNum].spawns[i].position,Quaternion.identity);
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
			if (waveNum == waves.Length-1)
			{
				WavesOver();
			}
			else
			{ 
				Debug.LogError("ERROR: waveNum is too high. [waveNum: " + waveNum + "], [waves.length: " + waves.Length +"]");
				WavesOver();
			}
		}

		enemiesAlive = 0;
		return false;
	}

	public void StartWaves()	// Set the state of this WaveManager to have started the waves
	{
		wavesStarted = true;
		spawnNextWave = true;
	}

	public void SpawnNextWave()		// Force next wave to spawn if you already started the waves; forcing next wave to spawn kills all enemies in previous one currently
	{
		spawnNextWave = true;
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

	private void WavesOver()
	{
		//Disable the gameobject after all waves are done with
		wavesStarted = false;

		if (!debug_gameOverAntiSpam)
		{
			Debug.Log("All waves complete! Disabling manager: " + name);
			debug_gameOverAntiSpam = true;
		}

		this.gameObject.SetActive(false);
	}
}
