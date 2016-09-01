using UnityEngine;
using System.Collections;

public class Fly : MonoBehaviour {

	public SpriteAimerComplex spriteAimer;
	private Vector3 prevPosition;
	public Vector3 target;
	public Vector3 origin;
	public bool lerpRotation = false;
	public float rotationInterpolationSpeed = .1f;

//	public bool lerpPosition = false;
//	public float positionInterpolationSpeed = .1f;

	Quaternion prev = Quaternion.identity;
//	Vector3 prevPosition = Vector3.zero;

	public bool active = false;
	MeshRenderer meshRend;

	public float deadForHowLong;
	private float deathTimer;
	public float scaleForDeath = 1;

	public GameObject splat;


	void Start(){
		meshRend = spriteAimer.gameObject.GetComponent<MeshRenderer> ();
		meshRend.enabled = true;
	}

	public void UpdatePosition(float fishScale){

//		if (lerpPosition){
//			Vector3 pr = target;
//			Vector3	next = this.transform.position;
//			this.transform.position = Vector3.Lerp (pr, next, positionInterpolationSpeed);
//		}
//		else
			this.transform.position = target;

		if(lerpRotation)
			prev = this.transform.rotation;
		
		this.transform.LookAt (prevPosition);

		if(lerpRotation){
			Quaternion next = this.transform.rotation;
			this.transform.rotation = Quaternion.Lerp(prev,next,rotationInterpolationSpeed);
		}

		this.transform.localScale = new Vector3 (fishScale, fishScale, fishScale);
		this.spriteAimer.UpdatePosition ();
		prevPosition = target;

		if(!active)
			updateDeathTimer ();
		if (scaleForDeath < 1) {
			scaleForDeath += Time.deltaTime;
		}
	}

	void updateDeathTimer(){
		if (deathTimer < deadForHowLong) {
			deathTimer += Time.deltaTime;
			scaleForDeath = 0;
		} else {
			meshRend.enabled = true;
			active = true;
		}
	}

	void OnCollisionEnter(Collision other) {
		meshRend.enabled = false;
		active = false;
		deathTimer = 0;
		GameObject sp = Instantiate (splat,this.transform.position,Quaternion.identity) as GameObject;
		sp.transform.SetParent (this.transform);
		print ("Splat");
	}

}
