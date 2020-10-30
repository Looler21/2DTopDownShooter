using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon { 
    
	public enum WeaponClass
	{
		Pistol,
		Rifle,
		Sniper,
		Plasma
	}

	public enum ShootType
	{
		projectile,
		hitscan
	}

	private WeaponClass weaponClass;
	private ShootType shootType;
	private int ammo;
	public int maxAmmo;

	public BaseWeapon(WeaponClass weaponClass,ShootType shootType )
	{
		this.shootType = shootType;
		this.weaponClass = weaponClass;
		ammo = GetMaxAmmo();
	}

	public WeaponClass GetWeaponClass(){ return weaponClass;}

	private int GetMaxAmmo(){return maxAmmo;}

	public bool checkIfAvailableAmmo()
	{
		if(ammo > 0)
		{
			ammo--;
			return true;
		}
		else
		{
			return false;
		}
	}

	public void Reload()
	{
		ammo = GetMaxAmmo();
	}

	public float GetFireRate()
	{
		switch (weaponClass)
		{
			default:
			case WeaponClass.Pistol:
				return .25f;
			case WeaponClass.Rifle:
				return .1f;
			case WeaponClass.Sniper:
				return .75f;
			case WeaponClass.Plasma:
				return .5f;
		}
	}

	public float GetDamageMultiplier()
	{
		switch (weaponClass)
		{
			default:
			case WeaponClass.Pistol:
				return 1.2f;
			case WeaponClass.Rifle:
				return .7f;
			case WeaponClass.Sniper:
				return 4f;
			case WeaponClass.Plasma:
				return 3.2f;
		}
	}

	public void Shoot()
	{
		if(shootType == ShootType.hitscan)
		{

		}else if(shootType == ShootType.projectile)
		{

		}
	}
}
