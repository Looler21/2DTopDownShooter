using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : Health {


    public Image healthBar;
    public TextMeshProUGUI healthText;
    // Use this for initialization
    void Start()
    {
        healthText.text = (health / maxHealth * 100).ToString() + "%";
        healthBar.fillAmount = health / maxHealth;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public override void Damage(float damage)
    {
        health -= damage;
        float currentHp = health / maxHealth;
        healthText.text = (currentHp * 100).ToString() + "%";
        healthBar.fillAmount = currentHp;
    }
}
