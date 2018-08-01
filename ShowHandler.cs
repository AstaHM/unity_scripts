using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHandler : MonoBehaviour {

	public static void Set(GameObject model, bool active)
	{
		model.SetActive (active);
	}
}
