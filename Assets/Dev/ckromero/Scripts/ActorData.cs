using UnityEngine;
using System.Collections;
using Holojam.Tools;


public class ActorData : MonoBehaviour {
//	public ActorManager actorManager;
//	public ActorSyncer actorSyncer;
	public GameObject ActorSynchronizableManager;
	ActorSyncer actSync;
	string currentActor;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find("ActorSynchronizableManager");
		actSync = (ActorSyncer) go.GetComponent(typeof(ActorSyncer));
		GameObject am = GameObject.Find ("ActorManager");
		ActorManager actorManager = (ActorManager)am.GetComponent (typeof(ActorManager));
		Actor[] actors = actorManager.actors;
		foreach (Actor a in actors) {
			if (a.name.Contains ("Build")) {
				currentActor = a.name.Substring (0, a.name.IndexOf ("]") + 1);			
				Debug.Log ("curentActor is: " + currentActor);
			}
		}
	}

	public void UpdateActor(Synchronizable synchronizable, string actorName, string interactorName, int updateType){
		switch (updateType){
		case 1:
			string actorNum = actorName.Substring (0, actorName.IndexOf ("]") + 1);
			Debug.Log ("actorNum: " + actorNum);
			actSync.SendSyncStringData (actorNum);

			break;
		}
	}
	public int ActorBugsEaten(){
		return actSync.checkBugsEaten (currentActor);
	}
}
