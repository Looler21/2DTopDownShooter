using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Transform[] patrolAreas; //positions to move from
    public Transform target;

    public float speed;
    public float attackRange;
    public float waitTime;
    public float startWaitTime;

    public bool patrolling;
    public bool chasing;

	public bool lunging;
	public float lungeRange;
	public float lungeSpeedMultiplier;	//based on base speed
	Vector2 lungeLocation;
	public float lungeCooldown;
	[SerializeField] private float lungeCooldownTimer;

	private int areaToPatrol;
    void Start()
    {
        patrolling = true;
        waitTime = startWaitTime;
        areaToPatrol = Random.Range(0, patrolAreas.Length);

		lungeSpeedMultiplier *= speed;
    }

    void Update()
    {
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

			/*
			if (distanceToPlayer <= attackRange && !lunging)
			{
				Debug.Log("Player touched me");
				Attack(target);
			}
			*/
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
				Look(target);

				transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

				if (distanceToPlayer <= lungeRange)
				{
					lunging = true;
					lungeLocation = target.position;
				}
			}
		}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            patrolling = false;
            chasing = true;
        }
    }

    private void Look(Transform toLook)
    {
        Vector3 dir = toLook.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

	private void Attack(Transform playerManager)
	{
		float weaponDamage = 5f;
		PlayerHealth player = playerManager.GetComponent<PlayerHealth>();
		player.TakeDamage(weaponDamage);

		//Debug.Log(player.hp);
	}

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
}
