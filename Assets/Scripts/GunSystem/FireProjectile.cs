using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MixinBase
{
	public GameObject projectilePrefab;
	public Transform firePosition;
	public GameObject player;
	public float bulletSpeed;
	public GameObject gunFlare;

	public override void Action()
	{
		GameObject bulletShoot = (GameObject)Instantiate(projectilePrefab, firePosition.position, firePosition.rotation);
		bulletShoot.GetComponent<Rigidbody2D>().velocity = (transform.position - player.transform.position).normalized * bulletSpeed;
		Instantiate(gunFlare, transform.position, Quaternion.identity);
	}
}
