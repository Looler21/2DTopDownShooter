using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class CheckCooldown : MixinBase
{
	public float cooldownTimer;
	float cooldownTime;
	bool isCool = true;

	public override bool Check()
	{
		return isCool;
	}

	public override void Action()
	{
		isCool = false;
		cooldownTime = 0.0f;
	}

	private void Update()
	{
		if (!isCool)
		{
			cooldownTime += Time.deltaTime;
			if(cooldownTime > cooldownTimer)
			{
				isCool = true;
			}
		}
	}
}
