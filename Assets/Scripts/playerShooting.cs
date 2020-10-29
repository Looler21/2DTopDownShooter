using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShooting : MonoBehaviour {
	[Header("Shooting Number Variables")]
	public CallMixinActions fireWeapon; // reference to the mixinscript for the gun attached to player
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
		{
			fireWeapon.CallActions();
		}
	}
}
