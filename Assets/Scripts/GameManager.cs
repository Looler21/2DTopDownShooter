using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public WaveManager[] waveManagers;
	public GameObject[] dialogues;

	public void TurnOnDialogue(int a)
	{
		dialogues[a].GetComponent<CanvasGroup>().interactable = true;
		dialogues[a].GetComponent<CanvasGroup>().alpha = 1f;
		dialogues[a].GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	public void TurnOffDialogue(int a)
	{
		dialogues[a].gameObject.SetActive(false);
	}

	public void TurnOffWave(int a)
	{
		waveManagers[a].gameObject.SetActive(false);
	}

	public void TurnOnWave(int a)
	{
		waveManagers[a].gameObject.SetActive(true);
	}



}
