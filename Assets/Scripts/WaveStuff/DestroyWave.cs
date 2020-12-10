using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyWave : MonoBehaviour
{

	public GameObject NextActivatePressurePad; // SET TO PREFAB OF PRESSURE PAD FOR NEXT WAVEMANAGER
	public int IndexOfDialogueToEnable;// int of index of dialogue
	public GameObject WeaponToDrop;
	public bool IsBoss = false;

	private GameManager gameMan;

	private void Start()
	{
		gameMan = FindObjectOfType<GameManager>();
	}

	private void OnDestroy()
	{
		if (IsBoss)
		{
			Debug.Log("Is Boss");
			SceneManager.LoadScene("Credits");
		}
		else
		{
			Debug.Log("Not Boss");
			gameMan.TurnOnDialogue(IndexOfDialogueToEnable);
			Instantiate(NextActivatePressurePad);

			FindObjectOfType<playerShooting>().transform.position = FindObjectOfType<SpawnPoint>().gameObject.transform.position;
			Instantiate<GameObject>(WeaponToDrop, FindObjectOfType<WeaponSpawnPoint>().gameObject.transform);
		}
	}
}
