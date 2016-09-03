using UnityEngine;
using Holojam.Network;
using System.Collections;

namespace Holojam.Tools{
	[ExecuteInEditMode, RequireComponent(typeof(HolojamView))]

	public class SynchronizableMomma : Synchronizable {

//		public string label = "Label";
//		public bool useMasterPC = false;
//		public bool sending = true;

		Vector3 newHeight = Vector3.zero;
		public TransformUniversal cpXform;
		public MommaBirdAnimator momma;
		float height = 0;

		bool activated = true;

		protected override void Sync(){
			//By default syncs transform data
			if(sending){
				if (!activated) {
					cpXform.enabled = true;
					momma.enabled = true;
					activated = true;
				}
				synchronizedVector3=transform.position;
				synchronizedQuaternion=transform.rotation;
			}
			else{
				if (activated) {
					cpXform.enabled = false;
					momma.enabled = false;
					activated = false;
				}
				transform.position=synchronizedVector3;
				transform.rotation=synchronizedQuaternion;
			}
		}
	}
}
