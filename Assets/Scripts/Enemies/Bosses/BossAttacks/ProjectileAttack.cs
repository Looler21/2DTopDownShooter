﻿using System.Collections;
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
	public float speed;			//how fast the projectile flies, based off of rigidbody2D.AddForce()
	
	private float duration_timer;
	private float rateOfFire_timer;


	public float angleOfFire;
	public float angleOfFireOffset;     //use this if you want to fire bullets behind the boss, 90 degrees of the boss, etc.
	public int bulletSpawns;        //bullets to fire at once

	public bool dontRotateWithPlayer;
	public bool patternCanRotate;
	public float patternRotationSpeed;
	private float angleOffsetCounter;


	[HideInInspector] public bool saveBossTransform;		//used to stop the boss from looking at the player; originally wanted it to save a constant transform so like bullet rotations wouldn't move during the attack
	private Transform savedTransform;

	public bool randomBulletSpread;
	public float spreadConstant;

	public float bulletScaleSize = 1f;

	public AudioSource fireSound;

	//public Pattern pattern;
	//public int[] bossPhasesUsedIn;
	
	public override void Do()
	{
		attackIsActive = true;
	}

	void Start()
	{
		attackIsActive = false;
		angleOffsetCounter = 0f;

		if (projectilePrefab == null)
			Debug.LogError("No projectile prefab defined for ProjectileAttack: " + gameObject.name);

		if (bossParent == null)
		{
			bossParent = transform.root;
			if (bossParent == null)
				Debug.LogWarning("WARNING: No root bossParent found for this ProjectileAttack to shoot from.");
		}

		// For the first frame/attack, set it to save the transform manually
		if (dontRotateWithPlayer)
			saveBossTransform = true;

		if(angleOfFire > 360f)
		{
			Debug.LogWarning("Angle of fire is > 360 degrees. Defaulting to 360.");
			angleOfFire = 360f;
		}

		if(spreadConstant >= 1f && randomBulletSpread == true)
			Debug.LogWarning("Warning: Spread constant is >= 1. Bullets *may* start going in unexpected directions");

		if(bulletScaleSize <= 0f)
		{
			Debug.LogWarning("Bullet scale size is <= 0 in attack: " + gameObject.name + ". Defaulting to size 1");
			bulletScaleSize = 1f;
		}
	}

	void FixedUpdate()
	{
		if(attackIsActive)
		{
			//gonna spaghetti this one is just in case theres a ConsecutiveAttack where multiple attacks may or may not look at player
			//bossParent.GetComponent<BossEnemy>().lookAtPlayer = true;

			// If boss must be static when firing this pattern, stop the boss from rotating to look at the player
			if (saveBossTransform && dontRotateWithPlayer)
			{
				// Stop the boss from looking at the player
				bossParent.GetComponent<BossEnemy>().lookAtPlayer = false;
				saveBossTransform = false;	// set this so this code block only gets called once

				/*
				savedTransform = bossParent; //apparently Transforms cant be copied, will only be referenced with =, so this doesnt save a static Transform value
				*/
			}
			
			// Update timers
			duration_timer += Time.deltaTime;
			rateOfFire_timer += Time.deltaTime;

			if(patternCanRotate)
				angleOffsetCounter += Time.deltaTime * patternRotationSpeed;

			// Shoot bullets with rate of fire
			if (rateOfFire_timer >= (1 / rateOfFire))
			{
				ShootWaveOfBullets(transform, angleOffsetCounter, angleOfFireOffset, 180f, 180f);

				if(fireSound != null)
					fireSound.PlayOneShot(fireSound.clip);

				rateOfFire_timer = 0f;
			}

			// When the attack is over
			if(duration_timer >= duration)
			{
				// Reset everything for when next boss attack
				attackIsActive = false;
				duration_timer = 0f;
				rateOfFire_timer = 0f;
				angleOffsetCounter = 0f;

				/*
				// When the boss attacks next, save the transform for that future time (if the bool dontRotateWithPlayer is set to true in Inspector)
				saveBossTransform = true;
				*/
				bossParent.GetComponent<BossEnemy>().lookAtPlayer = true;
				saveBossTransform = true;	//setting this to say its ready to set the boss.lookAtPlayer = false once
			}
		}
	}

	private void ShootWaveOfBullets(Transform transf, float positionOffset, float firingOffsetDegrees, float spriteAngleOffset = 180f, float bulletAngleOffset = 180f)
	{
		//sometimes bullets will collide with other bullets when randomBulletSpread == true, bc they spawn inside eachother
		//and they collide and mess up sprite rotation, nothing can do since they all run on RigidBodies and AddForce()


		if(firingOffsetDegrees > 0f)
		{
			spriteAngleOffset += firingOffsetDegrees;
			bulletAngleOffset += firingOffsetDegrees;
		}

		//offset the start angle, end angle by where the target is standing
		//Vector3 vectorToTarget = transform.position - target.position; 

		//Vector3 bossParent_rot = bossParent.rotation.eulerAngles;
		Vector3 rotationAngle = transf.rotation.eulerAngles;
		
		//calculate angle between each bullet spawn location
		float angleStep = (angleOfFire / bulletSpawns);
		
		float firingAngle = spriteAngleOffset - rotationAngle.z - positionOffset;
		firingAngle -= (angleOfFire / 2) - (firingOffsetDegrees);

		for (int i = 0; i < bulletSpawns; i++)
		{
			float x = transf.position.x;
			float y = transf.position.y;

			x += Mathf.Sin((firingAngle * Mathf.PI) / 180f);
			y += Mathf.Cos((firingAngle * Mathf.PI) / 180f);
			
			if (randomBulletSpread)
			{
				x += Random.Range(-spreadConstant, +spreadConstant);
				y += Random.Range(-spreadConstant, +spreadConstant);
			}
			
			Vector3 projectileSpawn = new Vector3(x, y, 0f);
			Vector2 shootDir = (projectileSpawn - transf.position).normalized;

			//try to align the bullet with the direction it's going
			float spriteOffset = 0f;
			float angle = Mathf.Rad2Deg * Mathf.Atan(shootDir.y/ shootDir.x);
			angle += spriteOffset;

			GameObject bullet;
			if (projectilePrefab != null)
			{
				bullet = Instantiate(projectilePrefab, projectileSpawn, Quaternion.Euler(new Vector3(0f, 0f, angle)));
				bullet.GetComponent<Rigidbody2D>().AddForce(shootDir.normalized * speed);
				bullet.transform.localScale = Vector3.one * bulletScaleSize;
			}

			//increase angle for next bullet in the circle
			firingAngle += angleStep;
		}
	}
}