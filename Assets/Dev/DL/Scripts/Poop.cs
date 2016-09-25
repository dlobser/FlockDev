using UnityEngine;
using System.Collections;

public class Poop : MonoBehaviour {

	public float maxScale;
	public float scaleSpeed = .1f;

	MeshRenderer rend;
	Vector3 scalar = Vector3.zero;
	AudioSource aud;
	Material swapMaterial;
	new bool enabled = false;
	bool killed = false;

	// Use this for initialization
	void Awake () {
		swapMaterial=Resources.Load("swapMaterial") as Material;
		rend = GetComponent<MeshRenderer> ();
		aud = GetComponent<AudioSource> ();
		aud.enabled = false;
		aud.pitch = Random.Range (.5f, 1.5f);
	}

	public void makeAlive(Vector3 pos){
		if (!enabled) {
			enabled = true;
			killed = false;
			rend.enabled = true;
			this.transform.position = pos;
			this.transform.localScale = Vector3.zero;
		}
	}

	public void Kill(){
		if (!killed) {
			this.transform.position = Vector3.up * Random.value * 1e6f;
			killed = true;
		}
	}
	// Update is called once per frame
	public void Animate () {
		if (enabled) {
			if (this.transform.localScale.x < maxScale) {
				scalar.Set (this.transform.localScale.x + scaleSpeed, this.transform.localScale.y + scaleSpeed, this.transform.localScale.z + scaleSpeed);
				this.transform.localScale = scalar;

			}
		} else if (!enabled){
			if (!aud.isPlaying) {
				Kill ();
			}
		}
		this.transform.Rotate (0, 3, 0);
	}

	void OnTriggerEnter(Collider other) {
		

		Debug.Log (this.name + " collided with " + other.name + " at: " + Time.time);

		//TODO: Is this really necessary? Need to aoivd checking this every time
		if (Time.time > 1) {
			Renderer myRend = GetComponent<Renderer> ();
			if (myRend != null) {
				myRend.material = swapMaterial;
			}
		}

		//		rend.enabled = false;
		//		enabled = false;
		//		aud.enabled = true;
		//		aud.Play ();
	}
}
