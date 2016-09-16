using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holojam.Tools;
using System;

public class ActorData : MonoBehaviour
{
	//	public ActorManager actorManager;
	//	public ActorSyncer actorSyncer;
	public GameObject ActorSynchronizableManager;
	ActorSyncer actSync;
	string currentActor;
	private Actor[] actors;
	private List<IndexedPos> indexedPos = new List<IndexedPos> ();
	private Holobounds hb;

	// Use this for initialization
	void Start ()
	{
		GameObject go = GameObject.Find ("ActorSynchronizableManager");
		actSync = (ActorSyncer)go.GetComponent (typeof(ActorSyncer));
		GameObject am = GameObject.Find ("ActorManager");
		ActorManager actorManager = (ActorManager)am.GetComponent (typeof(ActorManager));

		GameObject hbgo = GameObject.Find ("Holobounds");
		hb = (Holobounds)hbgo.GetComponent (typeof(Holobounds));
//		Debug.Log("hb.Corner: " + hb.Corner (0));
//		Debug.Log("hb.Corner: " + hb.Corner (1));
//		Debug.Log("hb.Corner: " + hb.Corner (2));
//		Debug.Log("hb.Corner: " + hb.Corner (3));

		actors = actorManager.actors;
		foreach (Actor a in actors) {
			if (a.name.Contains ("Build")) {
				currentActor = a.name.Substring (0, a.name.IndexOf ("]") + 1);			
				Debug.Log ("curentActor is: " + currentActor);
			}
		}
	}

	public void UpdateActor (Synchronizable synchronizable, string actorName, string interactorName, int updateType)
	{
		switch (updateType) {
		case 1:
			//convert to int
			string actorNum = actorName.Substring (0, actorName.IndexOf ("]") + 1);
			Debug.Log ("actorNum: " + actorNum);
			actSync.SendSyncStringData (actorNum);

			break;
		}
	}

	public int ActorBugsEaten ()
	{
		return actSync.checkBugsEaten (currentActor);
	}

	void Update ()
	{

//		List<IndexedPos> ip = GetActorPositions ();

//		Debug.Log(ip[2].position.ToString());
		GetGoodSpawnPoint();

	}

	private List<IndexedPos> GetActorPositions ()
	{
		indexedPos.Clear ();

		foreach (Actor a in actors) {
			int actorNum = int.Parse (a.name.Substring (1, a.name.IndexOf ("]") - 1));
			IndexedPos ip = new IndexedPos (actorNum, a.transform.position);
			indexedPos.Add (ip);
		}

		return indexedPos;
	}
	//returns a reasonably uncrowded spot in the holobounds
	private Vector3 GetGoodSpawnPoint ()
	{
		Vector3 result = new Vector3 ();

		//could so this with points? 
		//corners are ordered counter-clockwise from lower left	
		float x0 = hb.Corner (0).x;
		float x1 = hb.Corner (1).x;
		float x2 = hb.Corner (2).x;
		float x3 = hb.Corner (3).x;

		float z0 = hb.Corner (0).z;
		float z1 = hb.Corner (1).z;
		float z2 = hb.Corner (2).z;
		float z3 = hb.Corner (3).z;

		//Holobounds is not a perfect square so average the values on the side.
		//Calculations for odd shapes are more involved. 
		float axl = (x1 + x0)/2.0f;
		float axr = (x2 + x3)/2.0f;
		float azb = (z3 + z0)/2.0f; 
		float azt = (z2 + z1)/2.0f;

		float xR = Math.Abs (axl) + Math.Abs(axr);
		float zR = Math.Abs (azt) + Math.Abs(azb);


		List<IndexedPos> ipos = GetActorPositions ();
		List<IndexedPos> q0 = new List<IndexedPos> ();
		List<IndexedPos> q1 = new List<IndexedPos> ();
		List<IndexedPos> q2 = new List<IndexedPos> ();
		List<IndexedPos> q3 = new List<IndexedPos> ();

		//place the actors in quadrants
		foreach (IndexedPos ip in ipos) {
			if (ip.position.x / 2.0f < xR / 2.0f) {
				if (ip.position.z < zR / 2.0f) {
					q0.Add (ip);
				} else {
					q3.Add (ip);
				}
			} else {
				if (ip.position.z < zR / 2.0f) {
					q1.Add (ip);
				} else {
					q2.Add (ip);
				}
			}
		}
		List<List<IndexedPos>> Lip = new List<List<IndexedPos>>();
		Lip.Add (q0);
		Lip.Add (q1);
		Lip.Add (q2);
		Lip.Add (q3);

//		SectionList.Sort((a,b) => a.Count - b.Count);
	
		Lip.Sort((a,b)=>a.Count - b.Count);

		List<IndexedPos> leastPopulatedQuadrant = Lip [Lip.Count - 1];

		IndexedPos bestQuad = Lip [0];

		return result;
	}
}
