//ActorController.cs
//Created by Aaron C Gaudette on 05.07.16
//Example Actor extension

using UnityEngine;
using Holojam.Tools;

public class ActorBirdController : Holojam.Tools.Actor{
	public Transform head;
   public Color motif;

	protected override void UpdateTracking(){
		if(view.tracked){
//			transform.position=trackedPosition;
			
			//This example type uses a separate transform for rotation (a head) instead of itself
			if(head!=null){
				head.localPosition=trackedPosition;
				head.rotation=trackedRotation;
			}
			else Debug.LogWarning("ActorController: No head found for "+gameObject.name);
		}
	}

	public override Vector3 center
    {
		get{return trackedPosition;}
	}
	public override Quaternion orientation{
		get{return trackedRotation;}
	}

	//The orientation accessor matches the rotation assignment above
//	public override Quaternion orientation{
//		get{return head!=null?head.rotation:Quaternion.identity;}
//	}
	
	//Assign color of geometry with Motif tag
	void Start(){ApplyMotif();}
	
	void ApplyMotif(){
		if(Application.isPlaying)
			foreach(Renderer r in GetComponentsInChildren<Renderer>())
				if(r.gameObject.tag=="Motif")r.material.color=motif;
	}
}