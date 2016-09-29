﻿using UnityEngine;
using System.Collections;

public class LevelHandler : MonoBehaviour {

	public float timeMax;
	public float timeStartDeathClock;
	float deathClock;
	float deathCount;
	public float timer { get; set; }
	public float level;
	public float eatSpeed;
	float lerpLevel = 0;
	public float levelDelta { get; set; }
	public float bugsEaten;
	public float hungryTime = 10;
	float hungerTimer;

	public float maxLevel = 30;

	public FaderManager[] BugsEaten;
	public FaderManager[] Level;
	public FaderManager[] LerpLevel;
	public FaderManager[] LevelDelta;
	public FaderManager[] DeathCount;
	
	// Update is called once per frame
	void Update () {
		hungerTimer += Time.deltaTime;
		if (hungerTimer > hungryTime) {
			ReduceLevel ();
		}
		lerpLevel = Mathf.MoveTowards (lerpLevel, level, .01f);
		levelDelta = level - lerpLevel;
		timer += Time.deltaTime;
		if (timer > timeMax) {
			level = 0;
			deathClock = 0;
			deathCount = 0;
		}
		if (timer > timeStartDeathClock) {
			deathClock += Time.deltaTime;
			deathCount = deathClock / (timeMax - timeStartDeathClock);
		}
		UpdateFaders ();
	}

	void ReduceLevel(){
		if(level>0)
			level -= Time.deltaTime;
	}

	public void EatBug(){
		bugsEaten++;
		level++;
		hungerTimer = 0;
		if (level > maxLevel)
			level = 0;
	}

	void Debugger(){
		Debug.Log ("level: " + level);
		Debug.Log ("levelDelta: " + levelDelta);
		Debug.Log ("bugsEaten: " + bugsEaten);
		Debug.Log ("lerpLevel: " + lerpLevel);
		Debug.Log ("hungerTimer: " + hungerTimer);
	}

	void UpdateFaders(){

		foreach (FaderManager fader in BugsEaten) {
			fader.level = bugsEaten;
		}
		foreach (FaderManager fader in Level) {
			fader.level = level;
		}
		foreach (FaderManager fader in LerpLevel) {
			fader.level = lerpLevel;
		}
		foreach (FaderManager fader in LevelDelta) {
			fader.level = levelDelta;
		}
		foreach (FaderManager fader in DeathCount) {
			fader.level = deathCount;
		}

	}


}
