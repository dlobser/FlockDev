using UnityEngine;
using System.Collections;

public class scaleBackUp : MonoBehaviour {

	float initialScale;
	SphereCollider sphere;
	public float speed = .5f;
	// Use this for initialization
	void Start () {
		sphere = GetComponent<SphereCollider> ();
		initialScale = sphere.radius;
	}
	
	// Update is called once per frame
	void Update () {
		if (sphere.radius < initialScale) {
			sphere.radius += Time.deltaTime * speed;
		}
	}
}
