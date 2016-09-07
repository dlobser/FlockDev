using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TREESharp{
	public class TREESwapGeo : TREEMod {

		public string[] selectJoints;
		public TREESharp.Joint[] jointGeo;
		List<List<int[]>> selectedJoints;

		public override void Setup () {
			selectedJoints = TREEUtils.ArgsArrayToJointList (selectJoints, tree);
			swapGeo ();
		}

		public override void Animate(){
			if (rebuild) {
				Setup ();	
				rebuild = false;
			}
		}

		void swapGeo(){
			for (int i = 0; i < selectedJoints.Count; i++) {
				for (int j = 0; j < selectedJoints [i].Count; j++) {

					GameObject g = TREEUtils.findJoint (selectedJoints [i] [j], 0, tree.transform.GetChild (0).gameObject);

					GameObject duplicateScalar = Instantiate (jointGeo [i].scalar);
					Debug.Log (jointGeo [i]);
					duplicateScalar.name = "scalar_" + i + "_" + j;
					Transform t = g.GetComponent<TREESharp.Joint> ().rotator.transform;

					TREEUtils.copyTransforms (duplicateScalar, g.GetComponent<TREESharp.Joint> ().scalar.gameObject);
					duplicateScalar.transform.parent =  (t);
					Destroy (g.GetComponent<TREESharp.Joint> ().scalar.gameObject);
					g.GetComponent<TREESharp.Joint> ().scalar = duplicateScalar;

				}
			}
		}
	}
}
