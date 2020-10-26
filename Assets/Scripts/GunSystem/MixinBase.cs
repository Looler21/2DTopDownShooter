using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixinBase : MonoBehaviour
{
    public virtual void Action()
	{

	}

	public virtual bool Check()
	{
		return true;
	}
}
