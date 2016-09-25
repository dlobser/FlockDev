using UnityEngine;
using System.Collections;

public class BugSpawner : MonoBehaviour {
	public GameObject actorSynchronizable;

	private ActorDataSync actorDataSync = new ActorDataSync ();
	private Vector3 moveTo = new Vector3 ();

	// Use this for initialization
	void Start () {
		actorDataSync = actorSynchronizable.GetComponent<ActorDataSync> ();
	}
	
	// Update is called once per frame
	void Update () {
		moveTo = actorDataSync.GetGoodSpawnPoint ();
		this.transform.position = moveTo;
	}
}
