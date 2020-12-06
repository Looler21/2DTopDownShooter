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
			//assume not attacking at the moment
			attackIsActive = false;

			//check if any attack is still running, if so, then attackIsActive = true
			foreach (BossAttack attack in attacks)
			{
				attackIsActive = attackIsActive || attack.isActive();
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
