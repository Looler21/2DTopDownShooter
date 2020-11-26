using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
	[SerializeField] private BossAttack[] attacks;
	private BossAttack currentAttack;
	[SerializeField] private GameObject playerManager;
	private Transform playerTarget;
	public Health hp;

	public bool attacking = false;
	bool stahp = false;
	public bool lookAtPlayer;

	[SerializeField] private float timeBetweenAttacks;
	[SerializeField] private float attackTimer;

	private float stompAtkDistance = 4.0f;
	public bool stomping = false;
	private BossAttack stompAttack;		//just drag and drop the stompAttack script into the attacks list, just like any other BossAttack

	protected override void Start()
	{
		base.Start();

		playerTarget = playerManager.GetComponentInChildren<playerShooting>().transform;

		StompAttackStartupChecks();
	}

	protected override void FixedUpdate()
    {
		if(!stomping && !attacking && lookAtPlayer)
			Look(playerTarget, -90);

		//Update whether the attack is still in progress or not
		if(currentAttack != null)
			attacking = currentAttack.isActive();

		//If no attack in progress, check timer if boss can choose another attack
		if (!attacking)
		{
			attackTimer -= Time.deltaTime;

			if (attackTimer <= 0)
			{
				//Is player too close? --> Use next attack cycle to push player back with stomp attack
				float distance = Vector3.Distance(playerTarget.position, this.transform.position);
				if (stompAttack != null && distance <= stompAtkDistance)
				{
					currentAttack = stompAttack;
					currentAttack.Do();
					stomping = currentAttack.attackIsActive;
				}
				else
				{
					//randomly choose attack, make sure its not stomp attack
					int chosenIdx = Mathf.RoundToInt(Random.Range(0, attacks.Length));
					while(attacks[chosenIdx].GetComponent<StompAttack>() != null)
						chosenIdx = Mathf.RoundToInt(Random.Range(0, attacks.Length));

					//get ready for attack by going further/closer from player (designated by attack's parameters)
					//Coroutine here maybe

					//do that attack once distance is far enough
					currentAttack = attacks[chosenIdx];
					currentAttack.Do();
					return;
				}
			}
		}	
		else
		{
			//Reset timers and current state
			attackTimer = timeBetweenAttacks;
			attacking = false;
		}
    }

	void StompAttackStartupChecks()
	{
		//Search for a stomp attack in the list of attacks
		int stompAtkCount = 0;
		foreach (BossAttack attack in attacks)
		{
			if (attack.GetComponent<StompAttack>() != null)
			{
				stompAttack = attack;
				stompAtkCount++;
			}
		}

		if (stompAtkCount > 1)
			Debug.LogWarning("Found more than 1 Stomp Attacks for boss: " + name + ". Can only use the one with the largest index in the attacks array");

		//Get the stomp attack trigger distance from that script into this one
		if (stompAttack != null)
			stompAtkDistance = stompAttack.GetComponent<StompAttack>().stompDistance;
		else
			Debug.LogWarning("No stomp attack found, boss " + gameObject.name + " will not use it.");
	}
}
