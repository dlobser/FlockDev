using System;
using System.Collections;
using System.Collections.Generic;
using Holojam.Tools;
using UnityEngine;

public class ActorSyncer : Controller
{
	public override bool hasText{get{return true;}}
	public override string labelField{get{return "ActorSyncer";}}
	public override string scopeField{get{return Holojam.Network.Client.SEND_SCOPE;}}
	public override bool isSending{get{return BuildManager.IsMasterPC();}}
	
	//stub
	protected override ProcessDelegate Process{get{return Stub;}}
	void Stub(){}

	private ActorSetJSON capturedASJ;
	private ActorSetJSON revisedASJ;
	private ActorJson naj;
	private List<ActorJson> ls;
	private List<int> bugsEatenTime;
	private string cachedSync = "";
	private int cachedBugsEaten = 0;
	// Use this for initialization
	void Start ()
	{
		resetSync();
		capturedASJ = new ActorSetJSON ();
		revisedASJ = new ActorSetJSON ();
		naj = new ActorJson ();
		ls = new List<ActorJson> ();
		bugsEatenTime = new List<int> ();

	}

	public void PrintSyncString ()
	{
		DebugX.Log (GetText());
	}

	public void resetSync ()
	{ 
		ResetView();
	}

	public void SendSyncStringData (string data)
	{ 
		if (data != null && data != "") {  

			//search the current synchronized string and see if any data is associated with the current actor (data).
			if (GetText() != null && GetText() != "") { 
				capturedASJ = JsonUtility.FromJson<ActorSetJSON> (GetText());
				int casjLength = capturedASJ.actors.Length;
				bool foundExistingActor = false;
				for (int i = 0; i < casjLength; i++) {
					if (capturedASJ.actors [i].actorIndex == data) {
						capturedASJ.actors [i].bugsEaten++;	
						capturedASJ.actors [i].bugTime.Add ((int)Time.time);
						revisedASJ = capturedASJ;
						foundExistingActor = true;
						break;
					}
				}

				//data is already stored for other actors but not for this one yet. 
				if (!foundExistingActor) {
//					naj = default(ActorJson);

					naj.actorIndex = data;
					naj.bugsEaten = 1;
					naj.bugTime = new List<int> ();
					naj.bugTime.Add ((int)Time.time);

//					List<ActorJson> ls = new List<ActorJson> ();
					ls.Clear();	
					ls.Add (naj);
					int ct = capturedASJ.actors.Length;

					for (int i = 0; i < ct; i++) {
						ls.Add (capturedASJ.actors [i]);
					}

					revisedASJ.actors = ls.ToArray ();
				}
			} else { 
				//initiate the synchronized string by adding the current actor
//				ActorJson naj = new ActorJson ();
//				naj = default(ActorJson);
				naj.actorIndex = data;
				naj.bugsEaten = 1;
				naj.bugTime = new List<int> ();
				naj.bugTime.Add ((int)Time.time);

				ls.Add (naj);
			revisedASJ.actors = ls.ToArray ();
			}

			SetText(JsonUtility.ToJson(revisedASJ));
			PrintSyncString ();
		}
	}

	public void ResetActor (string currentActor)
	{ 
		ActorSetJSON newASJ =  new ActorSetJSON ();
		//remove the actor from the syncstring, all instances just in case!
		if (GetText() != null && GetText() != "") { 
			capturedASJ = JsonUtility.FromJson<ActorSetJSON> (GetText());
//			newASJ = capturedASJ;
//			newASJ.actors.Initialize();
//			revisedASJ=default(revisedASJ);
			int casjLength = capturedASJ.actors.Length;

			//TODO: ActorJSON value array needs thought
			newASJ.actors = new ActorJson[casjLength-1];

			int j=0;
			for (int i = 0; i < casjLength; i++) {
				if (capturedASJ.actors [i].actorIndex != currentActor) {
					newASJ.actors [j] = capturedASJ.actors [i];
					j++;
				}
			}	
		}
		SetText(JsonUtility.ToJson(newASJ));
		PrintSyncString ();
	}

	public int checkBugsEaten (string actor)
	{ 
		if (GetText() != cachedSync) {
			capturedASJ = default (ActorSetJSON);	
			if (GetText() != null && GetText() != "") { 
				capturedASJ = JsonUtility.FromJson<ActorSetJSON> (GetText());
				int casjLength = capturedASJ.actors.Length;
				for (int i = 0; i < casjLength; i++) {
					if (capturedASJ.actors [i].actorIndex == actor) {
						cachedBugsEaten = capturedASJ.actors [i].bugsEaten;	
					}
				}
			}
			cachedSync = GetText();
		} 

		return cachedBugsEaten;
	}

	public int checkBugsEatenSince (string actor, float sinceTime)
	{
		//access actor 
		capturedASJ = default( ActorSetJSON );
		bugsEatenTime = default (List<int>);
		bool foundTimeList = false;
			
		if (GetText() != null && GetText() != "") { 
			capturedASJ = JsonUtility.FromJson<ActorSetJSON> (GetText());
			int casjLength = capturedASJ.actors.Length;
			for (int i = 0; i < casjLength; i++) {
				if (capturedASJ.actors [i].actorIndex == actor) {
					bugsEatenTime = capturedASJ.actors [i].bugTime;	
					foundTimeList = true;
				}
			}
		}
				
		if (!foundTimeList) {
			return 0;
		}

		int bugTimeLength = bugsEatenTime.Count;
		int bugsEatenSinceTimeCounter = 0;
		float timeWindow = Time.time - sinceTime; 

		for (int i = 0; i < bugTimeLength; i++) {
			if (bugsEatenTime [i] > timeWindow) {
				bugsEatenSinceTimeCounter++;
			}
		}
		return bugsEatenSinceTimeCounter;
	}
}
