using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShooting : MonoBehaviour {
	[Header("Shooting Number Variables")]
	public float shootingDistance = 300.0f; //how far the bullet should travel (detect enemy)
	public Transform firingPoint; //Where the bullet will travel/check from (realistic gun)
	public CallMixinActions fireWeapon; // reference to the mixinscript for the gun attached to player
	public GameObject gunflare;
	public KeyCode fire1; //buttonn to fire guns
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
		{
			fireWeapon.CallActions();
			Instantiate(gunflare, firingPoint.position, transform.rotation);
		}
	}
}
