using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Lunge : Enemy
{
	public bool lunging = false;
	public float lungeRange = 5;
	public float lungeSpeedMultiplier = 2;  //based on base speed
	[SerializeField] private Vector2 lungeLocation;

	public float lungeCooldown = 1.25f;
	[SerializeField] private float lungeCooldownTimer;
	public float distanceToPlayer;
	public float distanceToLunge;

	protected override void Start()
	{
		base.Start();

		lungeSpeedMultiplier *= speed;
	}

	protected override void FixedUpdate()
	{
		//Base enemy class update
		//base.Update();		//if you double up on the update, it makes for a really cool effect jiggly effect, but i dont think the double code is good - also doubles their movement speed
		
		if(chasing)
		{
			distanceToPlayer = Vector2.Distance(transform.position, target.position);

			if (lunging)
			{
				LungeAt(lungeLocation);
				if (distanceToPlayer <= attackRange)
				{
					//Debug.Log("Lunged and hit");
					Attack(target);
				}
			}
			else
			{
				//TODO - if its time during a random range, dodge to the side every x seconds during chasing/alert/being shot at(?)

				base.Look(target, 90f);

				transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

				if (distanceToPlayer <= lungeRange)
				{
					lunging = true;
					lungeLocation = target.position;
				}
			}
		}
		else
		{
			MoveTo(target);
		}
	}

	protected override void Attack(Transform playerManager)
	{
		base.Attack(playerManager);
		Debug.Log("Child class Attack() called");
	}

	private void LungeAt(Vector3 location)
	{
		distanceToLunge = Vector2.Distance(transform.position, location);
		if (distanceToLunge > 1)		// if set to 0, they will sometimes break b/c --> 1e-08 > 0
		{
			// Lunge
			transform.position = Vector2.MoveTowards(transform.position, location, lungeSpeedMultiplier * Time.deltaTime);
		}
		else
		{
			//TODO - put in a dazed animation, or rotate the enemy slowly to look back at player while its motionless
			//Look(target.transform);

			// Keep track of timers; If finished get out of lunging state
			if (lungeCooldownTimer <= lungeCooldown)
			{
				lungeCooldownTimer += Time.deltaTime;
			}
			else
			{
				lunging = false;
				lungeCooldownTimer = 0f;
			}
		}
	}
}
