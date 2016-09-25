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

	//Environment
	public Material environmentMaterial;

	//Avatar
	public Material avatarMaterial;

	//Bugs
	public Material bugMaterial;

	//Audio
	public string audioSnapshotName;

	//Faded Object
	public float faderLevel;
}

