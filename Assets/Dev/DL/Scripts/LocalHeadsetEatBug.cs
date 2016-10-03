using UnityEngine;
using System.Collections;

public class LocalHeadsetEatBug : MonoBehaviour {

	LevelHandler handler;
	Holojam.Tools.Actor actor;

	void Start(){
		handler = GameObject.Find ("LevelHandler").gameObject.GetComponent<LevelHandler> ();
		actor = this.GetComponent<Holojam.Tools.Actor> ();
	}

	void OnTriggerEnter(Collider c){
		Bug b = c.GetComponent<Bug>();
		if(b!=null && actor.isBuild){
//			this.GetComponent<SphereCollider> ().enabled = false;
			handler.EatBug ();
		}
	}
}
