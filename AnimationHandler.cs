using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour {

	public static void changeState(GameObject model, bool active)
	{
		Animator animator = model.GetComponent<Animator> ();

		if (animator == null) 
		{
			return;
		}

		if (active) 
		{
			animator.enabled = true;
			animator.ForceStateNormalizedTime (0.0f);
			return;
		}

		animator.enabled = false;
	}
}
