using UnityEngine;
using System.Collections;
using Vuforia;

public class AudioAnimationController : MonoBehaviour, ITrackableEventHandler
{
	private TrackableBehaviour mTrackableBehaviour;
	public GameObject model;

	void Start()
	{
		mTrackableBehaviour = this.transform.GetComponent<TrackableBehaviour> ();

		changeStateAudioAndAnimation (false);

		if (mTrackableBehaviour == null)
		{
			Debug.LogError ("no pudo hacer el init");
			return;
		}

		mTrackableBehaviour.RegisterTrackableEventHandler (this);
		//Debug.LogError ("RegisterTrackableEventHandler");
	}
	
	/*void OnGUI()
	{
		Screen.SetResolution (1024, 683, true);
		
		/*if (GUI.Button (new Rect(Screen.width*.04f,Screen.height*.80f,Screen.width*.15f,Screen.height*.15f),"start"))
		{
			//GetComponent<Animation>().Play("camaro");
			//string name =  GetComponent<Animator>().name;
			//GetComponent<Animator>().enabled = true;

			reloj.GetComponent<Animator>().enabled = true;
			reloj.GetComponent<Animator> ().ForceStateNormalizedTime (0.0f);

		}
		if (GUI.Button (new Rect(Screen.width*.20f,Screen.height*.80f,Screen.width*.15f,Screen.height*.15f),"stop"))
		{
			//GetComponent<Animation>().Stop();
			//GetComponent<Animator>().Stop();
			reloj.GetComponent<Animator>().enabled = false;
		}
	}*/
	
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			// Play audio when target is found
			//Debug.LogError("play animation");

			changeStateAudioAndAnimation(true);
		}
		else
		{
			// Stop audio when target is lost
			//Debug.LogError("stop animation");

			changeStateAudioAndAnimation(false);
		}
	}

	void changeStateAudioAndAnimation(bool active)
	{
		AudioHandler.changeState(model, active);
		AnimationHandler.changeState(model, active);
	}
}

