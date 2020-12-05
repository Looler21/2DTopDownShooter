using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	public float maxHealth;
	public float health;

	public float getHealth() { return health; }

	private void Awake()
	{
		health = maxHealth;
	}

	public virtual void Damage(float amount)// takes perscribed amount away
	{
		health -= amount;
		if(health <= 0)
		{
			Die();
		}
	}

	public void Heal(int amount)//adds perscribed amount to health
	{
		health += amount;
		if(health> maxHealth)
		{
			health = maxHealth;
		}
	}

	public void Die()
	{
		//do something to die
		if (transform.CompareTag("Player") || transform.CompareTag("PlayerSprite"))
		{
			Destroy(gameObject);
			//something to impliment here
		}
		else if (transform.CompareTag("Enemy"))
		{
			Destroy(gameObject);
			//something to implement here
		}
	}
}