using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class PlayerStateManager : MonoBehaviour {

	public FlockLevel[] flockLevels;

	// Use this for initialization
	void Start () {
		//initialize the player references 
	}

	void ResetPlayer() {
		//reset all player variables

	}

	// Update is called once per frame
	void Update () {
		//review player state and update levels as needed
		//this will be the main consumer of the bugsEaten timeline.
	}
	[Serializable]
	public class FlockLevel {
		public int level;
		public int bugsEatenMinimum;
		public float timeBoundaryForBugsEatenForLevelEntry;
		public int BugsEatenInTimeBounds;
		public int bugsEatenInTimeBoundsForLevelRetention;
		//environmentShader
		//avatarShader
		//AudioSnapshot 
		//bugshadersAvailable
		//associatedZone
		//special actions
		//HUD events
	}
}
