using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro : MonoBehaviour 
{
	Gyroscope gyro;
	bool gyroSupported;
	Quaternion rotFix;

	float startY;
	// Use this for initialization
	void Start () 
	{
		gyroSupported = SystemInfo.supportsGyroscope;

		if (gyroSupported) 
		{
			//gyro = Input.gyro;
			//gyro.enabled = true;

			rotFix = new Quaternion (0, 0, 0, 0);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gyroSupported && startY == 0) 
		{
		}
		
	}

	void resetGyroRatation()
	{
		startY = transform.eulerAngles.y;
	}
}
