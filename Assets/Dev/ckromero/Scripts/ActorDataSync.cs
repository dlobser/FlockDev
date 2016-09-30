using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holojam.Tools;
using System;

public class ActorDataSync : MonoBehaviour
{
	public GameObject ActorSynchronizableManager;
	public bool resetSync = false;
	public float actorLevelCheck = 5.0f;
	public string currentActor;

	private List<IndexedPos> indexedPos = new List<IndexedPos> ();
	private Holobounds hb;
	//	private GameObject swarmMaker;
	private ActorSyncer actSync;
	private Actor[] actors;
	private ActorManager actorManager;

	//	private bool canCallFunction = true;

	void Start ()
	{
		actSync = GetComponent <ActorSyncer> ();

		GameObject am = GameObject.Find ("ActorManager");
		actorManager = (ActorManager)am.GetComponent (typeof(ActorManager));
		actors = actorManager.actors;

//		swarmMaker = GameObject.Find ("SwarmSpawnPoint");
		GameObject hbgo = GameObject.Find ("Holobounds");

		hb = (Holobounds)hbgo.GetComponent (typeof(Holobounds));

//		if (GameObject.Find ("ChosenHeadset")) {
//			GameObject headsetGO = GameObject.Find ("ChosenHeadset");
//			currentActor = headsetGO.GetComponent<ChosenHeadset> ().whichHeadset.ToString ();
//		} else {
//			//NORMALIZE HERE
////			actorManager.buildTag
		foreach (Actor a in actors) {
			if (a.trackingTag == actorManager.buildTag) {
				currentActor = a.name.Substring (0, a.name.IndexOf ("]") + 1);			
				Debug.Log ("curentActor is: " + currentActor);
			}
		}
	}

	void Update ()
	{
		if (resetSync) {
			Debug.Log ("resetSync clicked");
			actSync.resetSync ();
			resetSync = false;
		}
	}

	public void UpdateActor (string actorName, string interactorName, int updateType)
	{
		switch (updateType) {
		case 1:
			//convert to int
			string actorNum = actorName.Substring (0, actorName.IndexOf ("]") + 1);
			Debug.Log ("Updating actorNum: " + actorNum);
			actSync.SendSyncStringData (actorNum);

			break;
		}
	}

	public void ResetActor ()
	{ 
		actSync.ResetActor (currentActor);
	}

	public int ActorBugsEaten ()
	{
		return actSync.checkBugsEaten (currentActor);
	}

	public int ActorBugsEatenSince (float time)
	{

		return actSync.checkBugsEatenSince (currentActor, time);
	}

	public void SwapActor ()
	{
		actors = actorManager.actors;
		Actor currentActorComponent;

		foreach (Actor a in actors) {
			if (a.name.Contains ("Build")) {
				currentActorComponent = a.GetComponent<Actor> ();
				break;
			}
		}

//		currentActorComponent.eyes.y;

		Debug.Log ("actor swapped");
	}


	//IEnumerator
	public void MoveSpawner (float time)
	{
//		canCallFunction= false;

//
//		Vector3 moveTo = GetGoodSpawnPoint ();
//
//		Debug.Log ("moveTo is: " + moveTo.ToString ());
//		swarmMaker.transform.position = moveTo;

//		Debug.Log ("time started");
//		yield return new WaitForSeconds(time);

//		Debug.Log ("time ended");
//		canCallFunction=true;
	}

	private List<IndexedPos> GetActorPositions ()
	{
		indexedPos.Clear ();

		foreach (Actor a in actors) {
			int actorNum = int.Parse (a.name.Substring (1, a.name.IndexOf ("]") - 1));
			if (!(a.transform.position.y > 10.0f)) {
				IndexedPos ip = new IndexedPos (actorNum, a.transform.position);
				indexedPos.Add (ip);
			}

		}
	
		return indexedPos;
	}

	class QuadWithCount
	{
		public Vector3[] quad;
		public int count = 0;
	}

	//TODO: this could really use a unit test to ensure sane boundaries.
	//returns a reasonably uncrowded spot in the holobounds
	public Vector3 GetGoodSpawnPoint ()
	{
//		Vector3 result = new Vector3 ();

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
		float axl = (x1 + x0) / 2.0f;
		float axr = (x2 + x3) / 2.0f;
		float azb = (z3 + z0) / 2.0f; 
		float azt = (z2 + z1) / 2.0f;

		//width and depth of the area
		float xR = Math.Abs (axl) + Math.Abs (axr);
		float zR = Math.Abs (azt) + Math.Abs (azb);

		//Containers for areas that hold actors 
		QuadWithCount qb0 = new QuadWithCount ();
		qb0.quad = new [] { new Vector3 (x0, 0.0f, z0),
			new Vector3 (x0 + xR / 2.0f, 0.0f, z0),
			new Vector3 (x0 + xR / 2.0f, 0.0f, z0 + zR / 2.0f),
			new Vector3 (x0, 0.0f, z0 + zR / 2.0f)
		}; 

		QuadWithCount qb1 = new QuadWithCount (); 
		qb1.quad = new [] { 
			new Vector3 (x0 + xR / 2.0f, 0.0f, z0),
			new Vector3 (x0 + xR, 0.0f, z0),
			new Vector3 (x0 + xR, 0.0f, z0 + zR / 2.0f),
			new Vector3 (x0 + xR / 2.0f, 0.0f, z0 + zR / 2.0f)
		};

		QuadWithCount qb2 = new QuadWithCount (); 
		qb2.quad = new [] { 
			new Vector3 (x0 + xR / 2.0f, 0.0f, z0 + zR / 2.0f),
			new Vector3 (x0 + xR, 0.0f, z0 + zR / 2.0f),
			new Vector3 (x0 + xR, 0.0f, z0 + zR),
			new Vector3 (x0 + xR / 2.0f, 0.0f, z0 + zR)
		};

		QuadWithCount qb3 = new QuadWithCount ();
		qb3.quad = new [] { 
			new Vector3 (x0, 0.0f, z0 + zR / 2.0f),
			new Vector3 (x0 + xR / 2.0f, 0.0f, z0 + zR / 2.0f),
			new Vector3 (x0 + xR / 2, 0.0f, z0 + zR),
			new Vector3 (x0, 0.0f, z0 + zR)
		};

		//storage for actors in each area
		List<IndexedPos> ipos = GetActorPositions ();

		//place the actors in quadrants

		int iposCount = ipos.Count;
		for (int i = 0; i < iposCount; i++) {
			if (ipos [i].position.x < x0 + (xR / 2.0f)) {
				if (ipos [i].position.z < z0 + (zR / 2.0f)) {
					qb0.count++;
				} else {
					qb3.count++;
				}
			} else {
				if (ipos [i].position.z < z0 + zR / 2.0f) {
					qb1.count++;
				} else {
					qb2.count++;
				}
			}
		}

		List<QuadWithCount> allqb = new List<QuadWithCount> ();
		allqb.Add (qb0);
		allqb.Add (qb1);
		allqb.Add (qb2);
		allqb.Add (qb3);
		allqb.Sort ((x, y) => (x.count.CompareTo (y.count)));

		QuadWithCount leastPopulatedQuad = allqb [0];

		Bounds bounds = new Bounds ();

		//populate bounds with the corners of the winning quad and return its center as 
		//a spawn or action point.
		foreach (Vector3 vec in leastPopulatedQuad.quad) {
			bounds.Encapsulate (vec);
		}

//		Debug.Log(bounds.center.ToString());
		return bounds.center;
	}
}
