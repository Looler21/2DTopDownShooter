using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWave : MonoBehaviour
{
	public GameObject thingToEnable; //DIALOGUE FOR NEXT ROUND
	public GameObject waveToDestroy; //SET TO THE WAVE MANAGER THIS UNIT WAS APART OF
	public GameObject NextActivateWaveMan; // SET TO PREFAB OF PRESSURE PAD FOR NEXT WAVEMANAGER

	private void OnDestroy()
	{
		Destroy(waveToDestroy);
		Debug.Log("1");
		thingToEnable.GetComponent<CanvasGroup>().interactable = true;
		thingToEnable.GetComponent<CanvasGroup>().alpha = 1f;
		thingToEnable.GetComponent<CanvasGroup>().blocksRaycasts = true;

		Debug.Log("2");
		Instantiate(NextActivateWaveMan);

		Debug.Log("3");
		FindObjectOfType<playerMovement>().transform.position = new Vector2(-25.0f, -50.0f);
	}
}
