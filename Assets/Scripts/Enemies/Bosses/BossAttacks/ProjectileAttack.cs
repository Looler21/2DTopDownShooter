using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileAttack : BossAttack
{
	/*
	public enum Pattern
	{
		BOSSATTACK_PROJECTILE_FRONT,
		BOSSATTACK_PROJECTILE_CIRCULAR
	}
	*/

	private Transform bossParent;
	public GameObject projectilePrefab;
	public float duration;
	
	public float rateOfFire;    //projectiles spawned per sec
	public float speed;			//how fast the projectile flies
	
	private float duration_timer;
	private float rateOfFire_timer;


	public float angleOfFire;
	public int bulletSpawns;        //bullets to fire at once


	//public Pattern pattern;
	//public int[] bossPhasesUsedIn;



	public override void Do()
	{
		attackIsActive = true;
	}

	void Start()
	{
		attackIsActive = false;

		if (bossParent == null)
		{
			bossParent = transform.root;
			if (bossParent == null)
				Debug.LogWarning("WARNING: No root bossParent found for this ProjectileAttack to shoot from.");
		}
	}

	void Update()
	{
		if(attackIsActive)
		{
			duration_timer += Time.deltaTime;
			rateOfFire_timer += Time.deltaTime;

			if (rateOfFire_timer >= (1 / rateOfFire))
			{
				ShootWaveOfBullets(180f);
				rateOfFire_timer = 0f;
			}

			if(duration_timer >= duration)
			{
				attackIsActive = false;
				duration_timer = 0f;
				rateOfFire_timer = 0f;
			}
		}
	}

	private void ShootWaveOfBullets(float spriteAngleOffset)
	{
		//offset the start angle, end angle by where the target is standing
		//Vector3 vectorToTarget = transform.position - target.position; 
		Vector3 bossParent_rot = bossParent.rotation.eulerAngles;
		
		float angleStep = angleOfFire / bulletSpawns;

		float firingAngle = spriteAngleOffset - bossParent_rot.z;
		firingAngle -= (angleOfFire / 2);
		
		
		for (int i = 0; i < bulletSpawns; i++)
		{
			float x = transform.position.x + Mathf.Sin((firingAngle * Mathf.PI) / 180f);
			float y = transform.position.y + Mathf.Cos((firingAngle * Mathf.PI) / 180f);

			Vector3 velDir = new Vector3(x, y, 0f);
			Vector2 shootDir = (velDir - transform.position).normalized;
			
			GameObject bullet = Instantiate(projectilePrefab, velDir, Quaternion.Euler(new Vector3(0f, 0f, firingAngle)));
			bullet.GetComponent<Rigidbody2D>().AddForce(shootDir.normalized * speed);

			firingAngle += angleStep;
		}
	}
}