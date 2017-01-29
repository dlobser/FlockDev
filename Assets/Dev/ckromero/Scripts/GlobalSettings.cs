using UnityEngine;
using System.Collections;
using Holojam.Tools;

public class GlobalSettings : MonoBehaviour {

//	public bool resetPlayer = false;


	//handled by quaternion
	public float graceTime = 10.0f;
	public float warnForSeconds = 10.0f;
	public float allowedSessionTime = 420.0f;
	public float timeLeftToDie = 20.0f;

	//handled by vector
	public float maxSpeedToSitStill = 2.0f;
	//open vector slots


	private Synchronizable synchronizable;
	private Quaternion quat;
	private Vector3 vec;

	void Start () {
		synchronizable = GetComponent<Synchronizable> ();	

	}
	
	// Update is called once per frame
	void Update () {
		quat.w = graceTime;
		quat.x = warnForSeconds;
		quat.y = allowedSessionTime;
		quat.z = timeLeftToDie;
		synchronizable.SetQuad(0,quat);

		vec.x = maxSpeedToSitStill;
		synchronizable.SetTriple(0,vec);

	}
}
