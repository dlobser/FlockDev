using System;
using System.Collections;

[Serializable]
public class SettingsJSON 
{
	public float allowedSessionTime;

	public float birdColliderSize;
	public float birdMaxHeadSize;

	public float bugColliderSize;
	public float bugDeathTime;
	public float bugMaxSize;

	public bool emitFromHead;
	public bool emitFromFarthestQuadrant;
	public bool emitFromInitialGrid;
	public float experienceLengthSeconds;

	public float faderLevelsMax;

	public float graceTime;
	public string globalHeadsetUIMessage;

	public float maxSpeedToSitStill;

	public string slowMessage;

	public string timeToDieMessage;
	public float timeLeftToDie;

	public float warnForSeconds;
	public float wingLineRendererLength;

}

