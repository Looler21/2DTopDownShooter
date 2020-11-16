using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
	[SerializeField] private BossAttack[] attacks;
	private BossAttack currentAttack;
	[SerializeField] private Transform playerTarget;
	public Health hp;

	bool attacking = false;
	bool stahp = false;

	[SerializeField] private float timeBetweenAttacks;
	[SerializeField] private float attackTimer;

    protected override void Update()
    {
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
				//randomly choose attack
				int chosenIdx = Mathf.RoundToInt(Random.Range(0, attacks.Length));
				Debug.Log(chosenIdx);

				//get ready for attack by going further/closer from player (designated by attack's parameters)
				//Coroutine here maybe

				//do that attack once distance is far enough
				currentAttack = attacks[chosenIdx];
				currentAttack.Do();
				return;
			}
		}
		else
		{
			//Reset timers and current state
			attackTimer = timeBetweenAttacks;
			attacking = false;
		}
		
    }
}
