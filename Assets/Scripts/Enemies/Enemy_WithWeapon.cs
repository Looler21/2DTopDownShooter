using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WithWeapon : Enemy
{
	public Transform firingOrigin;
	private BaseWeapon weapon;
	public float fireRate;			// Can customize enemy fire rate on a per-enemy basis, easier to balance easier levels vs harder ones?
	private float timeSinceLastFire;

	private bool enemyInVision;

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

		timeSinceLastFire += Time.deltaTime;
		
		if (enemyInVision)
		{
			Look(target);
			if (timeSinceLastFire >= fireRate &&
				weapon.Shoot(Recoil(target.position), firingOrigin.position, timeSinceLastFire,300.0f) )
			{
				timeSinceLastFire = 0f;		//Bullet was successfully shot from weapon.Shoot(), so reset this counter
			}
		}
	}

	private Vector2 Recoil(Vector2 targetPosition, float spread = 0.05f)		//could move this to baseweapon class - or just let this class handle how skilled each enemy is?
	{
		//Slightly offset target position left/right to simulate innacuracy
		targetPosition.x += Random.Range(-spread, spread);
		targetPosition.y += Random.Range(-spread, spread);
		return targetPosition;
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			//Debug.Log("I see someone");
			enemyInVision = true;
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
			enemyInVision = false;
			//patrolling = false;
			chasing = true;
		}
	}
}
