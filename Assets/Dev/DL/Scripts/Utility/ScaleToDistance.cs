using UnityEngine;
using System.Collections;

public class ScaleToDistance : MonoBehaviour {

	public float nearDistance;
	public float farDistance;
	public float nearScale;
	public float farScale;
	DistanceToCamera dist;
	public float scale;
	Vector3 Scalar = Vector3.one;
	// Use this for initialization
	void Start () {
		dist = GetComponent<DistanceToCamera> ();
	}
	
	// Update is called once per frame
	void Update () {
		float distance = dist.distance;
		scale = Mathf.Max(farScale, Mathf.Min(nearScale, map (distance, nearDistance, farDistance, nearScale, farScale)));
		Scalar.Set (scale, scale, scale);
		this.transform.localScale = Scalar;
	}

	float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}
