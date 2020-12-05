using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float damageValue;

	void Start()
	{
		if (damageValue <= 0)
		{
			Debug.LogWarning("Bullet: " + gameObject.name + " has a damage value of 0. They will not do any damage.");
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Walls"))
		{
			Destroy(gameObject);
		}

		//doesnt seem to work 100%, not sure why sometimes bullets will have rotation wrong still
		if (collision.gameObject.tag == "Bullet")
		{
			Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
		}
	}
}

