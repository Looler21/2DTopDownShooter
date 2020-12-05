using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimultaneousAttacks : BossAttack
{
	public BossAttack[] attacks;

    // Update is called once per frame
    void FixedUpdate()
    {
		//check if each attack has finished
        if(attackIsActive)
		{
			foreach (BossAttack attack in attacks)
			{
				if (attack.attackIsActive)
					return;

				attackIsActive = false;
			}
		}
    }

	public override void Do()
	{
		foreach(BossAttack attack in attacks)
			attack.Do();

		attackIsActive = true;
	}

}
