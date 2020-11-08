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
        else if (chasing)		//TODO - prone to chasing and getting stuck through walls
        {
			float distanceToPlayer = Vector2.Distance(transform.position, target.position);
			Look(target);
			transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

			if (distanceToPlayer <= attackRange)
			{
				Attack(target);
			}
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

	protected virtual void Look(Transform toLook, float degOffset)
    {
        Vector3 dir = toLook.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

	protected virtual void Look(Transform toLook)
	{
		Look(toLook, 0);
	}

	protected virtual void Attack(Transform playerManager)
	{
		//float weaponDamage = 5f;
		//PlayerHealth player = playerManager.GetComponent<PlayerHealth>();
		//player.TakeDamage(weaponDamage);

		//Debug.Log(player.hp);
	}

	public virtual void Die()
	{
		Destroy(gameObject);
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
