using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Also can rely on PlayerInvulnerability, just like PlayerDamage (don't know, but it seems safer to just keep it in both places just in case)
public class PlayerHealth : Health {
	
    public Image healthBar;
    public TextMeshProUGUI healthText;
	private PlayerInvulnerability invulnScript;

    // Use this for initialization
    void Start()
    {
		if (healthText != null)
			healthText.text = (health / maxHealth * 100).ToString() + "%";
		else
			Debug.LogError("No healthText defined for player: " + gameObject.name);

		if (healthBar != null)
			healthBar.fillAmount = health / maxHealth;
		else
			Debug.LogError("No healthBar Image defined for player: " + gameObject.name);
		
		invulnScript = GetComponent<PlayerInvulnerability>();
		if(invulnScript == null)
			Debug.LogWarning("No player invulnerability script attached to " + gameObject.name + ". Will not check for it in PlayerHealth. Warning: Player may get frustrated by getting spammed attacked lol");
	}

    // Update is called once per frame
    void Update () {
		
	}

    public override void Damage(float damage)
    {
		// If player is invulnerable, just do nothing
		if (invulnScript != null && invulnScript.isInvulnerable)
			return;
		
		health -= damage;

		float currentHp = health / maxHealth;

		if (healthText != null)
			healthText.text = (currentHp * 100).ToString() + "%";

		if (healthBar != null)
			healthBar.fillAmount = currentHp;
		
		if (health < 0)
			Die();
	}
}
