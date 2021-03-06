﻿using UnityEngine;
using System.Collections;
using Holojam.Tools;

public class Fly : MonoBehaviour {

	public Synchronizable synchronizable;
	public GameObject splat;
	public SpriteAimerComplex spriteAimer;

	public Vector3 target;
	public Vector3 origin;
	public float rotationInterpolationSpeed = .1f;
	public float scaleForDeath = 1;
	public bool lerpRotation = false;
	public bool active = false;
	public float deadForHowLong;

//	ActorData actorData;
	private Vector3 prevPosition;
	Quaternion prev = Quaternion.identity;
	MeshRenderer meshRend;
	private float deathTimer;


//	public bool lerpPosition = false;
//	public float positionInterpolationSpeed = .1f;
//	Vector3 prevPosition = Vector3.zero;

	void Start(){

		synchronizable = GetComponent<Synchronizable> ();
//		actorData = GetComponent<ActorData> (); 


//		if (!Holojam.Utility.IsMasterPC ()) { 
//			Destroy (this);
//		}
			
		meshRend = spriteAimer.gameObject.GetComponent<MeshRenderer> ();
		meshRend.enabled = true;
	}

	public void UpdatePosition(float fishScale){
//
//		VRDebug.print (this.name + "synchronizable.synchronizedInt=" + synchronizable.synchronizedInt + "\n");
//		Debug.Log (this.name + "synchronizable.synchronizedInt=" + synchronizable.synchronizedInt);

//		if (lerpPosition){
//			Vector3 pr = target;
//			Vector3	next = this.transform.position;
//			this.transform.position = Vector3.Lerp (pr, next, positionInterpolationSpeed);
//		}
//		else
		if (Holojam.Utility.IsMasterPC ()) { 
			this.transform.position = target;

			if (lerpRotation)
				prev = this.transform.rotation;
		
			this.transform.LookAt (prevPosition);

			if (lerpRotation) {
				Quaternion next = this.transform.rotation;
				this.transform.rotation = Quaternion.Lerp (prev, next, rotationInterpolationSpeed);
			}

			this.transform.localScale = new Vector3 (fishScale, fishScale, fishScale);
			this.spriteAimer.UpdatePosition ();
			prevPosition = target;

			if (!active)
				updateDeathTimer ();
			if (scaleForDeath < 1) {
				scaleForDeath += Time.deltaTime;
			}
		} else {
			//clients use this to determine if their sprites should be enabled. 
			if (synchronizable.synchronizedInt == 1) {
				meshRend.enabled = false;
				active = false;
			} else {
				meshRend.enabled = true;
				active = true;
			}

		}

//			Debug.Log ("current actor bugs eaten is: " + actorData.ActorBugsEaten());

	}

	void updateDeathTimer(){
		if (deathTimer < deadForHowLong) {
			deathTimer += Time.deltaTime;
			scaleForDeath = 0;
		} else {
			Debug.Log (this.name + " is active again.");
			meshRend.enabled = true;
			active = true;
			synchronizable.synchronizedInt = 0;
		}
	}
	void OnTriggerEnter(Collider other) {

		if (Holojam.Utility.IsMasterPC ()) {
			if (Time.time > 1) {
				if (other.transform.parent.parent.name != null && other.transform.parent.parent.name == "ActorManager") {

					Debug.Log ("Fly script: " + this.name + " collided with " + other.transform.parent.name + ", " + this.name + " is not active now.");

//					actorData.UpdateActor(synchronizable, other.transform.parent.name,this.name,1);

					meshRend.enabled = false;
					active = false;
					deathTimer = 0;
					synchronizable.synchronizedInt = 1;

					//				GameObject obj = Instantiate (splat, this.transform.position, Quaternion.identity) as GameObject;
					//				obj.transform.SetParent (this.transform);
			
				}
			}
		}
			
	}

}

//Holojam.Utility.IsMasterPC()
//Destroy(this)
//Destroy(gameObject)
