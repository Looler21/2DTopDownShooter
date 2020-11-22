using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompAttack : BossAttack
{
	Animator stompAnimation;
	[SerializeField] private playerMovement playerPos;
	private Transform playerTransform;
	public float stompDistance = 4f;
	public float stompForce = 10f;
	public float stompDamage = 20f;
	public float stompLength = 2f;  // in seconds

	private Health playerHealth;

	public override void Do()
	{
		attackIsActive = true;
	}
	
	void Start()
    {
		attackIsActive = false;

		//find a player in the hierarchy
		if (playerPos == null)
		{
			playerPos = FindObjectOfType<playerMovement>();
			if(playerPos == null)
				Debug.LogWarning("No playerMovement found. BossEnemy \"" + transform.root.name + " cannot use its StompAttack.");
		}

		playerHealth = playerTransform.GetComponentInChildren<Health>();
		if (playerHealth == null)
			Debug.LogWarning("No player Health found. Stomp attack will not to damage.");
	}
	
    void FixedUpdate()
    {
		if(attackIsActive)
		{
			//TODO - Do spinny animation
			StartCoroutine("Stomp");
		}
	}

	IEnumerator Stomp()
	{
		GameObject boss = transform.root.gameObject;

		//temporary animation thingy
		float scale = Mathf.Lerp(1f, 1.5f, 5f);
		boss.GetComponent<Transform>().localScale = new Vector3(scale, scale, 1f);

		// Wait for animation to finish
		yield return new WaitForSeconds(stompLength);

		//if distance to player still in range, push them back
		playerTransform = playerPos.GetComponentInChildren<playerShooting>().transform;
		float distance = Vector3.Distance(playerTransform.position, this.transform.root.position);

		//Deal damage to player
		if(playerHealth != null)
			playerHealth.Damage(stompDamage);

		//Push player back if they're still in range && the attack hasn't already ended
		if (distance <= stompDistance && attackIsActive)
		{
			GameObject player = playerPos.transform.root.gameObject;
			Vector2 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y);
			Vector3 dirBackwards = playerPosition - (Vector2)transform.position;  //boss vector3 + player vector3
			playerPos.AddForce(dirBackwards.normalized * stompForce);
		}

		//reset boss state
		boss.GetComponent<BossEnemy>().stomping = false;

		//reset boss scale
		boss.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);

		attackIsActive = false;
	}

	
}
