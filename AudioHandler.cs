using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour 
{
	public static void changeState(GameObject model, bool active)
	{
		AudioSource audio = model.GetComponent<AudioSource> ();

		if (audio == null) 
		{
			//Debug.LogError("AudioSource is null "+model.name);
			return;
		}

		if (active) 
		{
			audio.enabled = true;
			audio.time = 0;
			audio.Play ();
			//Debug.LogError("play AudioSource "+model.name);
			return;
		}

		audio.enabled = false;
		audio.Stop();
		//Debug.LogError("stop AudioSource "+model.name);
	}

}
