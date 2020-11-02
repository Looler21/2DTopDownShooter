using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class playerShooting : MonoBehaviour {

	private BaseWeapon weapon;
	private BaseWeapon weaponPistol;
	private BaseWeapon weaponRifle;
	private BaseWeapon weaponSniper;
	private BaseWeapon weaponPlasma;

	public Transform firingPoint; //Where the bullet will travel/check from (realistic gun)
	private Vector2 mousePosition; //position of mouse
	private Vector2 firingOrigin;

	private bool RifleUsable;
	private bool SniperUsable;
	private bool PlasmaUsable;

	public float timeSinceLastFire = 0;

	[Tooltip("Prefab for the plasma gun")]
	public GameObject plasmaPrefab; // projectile prefab for plasma gun

	private void Awake()
	{
		weaponPistol = new BaseWeapon(BaseWeapon.WeaponClass.Pistol, BaseWeapon.ShootType.hitscan,7);
		weaponRifle = new BaseWeapon(BaseWeapon.WeaponClass.Rifle, BaseWeapon.ShootType.hitscan,30);
		weaponSniper = new BaseWeapon(BaseWeapon.WeaponClass.Sniper, BaseWeapon.ShootType.hitscan,2);
		weaponPlasma = new BaseWeapon(BaseWeapon.WeaponClass.Plasma, BaseWeapon.ShootType.projectile,5);
	}

	private void Start()
	{
		SetWeapon(weaponPistol);
	}

	// Update is called once per frame
	void Update () 
	{
		timeSinceLastFire += Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SetWeapon(weaponPistol);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2) && RifleUsable)
		{
			SetWeapon(weaponRifle);
			Debug.Log("Rifle now equipped");
		}
		

		if (Input.GetKeyDown(KeyCode.Alpha3) && SniperUsable)
		{
			SetWeapon(weaponSniper);
			Debug.Log("Sniper now equipped");
		}
		

		if (Input.GetKeyDown(KeyCode.Alpha4) && PlasmaUsable)
		{
			SetWeapon(weaponPlasma);
			Debug.Log("Plasma now equipped");
		}
		

		if (Input.GetButtonDown("Fire1"))
		{
			if (weapon != null)
			{
				if(BaseWeapon.getShootType(weapon) == BaseWeapon.ShootType.hitscan)
				{
					firingOrigin = new Vector2(firingPoint.position.x, firingPoint.position.y);
					mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
						Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

					weapon.Shoot(mousePosition, firingOrigin, timeSinceLastFire);
					Debug.Log("Shot da ray");
					timeSinceLastFire = 0;
				}else if(BaseWeapon.getShootType(weapon) == BaseWeapon.ShootType.projectile)
				{
					firingOrigin = new Vector2(firingPoint.position.x, firingPoint.position.y);
					mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
						Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

					GameObject bullet = (GameObject)Instantiate(getWeaponProjectilePrefab(weapon), firingOrigin, Quaternion.identity);
					bullet.GetComponent<Rigidbody2D>().velocity = (firingOrigin - new Vector2(transform.position.x,transform.position.y)).normalized * getWeaponProjectileSpeed(weapon);
					Debug.Log("Shoot a Projectile");
				}
			}
			else
			{
				Debug.Log("Weapon null");
			}
		}
	}

	private GameObject getWeaponProjectilePrefab(BaseWeapon weapon)
	{
		switch (weapon.GetWeaponClass().ToString())
		{
			default:
				return null; // return path should never be needed
			case "Plasma":
				return plasmaPrefab;
		}
	}

	private float getWeaponProjectileSpeed(BaseWeapon weapon)
	{
		switch (weapon.GetWeaponClass().ToString())
		{
			default:
				return 0; // return path should never be needed
			case "Plasma":
				return 10.0f;// projectile speed for plasma weapon
		}
	}

	public void SetRifleUsable()
	{
		RifleUsable = true;
		SetWeapon(weaponRifle);
	}

	public void SetSniperUsable()
	{
		SniperUsable = true;
		SetWeapon(weaponSniper);
	}

	public void SetPlasmaUsable()
	{
		PlasmaUsable = true;
		SetWeapon(weaponPlasma);
	}

	public void SetWeapon(BaseWeapon weapon)
	{
		this.weapon = weapon;
		Debug.Log("Set weapon to " + weapon.GetWeaponClass().ToString());
	}

	public BaseWeapon GetWeapon() { return weapon; }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<PickupPlasma>() != null)
		{
			//pickup Plasma Gun Handler
			SetPlasmaUsable();
			Destroy(collision.gameObject);
		}

		if (collision.GetComponent<PickupRifle>() != null)
		{
			//pickup Rifle Handler
			SetRifleUsable();
			Destroy(collision.gameObject);
		}

		if (collision.GetComponent<PickupSniper>() != null)
		{
			//pickup Plasma Gun Handler
			SetSniperUsable();
			Destroy(collision.gameObject);
		}
	}

}
