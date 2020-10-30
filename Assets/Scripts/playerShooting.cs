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

	private Health playerHealth;

	public Transform firingPoint; //Where the bullet will travel/check from (realistic gun)
	private Vector2 mousePosition; //position of mouse
	private Vector2 firingOrigin;

	private bool RifleUsable;
	private bool SniperUsable;
	private bool PlasmaUsable;

	public float timeSinceLastFire = 0;

	private void Awake()
	{
		weaponPistol = new BaseWeapon(BaseWeapon.WeaponClass.Pistol, BaseWeapon.ShootType.hitscan,7);
		weaponRifle = new BaseWeapon(BaseWeapon.WeaponClass.Rifle, BaseWeapon.ShootType.hitscan,30);
		weaponSniper = new BaseWeapon(BaseWeapon.WeaponClass.Sniper, BaseWeapon.ShootType.hitscan,2);
		weaponPlasma = new BaseWeapon(BaseWeapon.WeaponClass.Plasma, BaseWeapon.ShootType.projectile,5);
		playerHealth = new Health(10.0f);
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
		}

		if (Input.GetKeyDown(KeyCode.Alpha3) && SniperUsable)
		{
			SetWeapon(weaponSniper);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) && PlasmaUsable)
		{
			SetWeapon(weaponPlasma);
		}
		
		if (Input.GetButtonDown("Fire1"))
		{
			if (weapon != null)
			{
				firingOrigin = new Vector2(firingPoint.position.x, firingPoint.position.y);
				mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
					Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
				weapon.Shoot(mousePosition,firingOrigin, timeSinceLastFire);
			}
			else
			{
				Debug.Log("Weapon null");
			}
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
