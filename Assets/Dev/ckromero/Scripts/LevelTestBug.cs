using UnityEngine;
using System.Collections;

public class LevelTestBug : MonoBehaviour {
	PlayerStateManager playerStateManager;
	// Use this for initialization
	void Start () {
		GameObject go =GameObject.Find ("PlayerState");
		playerStateManager = go.GetComponent<PlayerStateManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider o) { 
		Debug.Log (o.name + " collided with " + this.name);
		playerStateManager.playerData.addBugEaten ();
	}
}
