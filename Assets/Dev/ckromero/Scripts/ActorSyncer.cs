using System;
using System.Collections;
using System.Collections.Generic;
using Holojam.Tools;
using UnityEngine;

public class ActorSyncer : MonoBehaviour
{
	public Synchronizable synchronizable;

	// Use this for initialization
	void Start ()
	{
		synchronizable = GetComponent<Synchronizable> ();
		synchronizable.label = "ActorSyncer";
	}

	public void SendSyncStringData (string data)
	{ 
		if (data != null && data != "") {  
			ActorSetJSON capturedASJ = new ActorSetJSON ();
			ActorSetJSON revisedASJ = new ActorSetJSON ();

			//search the current synchronized string and see if any data is associated with the current actor (data).
			if (synchronizable.synchronizedString != null && synchronizable.synchronizedString != "") { 
				capturedASJ = JsonUtility.FromJson<ActorSetJSON> (synchronizable.synchronizedString);
				int casjLength = capturedASJ.actors.Length;
				bool foundExistingActor=false;
				for (int i = 0; i < casjLength; i++) {
					if (capturedASJ.actors [i].actorIndex == data) {
						capturedASJ.actors [i].bugsEaten++;	
						revisedASJ = capturedASJ;
						foundExistingActor = true;
						break;
					}
				}

				//data is already stored for other actors but not for this one yet. 
				if (!foundExistingActor) {
					ActorJson naj = new ActorJson ();
					naj.actorIndex = data;
					naj.bugsEaten = 1;

					List<ActorJson> ls = new List<ActorJson> ();
						
					ls.Add (naj);
					int ct = capturedASJ.actors.Length;

					for (int i = 0; i < ct; i++) {
						ls.Add (capturedASJ.actors [i]);
					}

					//sort by key?
//					capturedASJ.actors[capturedASJ.actors.Length]=ls[0];

					revisedASJ.actors = ls.ToArray ();

				}
			} else { 
				//initiate the synchronized string by adding the current actor
				ActorJson naj = new ActorJson ();
				naj.actorIndex = data;
				naj.bugsEaten = 1;
				List<ActorJson> ls = new List<ActorJson> ();
				ls.Add (naj);
				revisedASJ.actors = ls.ToArray ();
			}

			synchronizable.synchronizedString = JsonUtility.ToJson (revisedASJ);
			Debug.Log ("synchronizable.synchronizedString is: " + synchronizable.synchronizedString);
		}
	}

	public int checkBugsEaten(string actor) { 
		ActorSetJSON capturedASJ = new ActorSetJSON ();	
		if (synchronizable.synchronizedString != null && synchronizable.synchronizedString != "") { 
			capturedASJ = JsonUtility.FromJson<ActorSetJSON> (synchronizable.synchronizedString);
			int casjLength = capturedASJ.actors.Length;
			for (int i = 0; i < casjLength; i++) {
				if (capturedASJ.actors [i].actorIndex == actor) {
//					Debug.Log("actor " + actor + " bugs eaten " +capturedASJ.actors [i].bugsEaten); 
					return capturedASJ.actors [i].bugsEaten;	
				}
			}
		}
		return 0;
	}
}
