using UnityEngine;
using System.Collections;
using Holojam.Tools;


public class flySync :  Synchronizable {

	protected override void Sync(){
		base.Sync ();
//		//By default syncs transform data
//		if(sending){
//			synchronizedVector3=transform.position;
//			synchronizedQuaternion=transform.rotation;
//		}
//		else{
//			transform.position=synchronizedVector3;
//			transform.rotation=synchronizedQuaternion;
//		}
	}

}
