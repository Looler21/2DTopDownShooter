using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to flash player red or whatever color when they collide with an object that can damage them
// Actual damage taking should be done elsewhere

public class FlashWhenCollide : MonoBehaviour
{
	public Color hitColor = new Color(1f, 0f, 0f);
	public float flashForSeconds = 0.2f;
	private Color origColor;
	private SpriteRenderer sprite;

	private void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		origColor = sprite.color;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//make sure bosses dont flash when they spawn bullets and their collision boxes hit them
		if (collision.gameObject.tag == "Bullet" && !gameObject.CompareTag("Enemy"))
		{
			StartCoroutine("FlashColor");
		}
	}
	
	//used by playerShooting by sending a message to this function when it detects a raycast hit
	public void FlashSpriteColor()
	{
		StartCoroutine("FlashColor");
	}

	IEnumerator FlashColor()
	{
		SpriteRenderer[] childrenSprites0 = gameObject.GetComponentsInChildren<SpriteRenderer>();
		if (childrenSprites0 != null)
		{
			foreach (SpriteRenderer sprite_ in childrenSprites0)
				sprite.color = hitColor;
		}

		yield return new WaitForSeconds(flashForSeconds);
		
		// same object as above
		// but have to create a new variable b/c the one above is destroyed when Unity resumes the running the IEnumerator
		SpriteRenderer[] childrenSprites1 = gameObject.GetComponentsInChildren<SpriteRenderer>();
		if (childrenSprites1 != null)
		{
			foreach (SpriteRenderer sprite_ in childrenSprites1)
				sprite.color = origColor;
		}
	}
}
