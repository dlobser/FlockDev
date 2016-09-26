using System;
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

	public GameObject audioManagerObject;
	public GameObject faderManagerObject;
	public GameObject speedManagerObject;
	public GameObject ZoneManagerObject;

	public Canvas canvas;
	public Sprite youMustEat;
	public Sprite timeToDie;

	public float graceTime = 5.0f;
	public float warnForSeconds = 5.0f;
	public bool resetPlayer = false;
	public float allowedSessionTime = 30.0f;
	public float timeLeftToDie = 5.0f;
	public float maxSpeedToSitStill = 2.0f;

	//TODO: make private?
	public PlayerData playerData;
	private ActorDataSync actorDataSync;
	private AudioManager audioManager;
	private FaderManager faderManager;
	private FaderManager speedManager;
	private ZoneManager zoneManager;
	private HUDManager hudManager;
	private GetSpeed speedObject;

	private int bugsAte;

	private float audioTransitionDefaultTime = 2.0f;
	private bool isDead = false;
	private bool isAscendTriggered = false;

	void Awake ()
	{
		actorDataSync = player.GetComponent<ActorDataSync> ();
		playerData = new PlayerData ();
		audioManager = audioManagerObject.GetComponent<AudioManager> ();
		faderManager = faderManagerObject.GetComponent<FaderManager> ();
		speedManager = speedManagerObject.GetComponent<FaderManager> (); 
		hudManager = GetComponentInParent<HUDManager> ();
		zoneManager = ZoneManagerObject.GetComponent<ZoneManager> ();
		speedObject = speedManagerObject.GetComponent<GetSpeed> ();
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
		//This isn't very defensive right now, it makes assumptions about the current user!
		UpdatePlayerZone ();
		//This is a player that is ready to be born
		//TODO: consider zone here as well
		if (playerData.expState == ExpState.ReadyToLive && playerData.zoneName == "readyZone") {
			resetPlayer = true;
		} else {

			//This is a player that is about to die
			if (playerData.level == 9) { 
				
				//if player is in the dead zone
				//and player is holding still (transformation < .1m)
				if (playerData.zoneName == "dyingZone") {
					if (speedObject.speed < maxSpeedToSitStill && !isAscendTriggered) { 
						//and player is holding still (transformation < .1m)
						//begin death animation
						Debug.Log ("wake up, time to die");
						isAscendTriggered = true;
						Ascend ();
						hudManager.UpdateHUD ("sessionComplete");
						playerData.expState = ExpState.ReadyToLive;
						isDead = true;
					}					
				} else {

					//if complete fade to black, instruct to remove headset
					//if time left to die > time int level ELSE session complete
					if (Time.time - playerData.sessionStartTime + graceTime > allowedSessionTime && !isDead) { 
						hudManager.UpdateHUD ("sessionComplete");
						playerData.expState = ExpState.ReadyToLive;
						isDead = true;
						//if complete fade to black, instruct to remove headset

					}
				}

			} else {
				//review current level
				bugsAte = actorDataSync.ActorBugsEaten ();
				int i = Array.FindLastIndex (flockLevels, w => w.bugsEatenMinimum <= bugsAte);	
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
		Debug.Log ("Acending");
		yield return new WaitForSeconds (3);
		Debug.Log ("Ascended");
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
		if (playerData.expState == ExpState.Warn && Time.time > playerData.dyingTime || playerData.sessionStartTime > allowedSessionTime) { 
			LoadLevel (9);
			playerData.level = 9;
			hudManager.UpdateHUD ("dying");
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
			
			//TODO: should swapmaterial go here? 
			if (levelToLoad.audioSnapshotName != null && levelToLoad.audioSnapshotName != "") {
				audioManager.TransitionAudio (levelToLoad.audioSnapshotName, audioTransitionDefaultTime);
			}
			//Fader Manager
			if (levelToLoad.globalFadeLevel != 0.0f) { 
				faderManager.level = levelToLoad.globalFadeLevel;
			}
			//Speed Manager
			if (levelToLoad.speedFaderLevel != 0.0f) { 
				speedManager.level = levelToLoad.speedFaderLevel;
			}

		} else {
			Debug.Log ("no player!");
		}
		Debug.Log ("Loaded FlockLevel " + _level);
	}
}