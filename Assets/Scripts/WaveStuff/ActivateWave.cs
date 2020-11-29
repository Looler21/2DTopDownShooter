using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWave : MonoBehaviour
{
	public int IndexOfWaveToStart;
	public int IndexOfDialogueToDisable;

	private GameManager gameMan;

	private void Start()
	{
		gameMan = FindObjectOfType<GameManager>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		gameMan.TurnOffDialogue(IndexOfDialogueToDisable);
		gameMan.TurnOnWave(IndexOfWaveToStart);
		Destroy(gameObject);
	}
}
