using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScenes : MonoBehaviour 
{
	public void LoadScene(string name)
	{
		SceneManager.LoadScene(name);
	}

	void OnGUI() 
	{
		Screen.SetResolution (1136, 640, true);
	}
}
