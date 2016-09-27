using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holojam.Tools;

public class setActorDataSyncHeadsetFix : MonoBehaviour {

	public GameObject actor;
	// Use this for initialization
	void Start () {
		Invoke ("headSwitch", 1);
	}

	void headSwitch(){
		ActorDataSync pState = actor.GetComponent<ActorDataSync> ();

		GameObject am = GameObject.Find ("ActorManager");

		ActorManager actorManager = (ActorManager)am.GetComponent (typeof(ActorManager));

	 	Holojam.Tools.Actor[]	actors = actorManager.actors;
		foreach (Actor a in actors) {
			if (a.name.Contains ("Build")) {
				pState.currentActor =  a.name.Substring (0, a.name.IndexOf ("]") + 1);			
				Debug.Log ("curentActor is: " + pState.currentActor);
			}
		}

	}

}
