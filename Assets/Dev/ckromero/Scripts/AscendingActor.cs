﻿using UnityEngine;
using System.Collections;
using Holojam.Tools;

public class AscendingActor : Actor {

	private Vector3 somewhere = new Vector3();
	private Vector3 startPos;
	private bool isMoving=false;


	void Start() { 
		somewhere = Vector3.zero;
		startPos = somewhere;
	}
	void Update(){
		if (isMoving) { 
			somewhere.y += 0.1f;
			Debug.Log ("somewhere.y is " + somewhere.y);
		}
		if (somewhere.y > 3.0f) {
			isMoving = false;
		}

	}

	public override Vector3 eyes{

		get{
			isMoving = true;
			return somewhere;}
	
	}

}


