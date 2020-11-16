using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BossAttack : MonoBehaviour
{
	public bool attackIsActive;

	public abstract void Do();
	public virtual bool isActive()
	{
		return attackIsActive;
	}
}
