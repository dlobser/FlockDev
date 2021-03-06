﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using UnityEditor;
using Holojam.Tools;

public class PlayerStateManager : MonoBehaviour
{
	public FlockLevel[] flockLevels;
	public GameObject player;

//	public GameObject audioManagerObject;
	public GameObject faderManagerObject;
//	public GameObject speedManagerObject;
	public GameObject ZoneManagerObject;

//	public Canvas canvas;
//	public Sprite youMustEat;
//	public Sprite timeToDie;

	public bool resetPlayer = false;

	//now handled by Global Settings!
//	public float graceTime = 5.0f;
//	public float warnForSeconds = 10.0f;
//	public float allowedSessionTime = 420.0f;
//	public float timeLeftToDie = 10.0f;
//	public float maxSpeedToSitStill = 2.0f;

	public float currentFaderLevel;


	//TODO: make private?
	public PlayerData playerData;

	public GameObject levelHandler2GO;
	private LevelHandler2 levelHandler2;

	public GameObject settingsObject;
//	private SettingsManager globalSettings;
	private SettingsJSON settingsJSON;

	private ActorDataSync actorDataSync;
//	private AudioManager audioManager;
	private FaderManager faderManager;
	//	private FaderManager speedManager;
	private ZoneManager zoneManager;
	private HUDManager hudManager;
	private GetSpeed speedObject;

	private int bugsAte;

	private float audioTransitionDefaultTime = 2.0f;
	private bool isDead = false;
	private bool isAscendTriggered = false;

	public TextMesh txt;

	void Awake ()
	{
		actorDataSync = player.GetComponent<ActorDataSync> ();
		playerData = new PlayerData ();
//		audioManager = audioManagerObject.GetComponent<AudioManager> ();
		faderManager = faderManagerObject.GetComponent<FaderManager> ();
//		speedManager = speedManagerObject.GetComponent<FaderManager> (); 
		hudManager = GetComponentInParent<HUDManager> ();
		zoneManager = ZoneManagerObject.GetComponent<ZoneManager> ();
		levelHandler2 = levelHandler2GO.GetComponent<LevelHandler2> ();

//		speedObject = speedManagerObject.GetComponent<GetSpeed> ();

		settingsJSON = settingsObject.GetComponent<SettingsManager> ().settingsJSON;
	
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
//		Debug.Log ("hi");
		//review player state and update levels as needed
		//this will be the main consumer of the bugsEaten timeline.
		CheckPlayerLevel ();	
//		CheckExpState ();

		//currently resetPlayer is a checkbox in the Editor UI for testing, needs to be triggered by zones.
		if (resetPlayer) { 
			ResetPlayer ();
		}

//		txt.text = "level:" + playerData.level.ToString();
	}

	//This looks at how many bugs the current player has eaten as a basis for level loading and playerState.
	public void CheckPlayerLevel ()
	{	
		//This isn't very defensive right now, it makes assumptions about the current user!
		UpdatePlayerZone ();
		//This is a player that is ready to be born
		if (playerData.expState == ExpState.ReadyToLive) {
			if (playerData.zoneName == "readyZone") {
				ResetPlayer ();
				playerData.expState = ExpState.Living;
			} else {
				cachedDebugMessage (playerData.actorName + " is standing by for reset");
//				Debug.Log (playerData.actorName + " is standing by for reset");
			}
		} else {

			//This is a player that is about to die
			if (playerData.level == 9) { 
				if (Time.time - playerData.levelStartTime > settingsJSON.timeLeftToDie) {
					Debug.Log ("waiting for player to enter dyingZone");
					//if player is in the dead zone
					//and player is holding still (transformation < .1m)
					if (playerData.zoneName == "dyingZone") {
						if (speedObject.speed < settingsJSON.maxSpeedToSitStill && !isAscendTriggered) { 
							//and player is holding still (transformation < .1m)
							//begin death animation
							isAscendTriggered = true;
							StartCoroutine (Ascend ());
						}					
					} else {
						LetPlayerDie ();
					}
				} 
			} else {
				//regular level review
				bugsAte = actorDataSync.ActorBugsEaten ();

				int i = Array.FindLastIndex (flockLevels, w => w.bugsEatenMinimum <= bugsAte);	

				if (i != 9) {
					//FADED LEVEL CALCULATIONS
					int bugsInRange = flockLevels [i + 1].bugsEatenMinimum - flockLevels [i].bugsEatenMinimum;
					float timeRangeForBugsEatenCheck = Time.time - playerData.levelStartTime; 
					int bugsInTime = actorDataSync.ActorBugsEatenSince (timeRangeForBugsEatenCheck);
					float fadedLevel = bugsInTime / (float)bugsInRange;
					faderManager.level = flockLevels [i].globalFadeLevel + fadedLevel;
					levelHandler2.bugsEaten = bugsAte;
					levelHandler2.percentInCurrentLevel = fadedLevel;
					//checking level 8 (which equals array index 9) for bugs needed
					//we should state top level of array instead of hardcoding numerics here
					levelHandler2.percentOfTotal = bugsAte / (float) flockLevels [8].bugsEatenMinimum;
					levelHandler2.level = playerData.level;
				}	
				if (i > playerData.level) {
					Debug.Log ("Current level is " + playerData.level + " level should be " + i + " so changing level");
					LoadLevel (i);
					playerData.level = i;
					playerData.levelStartTime = Time.time;
				}
			}
		}
	}

	//begin death animation
	private IEnumerator Ascend ()
	{
		Debug.Log ("Ascending");
		actorDataSync.SwapActor ();
		yield return new WaitForSeconds (3);
		Debug.Log ("Ascended");
		LetPlayerDie ();
	}

	private void LetPlayerDie ()
	{
		hudManager.UpdateHUD ("sessionComplete");
		playerData.expState = ExpState.ReadyToLive;
		Debug.Log ("you're done");
	}
	//This also looks at bugs eaten within a time.
	public void CheckExpState ()
	{
		//Does the current level have a bugs eaten with time requirement
		if (flockLevels [playerData.level].bugsNeededForTime != 0) { 
			//Allow a grace time at the beginning of the level. and note current player state. 
			if (playerData.levelStartTime + settingsJSON.graceTime < Time.time && playerData.expState != ExpState.Warn && playerData.expState != ExpState.Dying) {
				//Warn if not enough bugs have been eaten within the time range.
				// currentTime bugs eaten since levelStartTime + allowedTime   
				if ((playerData.bugsEatenSince (Time.time - flockLevels [playerData.level].bugTime)) < flockLevels [playerData.level].bugsNeededForTime) {
					Debug.Log ("not enough bugs!");
					hudManager.UpdateHUD ("warn");
					playerData.expState = ExpState.Warn;
					playerData.dyingTime = Time.time + settingsJSON.warnForSeconds;
				}
			}
		}
		//A warning before dying in this case
		//TODO: after a certain amount of time even if no warn (total time since last reset) 
		//player can die without warning  
		if ((playerData.expState == ExpState.Warn && Time.time > playerData.dyingTime) || playerData.sessionStartTime > settingsJSON.experienceLengthSeconds) { 
			LoadLevel (9);
			playerData.level = 9;
			playerData.levelStartTime = Time.time;
			hudManager.UpdateHUD ("ascensionNest");
			playerData.expState = ExpState.Dying;
		}
	}

	void UpdatePlayerZone ()
	{
		playerData.zoneName = zoneManager.currentActorZone;
	}

	void ResetPlayer ()
	{
		//reset all player variables
		playerData.resetPlayerData ();
		actorDataSync.ResetActor ();

		LoadLevel (0);
		hudManager.UpdateHUD ("hide");
		Debug.Log ("player has been reset");
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
//			
//			//TODO: should swapmaterial go here? 
//			if (levelToLoad.audioSnapshotName != null && levelToLoad.audioSnapshotName != "") {
//				audioManager.TransitionAudio (levelToLoad.audioSnapshotName, audioTransitionDefaultTime);
//			}
			//Fader Manager
			if (levelToLoad.globalFadeLevel != 0.0f) { 
				faderManager.level = levelToLoad.globalFadeLevel;
			}
//			//Speed Manager
//			if (levelToLoad.speedFaderLevel != 0.0f) { 
//				speedManager.level = levelToLoad.speedFaderLevel;
//			}

		} else {
			Debug.Log ("no player!");
		}
		Debug.Log ("Loaded FlockLevel " + _level);
	}

	private string message = "";

	void cachedDebugMessage (string _message)
	{
		if (_message != message) {
			Debug.Log (_message);
			message = _message;
		}
	}
}
