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
	public float graceTime = 5.0f;
	//is this the best access modifier?
	public PlayerData playerData;

	public enum ExpState{Living, Warn, Dying};
	// Use this for initialization
	void Start ()
	{
		//initialize the player references 
		LoadLevel (0);
		UpdateHUD("hide");

		playerData = new PlayerData ();

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
		CheckHUDState ();
	}

	public void CheckPlayerLevel ()
	{ 
		
		int bugsAte = playerData.bugsAte ();
		int i = Array.FindLastIndex (flockLevels, w => w.bugsEatenMinimum <= bugsAte);	

		if (i > playerData.level) {
			Debug.Log ("Current level is " + playerData.level + " level should be " + i + " so changing level");
			LoadLevel (i);
			playerData.level = i;
			playerData.levelStartTime = Time.time;
		}
	}

	public void CheckHUDState ()
	{

		if (flockLevels [playerData.level].bugsNeededForTime != null) { 
			if (playerData.levelStartTime + graceTime < Time.time) {
				if (playerData.bugsEatenSince (Time.time - flockLevels [playerData.level].bugTime) < flockLevels [playerData.level].bugsNeededForTime) {
					Debug.Log ("not enough bugs!");
					UpdateHUD ("warn");
				}
			}
		}
	}

	public void UpdateHUD (string HUDState)
	{
		
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
				playerData.expState = ExpState.Warn;

				break;		
			}
		case "die":
			{

				break;

			}
		}

	}

	private FlockLevel getFlockLevel (int lvl)
	{ 
		int i = Array.FindIndex (flockLevels, w => w.level == lvl);
		return flockLevels [i];
	}

	public void LoadLevel (int _level)
	{
			
//		int i = Array.FindIndex (flockLevels, w => w.level == _level);
//	

		FlockLevel levelToLoad = getFlockLevel (_level);

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

		//		public float timeBoundaryForBugsEatenForLevelEntry;
		//		public int bugsEatenInTimeBounds;
		//		public int bugsEatenInTimeBoundsForLevelRetention;

		//Environment
		public Material environmentMaterial;

		//Avatar
		public Material avatarMaterial;

		//Bugs
		public Material bugMaterial;

		//Audio



		//ZONE

		//special actions
	}

	public class PlayerData
	{
			
		private int bugsEaten = 0;
		private List<float> bugsEatenLog;

		public int level { get; set; }
		public ExpState expState;

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
