using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWave : MonoBehaviour
{
	public GameObject wavesToStart; //SET TO WAVE MANAGER YOU WANT TO SPANW
	public GameObject DialogueToDisable;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Destroy(DialogueToDisable);
		wavesToStart.gameObject.SetActive(true);
		Destroy(gameObject);
	}
}
