using UnityEngine;
using System.Collections;
using Holojam.Tools;

public class Bug : Synchronizable{
	BugManager bb;
	int active = 1;

	public Vector3 lastPosition { get; set; }
	public Vector3 origin { get; set; }

	bool doneScalingDown = false;
	bool doneScalingUp = false;
	public float scaleSpeed = .1f;
	float scale;
	float initScale;
	Vector3 scalar = Vector3.one;

	Renderer[] renderers;

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

//		r.enabled = active==1;

		ActivateDeactivateRenderer ();
	}

	void ActivateDeactivateRenderer(){
		foreach (Renderer r in renderers) {
			r.enabled = active==1;
		}
	}
	void Awake(){
		//Okay to do in Awake
		bb = GameObject.Find("BugManager").GetComponent<BugManager>();
		renderers = GetComponentsInChildren<Renderer> ();
		scale = this.transform.localScale.x;
		initScale = scale;
	}

	//Nothing below here executes on the client.
	void OnTriggerEnter(Collider c){
		
		if(!sending || active!=1)return;
		Holojam.Tools.Actor a = c.GetComponent<Holojam.Tools.Actor>();
		Holojam.Tools.Viewer v =  c.GetComponent<Holojam.Tools.Viewer>();
		if(a!=null||v!=null){
			Debug.Log ("hi");
			bb.SendMessage("ProcessCollision",this); //Callback
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
			Debug.Log (scale);
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