using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
	public Transform target;
	private BaseWeapon weaponPistol;
	private float timeSinceLastFire;

	// Start is called before the first frame update

	private void Awake()
	{
		weaponPistol = new BaseWeapon(BaseWeapon.WeaponClass.Pistol, BaseWeapon.ShootType.projectile, 5);
		timeSinceLastFire = 0f;
	}
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		timeSinceLastFire += Time.deltaTime;

		//each frame, check for enemies in a vision cone?
		
			//if enemy in cone, shoot
		
		//otherwise

		//wait this should be handled by Enemy.cs? idk
    }

	public void Shoot(Vector2 target, Vector2 firingOrigin)
	{
		weaponPistol.Shoot(target, firingOrigin, timeSinceLastFire);	//also probably a bullet projectile need to pass in for projectile? diff enemies can have different bullets
	}
}
