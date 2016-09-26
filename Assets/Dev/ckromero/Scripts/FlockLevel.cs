using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlockLevel
{
	public int level;
	public int bugsEatenMinimum;

	//time management
	public float bugTime;
	public int bugsNeededForTime;

	//fader for shaders
	public float globalFadeLevel;

	//fader for speeds
	public float speedFaderLevel;

	//Audio
	public string audioSnapshotName;

}

