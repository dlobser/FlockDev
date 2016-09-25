﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Holojam.Tools;

public class PlayerStateManager : MonoBehaviour
{
	public FlockLevel[] flockLevels;
	public GameObject player;
	public GameObject bug;
	public GameObject ground;
	public GameObject audioManagerObject;
	public GameObject faderManagedObject;

	public Canvas canvas;
	public Sprite youMustEat;
	public Sprite timeToDie;

	public float graceTime = 5.0f;
	public float warnForSeconds = 5.0f;
	public bool resetPlayer = false;

	//TODO: make private?
	public PlayerData playerData;
	private ActorDataSync actorDataSync;
	private AudioManager audioManager;
	private FaderManager faderManager;
	private HUDManager hudManager;

	private float audioTransitionDefaultTime = 2.0f;

	void Awake ()
	{
		actorDataSync = player.GetComponent<ActorDataSync> ();
		playerData = new PlayerData ();
		audioManager = audioManagerObject.GetComponent<AudioManager> ();
		faderManager = faderManagedObject.GetComponent<FaderManager> ();
		hudManager = GetComponentInParent<HUDManager> ();
	}

	void Start ()
	{
		playerData.actorName = actorDataSync.currentActor;
		LoadLevel (0);
		hudManager.UpdateHUD ("hide");
	}

	// Update is called once per frame
	void Update ()
	{
		//review player state and update levels as needed
		//this will be the main consumer of the bugsEaten timeline.
		CheckPlayerLevel ();	
		CheckExpState ();
		//currently resetPlayer is a checkbox in the Editor UI for testing, needs to be triggered by zones.
		//TODO: master sync will reset the player IRL based on zones. 
		if (resetPlayer) { 
			ResetPlayer ();
			resetPlayer = false;
		}
	}

	//This looks at how many bugs the current player has eaten as a basis for level loading and playerState.
	public void CheckPlayerLevel ()
	{ 
		int bugsAte = actorDataSync.ActorBugsEaten ();
		int i = Array.FindLastIndex (flockLevels, w => w.bugsEatenMinimum <= bugsAte);	
		if (i > playerData.level) {
			Debug.Log ("Current level is " + playerData.level + " level should be " + i + " so changing level");
			LoadLevel (i);
			playerData.level = i;
			playerData.levelStartTime = Time.time;
		}
	}

	//This also looks at bugs eaten within a time.
	public void CheckExpState ()
	{
		//Does the current level have a bugs eaten with time requirement
		if (flockLevels [playerData.level].bugsNeededForTime != 0) { 
			//Allow a grace time at the beginning of the level. and note current player state. 
			if (playerData.levelStartTime + graceTime < Time.time && playerData.expState != ExpState.Warn && playerData.expState != ExpState.Dying) {
				//Warn if not enough bugs have been eaten within the time range.
				if (playerData.bugsEatenSince (Time.time - flockLevels [playerData.level].bugTime) < flockLevels [playerData.level].bugsNeededForTime) {
					Debug.Log ("not enough bugs!");
					hudManager.UpdateHUD ("warn");
					playerData.expState = ExpState.Warn;
					playerData.dyingTime = Time.time + warnForSeconds;
				}
			}
		}
		//A warning before dying in this case
		//TODO: after a certain amount of time even if no warn (total time since last reset) 
		//player can die without warning  
		if (playerData.expState == ExpState.Warn && Time.time > playerData.dyingTime) { 
			hudManager.UpdateHUD ("die");
			playerData.expState = ExpState.Dying;
		}
	}

	void ResetPlayer ()
	{
		//reset all player variables
		playerData.resetPlayerData ();
		LoadLevel (0);
		hudManager.UpdateHUD ("hide");
	}

	private FlockLevel GetFlockLevel (int lvl)
	{ 
		int i = Array.FindIndex (flockLevels, w => w.level == lvl);
		return flockLevels [i];
	}

	public void LoadLevel (int _level)
	{
		FlockLevel levelToLoad = GetFlockLevel (_level);

		if (player != null) {
			
			//Avatar Material
			if (levelToLoad.avatarMaterial != null) { 
				player.GetComponent<MeshRenderer> ().material = levelToLoad.avatarMaterial;
			}
			//TODO: should swapmaterial go here? 
			if (levelToLoad.bugMaterial != null) { 
				bug.GetComponent<MeshRenderer> ().material = levelToLoad.bugMaterial;
			}
			if (levelToLoad.environmentMaterial != null) { 
				ground.GetComponent<MeshRenderer> ().material = levelToLoad.environmentMaterial;
			}
			if (levelToLoad.audioSnapshotName != null && levelToLoad.audioSnapshotName != "") {
				audioManager.TransitionAudio (levelToLoad.audioSnapshotName, audioTransitionDefaultTime);
			}
			//fader object stand in
			if (levelToLoad.faderLevel != 0.0f) { 
				faderManager.level = levelToLoad.faderLevel;
			}
		} else {
			Debug.Log ("no player!");
		}
		Debug.Log ("Loaded FlockLevel " + _level);
	}
}