using UnityEngine;
using UnityEngine.iOS;
using System.Collections;

public class Gyroscope : MonoBehaviour 
{
	// Faces for 6 sides of the cube
	//private GameObject[] quads = new GameObject[6];

	// Textures for each quad, should be +X, +Y etc
	// with appropriate colors, red, green, blue, etc
	//public Texture[] labels;
	// Use this for initialization
	void Start()
	{
		// make camera solid colour and based at the origin
		/*GetComponent<Camera>().backgroundColor = new Color(49.0f / 255.0f, 77.0f / 255.0f, 121.0f / 255.0f);
		GetComponent<Camera>().transform.position = new Vector3(0, 0, 0);
		GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;

		// create the six quads forming the sides of a cube
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

		quads[0] = createQuad(quad, new Vector3(1,   0,   0), new Vector3(0,  90, 0), "plus x",
			new Color(0.90f, 0.10f, 0.10f, 1));
		quads[1] = createQuad(quad, new Vector3(0,   1,   0), new Vector3(-90,   0, 0), "plus y",
			new Color(0.10f, 0.90f, 0.10f, 1));
		quads[2] = createQuad(quad, new Vector3(0,   0,   1), new Vector3(0,   0, 0), "plus z",
			new Color(0.10f, 0.10f, 0.90f, 1));
		quads[3] = createQuad(quad, new Vector3(-1,   0,   0), new Vector3(0, -90, 0), "neg x",
			new Color(0.90f, 0.50f, 0.50f, 1));
		quads[4] = createQuad(quad, new Vector3(0,  -1,  0), new Vector3(90,   0,  0), "neg y",
			new Color(0.50f, 0.90f, 0.50f, 1));
		quads[5] = createQuad(quad, new Vector3(0,   0, -1), new Vector3(0, 180,  0), "neg z",
			new Color(0.50f, 0.50f, 0.90f, 1));

		GameObject.Destroy(quad);*/

		if (!Input.gyro.enabled || !SystemInfo.supportsGyroscope) 
			moveModels ();
	}

	// Update is called once per frame
	protected void Update()
	{
		if (Input.gyro.enabled && SystemInfo.supportsGyroscope) 
		{
			GyroModifyCamera ();
			return;
		}

		//if (SystemInfo.supportsAccelerometer)
		//	AccelerometerModifyCamera ();
			
	}

	// make a quad for one side of the cube
	GameObject createQuad(GameObject quad, Vector3 pos, Vector3 rot, string name, Color col)
	{
		Quaternion quat = Quaternion.Euler(rot);
		GameObject GO = Instantiate(quad, pos, quat) as GameObject;
		GO.name = name;
		//GO.GetComponent<Renderer>().material.color = col;
		//GO.GetComponent<Renderer>().material.mainTexture = t;
		GO.transform.localScale += new Vector3(0.25f, 0.25f, 0.25f);
		return GO;
	}

	/*
	protected void OnGUI()
	{
		GUI.skin.label.fontSize = Screen.width / 40;

		GUILayout.Label("Orientation: " + Screen.orientation);
		GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
		GUILayout.Label("iphone width/font: " + Screen.width + " : " + GUI.skin.label.fontSize);
	}*/

	/********************************************/

	// The Gyroscope is right-handed.  Unity is left handed.
	// Make the necessary change to the camera.
	void GyroModifyCamera()
	{
		transform.rotation = GyroToUnity(Input.gyro.attitude);
	}

	void AccelerometerModifyCamera()
	{
		transform.rotation = Quaternion.Euler (Input.acceleration.x, Input.acceleration.y, 0);
	}

	private static Quaternion GyroToUnity(Quaternion q)
	{
		return new Quaternion(q.x+0.5f, q.y, 0, -q.w);
	}

	private void moveModels()
	{
		GameObject obj = GameObject.Find ("ModelsController");

		if (obj == null)
			return;
		
		ModelsController modelsController = obj.GetComponent<ModelsController> ();

		if (modelsController == null)
			return;

		if (modelsController.models.Length < 1)
			return;
		
		modelsController.models [0].transform.Translate ( 3.8f, 0, 0);
		modelsController.models [1].transform.Translate ( 0, 0, 1);
		modelsController.models [2].transform.Translate ( 0, -1, 0);
	}
}
