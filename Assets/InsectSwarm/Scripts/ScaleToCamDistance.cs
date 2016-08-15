using UnityEngine;
using System.Collections;

public class ScaleToCamDistance : MonoBehaviour {

	public float nearDistance;
	public float farDistance;
	public float nearScale;
	public float farScale;

	public float scale;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance (this.transform.position, Camera.main.transform.position);
		scale = Mathf.Max(farScale, Mathf.Min(nearScale, map (dist, nearDistance, farDistance, nearScale, farScale)));

	}

	float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}
