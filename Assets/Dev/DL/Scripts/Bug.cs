using UnityEngine;
using System.Collections;
using Holojam.Tools;

public class Bug : Synchronizable{
	public Vector3 lastPosition { get; set; }
	public Vector3 origin { get; set; }
	public float scaleSpeed = .1f;

	Renderer[] renderers;
	BugManager bb;
	Vector3 scalar = Vector3.one;
	ActorDataSync actorDataSync;
//	Synchronizable synchronizable;
	Holojam.Tools.Viewer viewer;

	float scale;
	float initScale;
	int active = 1;
	bool doneScalingDown = false;
	bool doneScalingUp = false;

	void Awake(){
		//Okay to do in Awake
		bb = GameObject.Find("BugManager").GetComponent<BugManager>();
		viewer = GameObject.Find("Viewer").GetComponent<Holojam.Tools.Viewer>();
		renderers = GetComponentsInChildren<Renderer> ();
		scale = this.transform.localScale.x;
		initScale = scale;

		//prep for reporting on actor collisions
		actorDataSync = GameObject.Find ("ActorSynchronizableManager").GetComponent<ActorDataSync> ();

	}

	protected override void Sync(){
		if(sending){
			synchronizedVector3=transform.position;
			synchronizedQuaternion = new Quaternion (transform.localScale.x, transform.localScale.y, transform.localScale.z, 1);
			synchronizedInt = active;
		}
		else{
			transform.position=synchronizedVector3;
			transform.localScale = new Vector3 (synchronizedQuaternion.x, synchronizedQuaternion.y, synchronizedQuaternion.z);
			active = synchronizedInt;
		}
		int bugsEaten = actorDataSync.ActorBugsEaten(); 
		if (bugsEaten>5){
			Material newMaterial = (Material)Resources.Load ("testScreenShot 3");

			this.renderers [0].material = newMaterial;
			}
		
//		r.enabled = active==1;
//		this.renderers

//		Debug.Log (synchronizedString);



		ActivateDeactivateRenderer ();
	}

	void ActivateDeactivateRenderer(){
		foreach (Renderer r in renderers) {
			r.enabled = active==1;
		}
	}

	//Nothing below here executes on the client.
	void OnTriggerEnter(Collider c){
		
		if(!sending || active!=1)return;
		Holojam.Tools.Actor a = c.GetComponent<Holojam.Tools.Actor>();
		Holojam.Tools.Viewer v =  c.GetComponent<Holojam.Tools.Viewer>();
		if(a!=null){

//			Debug.Log (c.name + " collided with Bug " + this.name);
		
			// add to bugsEaten for the colliding actor!!!!
			if (Holojam.Utility.IsMasterPC ()) {
				if (Time.time > 1) {
					if (c.name != null && c.name!="") {
//						Debug.Log (this.name + " collided with " + c.transform.parent.name + ", " + this.name + " is not active now.");
						actorDataSync.UpdateActor( c.name,this.name,1);
					}
				}
			} 

			//This handles particle placement
			bb.SendMessage("ProcessCollision", this); //Callback
			StartCoroutine(DisableThis());
		}
	}


	IEnumerator DisableThis(){
		while(!doneScalingDown){
			if (scale > .001f) {
				scale = Mathf.MoveTowards(scale, 0f, scaleSpeed);
				scalar.Set (scale, scale, scale);
				this.transform.localScale = scalar;
				yield return null;
			} else
				doneScalingDown = true;
		}
		active = 0;
		doneScalingDown = false;
		yield return new WaitForSeconds(bb.disableTime);
		active = 1;
		while (!doneScalingUp) {
			if (scale < initScale) {
				scale = Mathf.MoveTowards (scale, initScale, scaleSpeed);
				scalar.Set (scale, scale, scale);
				this.transform.localScale = scalar;
				yield return null;
			} else
				doneScalingUp = true;
		}
		doneScalingUp = false;
		scale = initScale;


	}
}