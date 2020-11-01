using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

	private float maxHealth;
	public float health;

	public Health(float maxHealth)
	{
		this.maxHealth = maxHealth;
		health = maxHealth;
	}

	public float getHealth() { return health; }

	public void Damage(float amount)// takes perscribed amount away
	{
		health -= health;
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
		if (transform.CompareTag("Player"))
		{
			Destroy(gameObject);
			//something to impliment here
		}else if (transform.CompareTag("Enemy"))
		{
			Destroy(gameObject);
			//something to implement here
		}
	}
}