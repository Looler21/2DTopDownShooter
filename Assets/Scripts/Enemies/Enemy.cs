using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Transform target;
	public float speed;
	public float attackRange;
	public float attackDamage;

	//public bool patrolling = false;
	public bool chasing;

	//public Transform[] patrolAreas; //positions to move from
	public float waitTime;
	public float startWaitTime;

	[SerializeField] protected WaveManager waveManager;
	private int areaToPatrol;

	public float spriteOffset;

    protected virtual void Start()
    {
		if (target == null)
		{
			target = GameObject.FindGameObjectsWithTag("PlayerSprite")[0].transform;
			if(target == null)
				Debug.LogWarning("Enemy: " + gameObject.name + " cannot find a GameObject with the tag PlayerSprite");
		}
		
			//if(patrolAreas.Length > 0)
			//	patrolling = true;

			waitTime = startWaitTime;
        //areaToPatrol = Random.Range(0, patrolAreas.Length);

		waveManager = FindObjectOfType<WaveManager>();
		if(waveManager == null)
			Debug.LogWarning("WARNING: No active WaveManager to keep track of waves and enemy deaths. EnemyName: " + gameObject.name);

		if (speed <= 0)
		{
			Debug.LogWarning("WARNING: Speed is 0 for enemy: " + gameObject.name);
		}

		if(attackDamage <= 0)
			Debug.LogWarning("WARNING: Damage is <= 0 for enemy: " + gameObject.name + ". Will not do any damage to players.");

		//initialize enemies to chase players on startup
		chasing = true;
	}

    protected virtual void FixedUpdate()
    {

		/*
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
		*/

		float distanceToPlayer = Vector2.Distance(transform.position, target.position);
		if (chasing)        //TODO - prone to chasing and getting stuck through walls
		{
			Look(target, spriteOffset);
			transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

			if (distanceToPlayer <= attackRange)
			{
				Attack(target);
			}
		}
		else if(distanceToPlayer == attackRange)
		{
			MoveTo(target);
		}
	}

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //patrolling = false;
            chasing = true;
        }
    }

	protected virtual void MoveTo(Transform target)
	{
		transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
	}

	protected virtual void Look(Transform toLook, float degOffset = 0f)
	{

		//float rotationSpeed
		/*
		Quaternion.Slerp(transform.rotation,
			rotator, rotationSpeed * Time.deltaTime);
		*/
		
		Vector3 dir = toLook.position - transform.position;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - degOffset;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	protected virtual void Attack(Transform playerManager)
	{
		PlayerHealth player = playerManager.GetComponent<PlayerHealth>();
		player.Damage(attackDamage);
	}

	protected virtual void UpdateWaveManager(WaveManager waveManager)
 	{
		//Destroy(gameObject);
		//Debug.Log("Killed enemy");

		if (waveManager != null && waveManager.enemiesAlive >= 0)
			waveManager.enemiesAlive--;
		else if (waveManager != null)
			Debug.LogError("ERROR: Die() - Cannot decrease waves[" + waveManager.waveNum + "]'s enemiesAlive count because it's <= 0.");
		else if (waveManager == null)
			Debug.LogError("ERROR: Enemy destroyed. No wave manager found to decrease death of enemy: " + transform.root.name);
	}

	protected virtual void OnDestroy()
	{
		UpdateWaveManager(waveManager);
	}
}
