using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class playerShooting : MonoBehaviour {

	private BaseWeapon weapon;
	private BaseWeapon weaponPistol;
	private BaseWeapon weaponRifle;
	private BaseWeapon weaponSniper;
	private BaseWeapon weaponPlasma;

	private SpriteRenderer sr;
	private AudioSource au; // Audio Source for gun sound

	public TextMeshProUGUI ammoText;

	public Transform firingPoint; //Where the bullet will travel/check from (realistic gun)
	private Vector2 mousePosition; //position of mouse
	private Vector2 firingOrigin;
	public GameObject muzzleFlash;

	private bool RifleUsable;
	private bool SniperUsable;
	private bool PlasmaUsable;

	public float timeSinceLastFire = 0;

	public Sprite pistolChar;
	public Sprite ArChar;
	public Sprite SniperChar;

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
		weapon = weaponPistol;
		sr = GetComponent<SpriteRenderer>();
		au = GetComponent<AudioSource>();
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

					if(weapon.Shoot(mousePosition, firingOrigin, timeSinceLastFire, weapon.GetShootDistance()))
					{
						GameObject thisThing = (GameObject)Instantiate(muzzleFlash, firingOrigin, Quaternion.identity);
						timeSinceLastFire = 0;
						Destroy(thisThing, .2f);
						au.PlayOneShot(au.clip);
					}
					
				}else if(BaseWeapon.getShootType(weapon) == BaseWeapon.ShootType.projectile)
				{
					firingOrigin = new Vector2(firingPoint.position.x, firingPoint.position.y);
					mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
						Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

					GameObject bullet = (GameObject)Instantiate(getWeaponProjectilePrefab(weapon), firingOrigin, Quaternion.identity);
					bullet.GetComponent<Rigidbody2D>().velocity = (firingOrigin - new Vector2(transform.position.x,transform.position.y)).normalized * getWeaponProjectileSpeed(weapon);
				}
			}
			else
			{
				Debug.Log("Weapon null");
			}
		}

		if(ammoText != null)
			ammoText.text = weapon.GetAmmo() + "/" + weapon.GetMaxAmmo();
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
		switch (weapon.GetWeaponClass())
		{
			case BaseWeapon.WeaponClass.Rifle:
				sr.sprite = ArChar;
				break;
			case BaseWeapon.WeaponClass.Sniper:
				sr.sprite = SniperChar;
				break;
			case BaseWeapon.WeaponClass.Pistol:
				sr.sprite = pistolChar;
				break;
			default:
				break;
		}
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
