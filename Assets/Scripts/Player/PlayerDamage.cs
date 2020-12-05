using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Calls PlayerHealth.Damage depending on what the player collides with
// Takes into account if the player is invulnerable with a separate script: PlayerInvulnerability
public class PlayerDamage : MonoBehaviour
{
	private PlayerHealth player;
	private PlayerInvulnerability invulnScript;

	private void Start()
	{
		player = GetComponent<PlayerHealth>();
		if(player == null)
		{
			Debug.LogError("No PlayerHealth found on object: " + gameObject.name + ". Damage from collisions will not be applied.");
		}

		invulnScript = GetComponent<PlayerInvulnerability>();
		if(invulnScript == null)
			Debug.LogWarning("No player invulnerability script attached to " + gameObject.name + ". Will not check for it in PlayerDamage. Warning: Player may get frustrated by getting spammed attacked lol");
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Bullet projectile collision
		if(collision.gameObject.CompareTag("Bullet"))
		{
			// Always destroy the bullets when collide with player
			// (cuz bullets currently on Rigidbodies, and they'll bounce around otherwise)
			Destroy(collision.gameObject);
			
			// Check if player not already invulnerable
			if (invulnScript != null && invulnScript.isInvulnerable)
				return;
			
			// Decide if bullet can give damage to player
			float bulletDmg = collision.gameObject.GetComponent<Bullet>().damageValue;
			if (bulletDmg > 0)
			{
				player.Damage(bulletDmg);
				//Debug.Log("Player took " + bulletDmg + " damage.");
			}
			else
				Debug.LogWarning("Bullet damage is <= 0 for: " + collision.gameObject.name);

			// When damaged, give player brief invuln window
			if (invulnScript != null && !invulnScript.isInvulnerable)
				invulnScript.StartCoroutine("BriefInvulnerability");
		}
	}
}
