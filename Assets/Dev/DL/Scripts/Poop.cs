using UnityEngine;
using System.Collections;

public class Poop : MonoBehaviour {

	MeshRenderer rend;
	public float maxScale;
	bool enabled = false;
	public float scaleSpeed = .1f;
	Vector3 scalar = Vector3.zero;
	AudioSource aud;
	bool killed = false;


	// Use this for initialization
	void Awake () {
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
		Debug.Log (other.name);
		Material swapMaterial;
		swapMaterial = Resources.Load("swapMaterial") as Material;

		Renderer myRend = GetComponent<Renderer>();
		if (myRend != null){
			myRend.material = swapMaterial;
		}


		//		rend.enabled = false;
		//		enabled = false;
		//		aud.enabled = true;
		//		aud.Play ();
	}
}
