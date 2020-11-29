using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingDeathScript : MonoBehaviour
{
	private void OnDestroy()
	{
		Application.Quit();
	}
}
