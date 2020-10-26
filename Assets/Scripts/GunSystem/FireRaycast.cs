using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRaycast : MixinBase
{
	public Transform firingPoint; //Where the bullet will travel/check from (realistic gun)
	private Vector2 mousePosition; //position of mouse
	private Vector2 firingOrigin; //the (x,y) coordinates of the firingpoint gameObject
	private RaycastHit2D hit; //the line the bullet will follow
	public float shootingDistance = 300.0f; //how far the bullet should travel (detect enemy)

	public override void Action()
	{
		firingOrigin = new Vector2(firingPoint.position.x, firingPoint.position.y);
		mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
			Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		hit = Physics2D.Raycast(firingOrigin, mousePosition - firingOrigin, shootingDistance);
		Debug.DrawRay(firingOrigin, mousePosition - firingOrigin, Color.red,shootingDistance);

		if (hit)
		{
			Debug.Log("Hit Something");
			Debug.Log(hit.transform.name);
		}
	}
}
