using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShooting : MonoBehaviour {

	public BaseWeapon weapon;
	private BaseWeapon weaponPistol;
	private BaseWeapon weaponRifle;
	private BaseWeapon weaponSniper;
	private BaseWeapon weaponPlasma;

	private bool RifleUsable;
	private bool SniperUsable;
	private bool PlasmaUsable;

	private void Awake()
	{
		weaponPistol = new BaseWeapon(BaseWeapon.WeaponClass.Pistol, BaseWeapon.ShootType.hitscan);
		weaponRifle = new BaseWeapon(BaseWeapon.WeaponClass.Rifle, BaseWeapon.ShootType.hitscan);
		weaponSniper = new BaseWeapon(BaseWeapon.WeaponClass.Sniper, BaseWeapon.ShootType.hitscan);
		weaponPlasma = new BaseWeapon(BaseWeapon.WeaponClass.Plasma, BaseWeapon.ShootType.projectile);
	}

	// Update is called once per frame
	void Update () {
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
