using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
	[SerializeField] private BossAttack[] attacks;
	/*Debug*/ [SerializeField] private BossAttack currentAttack;
	[SerializeField] private GameObject playerManager;
	private Transform playerTarget;
	public Health hp;

	public bool attacking = false;
	[HideInInspector] public bool lookAtPlayer = true;		//generally will be set from its attacks; some attacks may require boss to stop looking at player

	[SerializeField] private float timeBetweenAttacks;
	[SerializeField] private float attackTimer;

	public float stompAtkDistance = 7.0f;
	public bool stomping = false;
	private BossAttack stompAttack;     //just drag and drop the stompAttack script into the attacks list, just like any other BossAttack

	public float startupDelayAttack = 5.0f; //set this to delay its first attack when it first spawns

	private int previousAttackIdx;

	protected override void Start()
	{
		base.Start();

		playerManager = GameObject.FindGameObjectWithTag("Player");

		if(playerManager != null)
			playerTarget = playerManager.GetComponentInChildren<playerShooting>().transform;

		StompAttackStartupChecks();

		attackTimer += startupDelayAttack;
		lookAtPlayer = true;

		startupDelayAttack = startupDelayAttack + Time.time;

		previousAttackIdx = -1;
	}

	protected override void FixedUpdate()
    {
		//Beginning, when player hasn't seen boss yet - boss shouldn't attack until visible or its been long enough
		if (Time.time <= startupDelayAttack && !GetComponent<SpriteRenderer>().isVisible)
			return;

		/* Removing stomp attack pretty much
		//Is player too close? --> Push player back with stomp attack
		float distance = Vector3.Distance(playerTarget.position, this.transform.position);
		if (stompAttack != null && distance <= stompAtkDistance)
		{
			currentAttack = stompAttack;
			currentAttack.Do();
			stomping = currentAttack.attackIsActive;
			return;
		}
		*/

		if (!stomping && lookAtPlayer)
			Look(playerTarget, spriteOffset);

		//Update whether the attack is still in progress or not
		if (currentAttack != null)
			attacking = currentAttack.isActive();

		//If no attack in progress, check timer if boss can choose another attack
		if (!attacking)
		{
			if (Vector2.Distance(transform.position, target.position) >= stompAtkDistance)
				MoveTo(playerTarget);

			attackTimer -= Time.deltaTime;

			if (attackTimer <= 0)
			{
				//randomly choose attack, make sure its not stomp attack and not previous attack
				//unless only 1 attack
				int chosenIdx = Mathf.RoundToInt(Random.Range(0, attacks.Length));

				if (attacks.Length > 1)
				{
					while (chosenIdx == previousAttackIdx)      //&& attacks[chosenIdx].GetComponent<StompAttack>() != null)	//throwing out the stomp attack
					{
						chosenIdx = Mathf.RoundToInt(Random.Range(0, attacks.Length));
					}
				}

				//update previous attack
				previousAttackIdx = chosenIdx;

				//get ready for attack by going further/closer from player (designated by attack's parameters)
				//Coroutine here maybe

				//do that attack once distance is far enough
				currentAttack = attacks[chosenIdx];
				currentAttack.Do();

				//Reset attack timer
				attackTimer = timeBetweenAttacks;
				//Randomly make the boss shoot earlier/later by a second
				float earlierlater = 1.0f;
				if (earlierlater >= timeBetweenAttacks)
					Debug.LogWarning("Warning random value of time between attacks is bigger than the actual time between attacks");
				attackTimer += Random.Range(-earlierlater, earlierlater);

				return;
			}
		}	
		else
		{
			//???
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

	protected override void OnDestroy()
	{
		base.OnDestroy();

		GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
		foreach (GameObject bullet in bullets)
			Destroy(bullet);
	}
}
