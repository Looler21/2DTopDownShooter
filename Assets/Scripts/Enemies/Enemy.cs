using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Transform target;
	public float speed;
	public float attackRange;

	public bool patrolling = false;
	public bool chasing;

	public Transform[] patrolAreas; //positions to move from
	public float waitTime;
	public float startWaitTime;

	public bool die = false;

	private int areaToPatrol;
    protected virtual void Start()
    {
		target = GameObject.FindGameObjectsWithTag("Player")[0].transform;

		if(patrolAreas.Length > 0)
			patrolling = true;

        waitTime = startWaitTime;
        areaToPatrol = Random.Range(0, patrolAreas.Length);
    }

    protected virtual void Update()
    {
		if(die)
		{
			Die();
			die = false;
		}

        if (patrolling)
        {
            Look(patrolAreas[areaToPatrol]);
            transform.position = Vector2.MoveTowards(transform.position, patrolAreas[areaToPatrol].position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, patrolAreas[areaToPatrol].position) <= 2)
            {
                if (waitTime <= 0)
                {
                    areaToPatrol = Random.Range(0, patrolAreas.Length);
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
        else if (chasing)
        {
			float distanceToPlayer = Vector2.Distance(transform.position, target.position);
			Look(target);
			transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
			
			/*
			if (distanceToPlayer <= lungeRange)
			{
				lunging = true;
				lungeLocation = target.position;
			}
			

			//if (distanceToPlayer <= attackRange && !lunging)
			//{
			//	Debug.Log("Player touched me");
			//	Attack(target);
			//}

			if (lunging)
			{
				LungeAt(lungeLocation);
				if (distanceToPlayer <= attackRange)
				{
					Debug.Log("Lunged and hit");
					//Attack(target);
				}
			}
			else
			{
				//if its time during a random range, dodge to the side every x seconds during chasing/alert/being shot at(?)

				Look(target);

				transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

				if (distanceToPlayer <= lungeRange)
				{
					lunging = true;
					lungeLocation = target.position;
				}
			}
			*/
		}
	}

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
		Debug.Log("I touched something");
        if (collision.gameObject.CompareTag("Player"))
        {
            patrolling = false;
            chasing = true;
			Debug.Log("I am chasing the player");
        }
    }

    protected virtual void Look(Transform toLook)
    {
        Vector3 dir = toLook.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

	protected virtual void Attack(Transform playerManager)
	{
		float weaponDamage = 5f;
		PlayerHealth player = playerManager.GetComponent<PlayerHealth>();
		player.TakeDamage(weaponDamage);

		//Debug.Log(player.hp);
	}

	public virtual void Die()
	{
		DestroyImmediate(gameObject);
		Debug.Log("Killed enemy");
	}

	/*
	private void LungeAt(Vector3 location)
	{
		if (Vector2.Distance(transform.position, location) > 0)	
		{
			// Lunge
			transform.position = Vector2.MoveTowards(transform.position, location, lungeSpeedMultiplier * Time.deltaTime);
		}
		else
		{
			// Rotate enemy to slowly look at player while its motionless
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
	*/
}
