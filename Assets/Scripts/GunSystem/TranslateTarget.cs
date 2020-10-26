using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateTarget : MonoBehaviour
{

	public float speed;

	private void Update()
	{
		transform.Translate(Vector2.one * speed * Time.deltaTime);
		Debug.Log("Moving");
	}
}
