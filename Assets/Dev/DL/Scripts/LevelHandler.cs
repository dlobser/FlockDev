﻿using UnityEngine;
using System.Collections;

public class LevelHandler : MonoBehaviour {

	public float timeMax;
	public float timeStartDeathClock;
	public float timer { get; set; }
	public float level;
	public float eatSpeed;
	public float levelDelta { get; set; }
	public float bugsEaten;
	public float hungryTime = 10;

	public float maxLevel = 30;

	public FaderManager[] BugsEaten;
	public FaderManager[] Level;
	public FaderManager[] LerpLevel;
	public FaderManager[] LevelDelta;
	public FaderManager[] DeathCount;

	float deathClock;
	float deathCount;
	float lerpLevel = 0;
	float hungerTimer;
	float reduceLevelCounter = 0;

    bool upDown = true;
	// Update is called once per frame

	void Update () {
		
		hungerTimer += Time.deltaTime;
		if (hungerTimer > hungryTime) {
			ReduceLevel ();
		}

		levelDelta = level - lerpLevel;
		lerpLevel = Mathf.MoveTowards (lerpLevel, level, .1f);

		timer += Time.deltaTime;


		if (timer > timeStartDeathClock && timer < timeMax) {
			deathClock += Time.deltaTime;
			deathCount = Mathf.Min(1.0f,deathClock / (timeMax - timeStartDeathClock));
		}

		UpdateFaders ();

	}

	void ReduceLevel(){
		
		if (level > 0) {
			reduceLevelCounter += Time.deltaTime;
			if (reduceLevelCounter >= 1) {
				reduceLevelCounter = 0;
				level -= 1;
			}
		}

	}

	public void EatBug(){
		//bugsEaten++;
        changeLevel();
		hungerTimer = 0;
	}

    public void changeLevel()
    {
        if (upDown && level < maxLevel)
        {
            bugsEaten++;
            level++;
        }
        if (!upDown && level > 0)
        {
            bugsEaten--;
            level--;
        }
        if (level >= maxLevel || level <= 0)
        {
            upDown = !upDown;
        }
    }

    public void Reset(){
		level = 0;
		deathClock = 0;
		deathCount = 0;
		timer = 0;
		bugsEaten = 0;
		lerpLevel = 0;
		hungerTimer = 0;
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
            if (fader != null)

                fader.level = bugsEaten;
		}
		foreach (FaderManager fader in Level) {
            if (fader != null)

                fader.level = level;
		}
		foreach (FaderManager fader in LerpLevel) {
            if (fader != null)

                fader.level = lerpLevel;
		}
		foreach (FaderManager fader in LevelDelta) {
            if(fader!=null)
			    fader.level = levelDelta;
		}
		foreach (FaderManager fader in DeathCount) {
            if (fader != null)

                fader.level = deathCount;
		}

	}


}
