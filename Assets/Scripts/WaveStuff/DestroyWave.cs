using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWave : MonoBehaviour
{

	public GameObject NextActivatePressurePad; // SET TO PREFAB OF PRESSURE PAD FOR NEXT WAVEMANAGER
	public int IndexOfDialogueToEnable;// int of index of dialogue

	private GameManager gameMan;

	private void Start()
	{
		gameMan = FindObjectOfType<GameManager>();
	}

	private void OnDestroy()
	{

		gameMan.TurnOnDialogue(IndexOfDialogueToEnable);
		Instantiate(NextActivatePressurePad);

		FindObjectOfType<playerMovement>().transform.position = new Vector2(-25.0f, -50.0f);
	}
}
