using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generally used when bullets go offscreen, just destroy them
public class DestroyWhenOffscreen : MonoBehaviour
{
	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
