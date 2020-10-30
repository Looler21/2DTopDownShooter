﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

	public BaseWeapon(WeaponClass weaponClass,ShootType shootType, int maxAmmo )
	{
		this.shootType = shootType;
		this.weaponClass = weaponClass;
		ammo = maxAmmo;
		this.maxAmmo = maxAmmo;
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
				return .05f;
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

	public float GetShootDistance() // ONLY FOR HITSCAN WEAPONS
	{
		switch (weaponClass)
		{
			default:
			case WeaponClass.Pistol:
				return 300.0f;
			case WeaponClass.Rifle:
				return 200.0f;
			case WeaponClass.Sniper:
				return 1000.0f;

		}
	}

	public bool Shoot(Vector2 mousePosition, Vector2 firingOrigin, float timeSinceLastFire ,float shootingDistance = 500f)
	{
		Debug.Log("Tried to Shoot");
		if (!checkIfAvailableAmmo()) {
			Reload();
			Debug.Log("Check ammo failed");
			return false;
		}

		if(!(GetFireRate() <= timeSinceLastFire)) {
			Debug.Log("fire rate failed");
			return false; 
		}
		
		if(shootType == ShootType.hitscan)
		{
			
			RaycastHit2D hit = Physics2D.Raycast(firingOrigin, mousePosition - firingOrigin, shootingDistance);

			Debug.DrawRay(firingOrigin, mousePosition - firingOrigin, Color.red, shootingDistance);
			
			if(hit && hit.collider.CompareTag("Enemy"))
			{
				hit.transform.GetComponent<Health>().Damage(1 * GetDamageMultiplier());
			}
			Debug.Log("ShootHitScan");
			return true;
		}
		else if(shootType == ShootType.projectile)
		{
			return true;
		}
		else
		{
			Debug.Log("gun missing shoot type");
			return false;
		}
	}
}