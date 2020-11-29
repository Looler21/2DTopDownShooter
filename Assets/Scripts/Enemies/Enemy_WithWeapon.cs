using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WithWeapon : Enemy
{
	public Transform firingOrigin;
	private BaseWeapon weapon;
	public float fireRate;			// Can customize enemy fire rate on a per-enemy basis, easier to balance easier levels vs harder ones?
	private float timeSinceLastFire;

	public float firingRange = 300f;
	private bool enemyInRange;

	public float fireRateRandomValue = 0.5f;	//set to the max/min that an enemy can fire?

	protected override void Start()
	{
		base.Start();

		//TODO - default starting with pistol; find a way to make this selectable from a drop-down or something?
		weapon = new BaseWeapon(BaseWeapon.WeaponClass.Pistol, BaseWeapon.ShootType.projectile, 5);
		timeSinceLastFire = 0f;

		//enemyFireRate = weapon.GetFireRate();
		if (fireRate <= 0)
		{
			Debug.LogWarning("WARNING: Fire rate is 0 for enemy: " + gameObject.name + ". Defaulting to 1 shot/sec");
			fireRate = 1f;
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		Look(target);

		timeSinceLastFire += Time.deltaTime;

		if (Vector2.Distance(transform.position, target.position) <= firingRange)
			enemyInRange = true;
		else
			enemyInRange = false;
		
		if (enemyInRange)
		{
			if (timeSinceLastFire >= fireRate &&
				weapon.Shoot(Recoil(target.position), firingOrigin.position, timeSinceLastFire, 300f) )
			{
				timeSinceLastFire = 0f;     //Bullet was successfully shot from weapon.Shoot(), so reset this counter
				timeSinceLastFire += Random.Range(-fireRateRandomValue, fireRateRandomValue);	//randomly shoot earlier or later
			}
		}
	}

	private Vector2 Recoil(Vector2 targetPosition, float spread = 0.05f)		//could move this to baseweapon class - or just let this class handle how skilled each enemy is?
	{
		//Slightly offset target position left/right/up/down to simulate innacuracy
		targetPosition.x += Random.Range(-spread, spread);
		targetPosition.y += Random.Range(-spread, spread);
		return targetPosition;
	}

	/*
	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			//Debug.Log("I see someone");
			enemyInRange = true;
			//patrolling = false;
			chasing = false;
			
			target = collision.transform;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			//Debug.Log("I can't shoot the player anymore. I will attempt to chase them.");
			enemyInRange = false;
			//patrolling = false;
			chasing = true;
		}
	}
	*/
}
