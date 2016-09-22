using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateManager : MonoBehaviour
{
	public FlockLevel[] flockLevels;
	//	public GameObject materialsCollection;
	public GameObject player;
	public GameObject bug;
	public GameObject ground;
	public Canvas canvas;
	public Sprite youMustEat;
	public Sprite timeToDie;
	public GameObject AudioManagerObject;

	public float graceTime = 5.0f;
	public float warnForSeconds=5.0f;
//	public float blinkForSeconds=5.0f;
	public bool resetPlayer=false;

	//is this the best access modifier?
	public PlayerData playerData;
	public enum ExpState{Living, Warn, Dying};

	private ActorDataSync actorDataSync;
	private AudioManager audioManager;
	private float audioTransitionDefaultTime=2.0f;


	void Awake(){

		actorDataSync = player.GetComponent<ActorDataSync> ();
		playerData = new PlayerData ();
		audioManager = AudioManagerObject.GetComponent<AudioManager> ();

	}


	// Use this for initialization
	void Start ()
	{

		playerData.actorName=actorDataSync.currentActor;

		LoadLevel (0);

		UpdateHUD("hide");


	}

	void ResetPlayer ()
	{
		//reset all player variables
		playerData.resetPlayerData ();
		LoadLevel (0);
		UpdateHUD("hide");
	}

	// Update is called once per frame
	void Update ()
	{
		//review player state and update levels as needed
		//this will be the main consumer of the bugsEaten timeline.
		CheckPlayerLevel ();	
		CheckExpState ();
		if (resetPlayer) { 
			playerData.resetPlayerData ();
			LoadLevel (0);
			UpdateHUD("hide");
			//TODO: notify actorDataSync to zero out the player. 

			resetPlayer = false;

		}
	}

	//This looks at how many bugs the current player has eaten as a basis for level, 
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
		//Does the current level has a bugs eaten with time requirement
		if (flockLevels [playerData.level].bugsNeededForTime != null) { 
			//Allow a grace time at the beginning of the level. and note current player state. 
			if (playerData.levelStartTime + graceTime < Time.time && playerData.expState!=ExpState.Warn&&playerData.expState!=ExpState.Dying) {
				//Warn if not enough bugs have been eaten within the time range.
				if (playerData.bugsEatenSince (Time.time - flockLevels [playerData.level].bugTime) < flockLevels [playerData.level].bugsNeededForTime) {
					Debug.Log ("not enough bugs!");
					UpdateHUD ("warn");
					playerData.expState = ExpState.Warn;
					playerData.dyingTime = Time.time + warnForSeconds;
				}
			}
		}
		//A warning before dying in this case
		//TODO: after a certain amount of time even if no warn (total time since last reset) 
		//player can die without warning  
		if (playerData.expState == ExpState.Warn && Time.time> playerData.dyingTime) { 
			UpdateHUD ("die");
			playerData.expState = ExpState.Dying;
//			playerData.dyingTime = Time.time + warnForSeconds;
		}

	}

	public void UpdateHUD (string HUDState)
	{
		if (canvas == null) {
			return;
		}
		switch (HUDState) {
		case "hide":
			{
				canvas.enabled = false;
				break;
			}
		case "warn":
			{
				Image image = canvas.GetComponentsInChildren<Image> () [0];
				image.sprite = youMustEat;
				canvas.enabled = true;
				break;		
			}
		case "die":
			{
				Image image = canvas.GetComponentsInChildren<Image> () [0];
				image.sprite = timeToDie;
				canvas.enabled = true;

				break;

			}
		}

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

			if (levelToLoad.bugMaterial != null) { 
				bug.GetComponent<MeshRenderer> ().material = levelToLoad.bugMaterial;
			}

			if (levelToLoad.environmentMaterial != null) { 
				ground.GetComponent<MeshRenderer> ().material = levelToLoad.environmentMaterial;
			}
			if (levelToLoad.audioSnapshotName != null && levelToLoad.audioSnapshotName != "") {
				audioManager.TransitionAudio (levelToLoad.audioSnapshotName,audioTransitionDefaultTime);
			}

				} else {
			Debug.Log ("no player!");
		}

		Debug.Log ("Loaded FlockLevel " + _level);
//		Debug.Log (i.ToString());

	}


	[Serializable]
	public class FlockLevel
	{

		public int level;
		public int bugsEatenMinimum;

		//time management
		public float bugTime;
		public int bugsNeededForTime;

		//Environment
		public Material environmentMaterial;

		//Avatar
		public Material avatarMaterial;

		//Bugs
//		public float bugFaderValue;
		public Material bugMaterial;

		//Audio
		public string audioSnapshotName;

		//ZONE

		//special actions
	}

	public class PlayerData
	{
			
		public ExpState expState;
		public int level { get; set; }
		public float dyingTime;
		public string actorName="";
		private List<float> bugsEatenLog;
		private int bugsEaten = 0;

		public PlayerData ()
		{
			level = 0;
			bugsEatenLog = new List<float> ();
			expState=ExpState.Living;
		}

		public float levelStartTime=0.0f;

		public void resetPlayerData ()
		{ 
			level = 0;
			bugsEaten = 0;
			bugsEatenLog = new List<float> ();
			expState = ExpState.Living;
		}

		public void addBugEaten ()
		{
			bugsEaten++;
			bugsEatenLog.Add (Time.time);
			if (expState == ExpState.Warn) { 
				expState = ExpState.Living;
			}
		}

		public int bugsEatenSince (float since)
		{
			float whichSince = since;
//			float whichSince = Time.time - since;
			if (bugsEatenLog != null) {
				return bugsEatenLog.FindAll (b => b > whichSince).Count;
			} else {
				return 0;
			}
		}

		public int bugsAte ()
		{ 
			return bugsEaten;
		}
	}
}
