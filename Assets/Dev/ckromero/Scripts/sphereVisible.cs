﻿using UnityEngine;
using System.Collections;

public class sphereVisible : MonoBehaviour {
	public MeshRenderer mr;



	void Start () {
		if(Holojam.Tools.BuildManager.IsMasterPC ()){
			mr=GetComponent<MeshRenderer>();
			mr.enabled=false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
