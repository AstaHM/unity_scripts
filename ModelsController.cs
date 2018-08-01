using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelsController : MonoBehaviour
{
	public GameObject[] models;
	GameObject currentModel;

	// Use this for initialization
	void Start () 
	{
		showModelAt(0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void showModelAt(int index)
	{
		if (models.Length < 1)
			return;

		if (models.Length <= index)
			return;

		GameObject newModel = models [index];

		if(currentModel != null)
			setProperties (currentModel, false);

		setProperties (newModel, true);
		currentModel = newModel;
	}

	void setProperties(GameObject model, bool state)//funcion de la animaion y de l audio
	{
		ShowHandler.Set (model, state);
		AudioHandler.changeState (model, state);
		AnimationHandler.changeState(model, state);
	}
}