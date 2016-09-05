using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeTransform : MonoBehaviour {


	List<Dictionary<string, float>> Transforms = new List<Dictionary<string, float>>();
	List<List<int[]>> SelectedJoints = new List<List<int[]>>();
	GameObject root;
	List<List<Vector3>> initialRotation = new List<List<Vector3>>();

	Vector3 sinRotate = Vector3.zero;
	Vector3 noiseRotate = Vector3.zero;
	Vector3 rotateOffset = Vector3.zero;

	void makeDefaults(int amount){
		for (int i = 0; i < amount; i++) {
			Transforms.Add (new Dictionary<string,float> ());
			Transforms[i].Add ("rx", 0);
			Transforms[i].Add ("ry", 0);
			Transforms[i].Add ("rz", 0);

			//offset rotation
			Transforms[i].Add ("orx", 0);
			Transforms[i].Add ("ory", 0);
			Transforms[i].Add ("orz", 0);

			Transforms[i].Add ("scale", 1);
			Transforms[i].Add ("sx", 1);
			Transforms[i].Add ("sy", 1);
			Transforms[i].Add ("sz", 1);

			Transforms[i].Add ("sMult", 0);
			//sin offset from root offset
			Transforms[i].Add ("srorx", 0);
			Transforms[i].Add ("srory", 0);
			Transforms[i].Add ("srorz", 0);
			//sin offset
			Transforms[i].Add ("sorx", 0);
			Transforms[i].Add ("sory", 0);
			Transforms[i].Add ("sorz", 0);
			//sin frequency
			Transforms[i].Add ("sfrx", 0);
			Transforms[i].Add ("sfry", 0);
			Transforms[i].Add ("sfrz", 0);
			//sin speed
			Transforms[i].Add ("ssrx", 0);
			Transforms[i].Add ("ssry", 0);
			Transforms[i].Add ("ssrz", 0);
			//sin multiply
			Transforms[i].Add ("smrx", 0);
			Transforms[i].Add ("smry", 0);
			Transforms[i].Add ("smrz", 0);

			//noise joint multiply
			Transforms[i].Add ("nMult", 0);
			//noise offset
			Transforms[i].Add ("nrorx", 0);
			Transforms[i].Add ("nrory", 0);
			Transforms[i].Add ("nrorz", 0);
			//noise offset
			Transforms[i].Add ("norx", 0);
			Transforms[i].Add ("nory", 0);
			Transforms[i].Add ("norz", 0);
			//noise frequency	 
			Transforms[i].Add ("nfrx", 0);
			Transforms[i].Add ("nfry", 0);
			Transforms[i].Add ("nfrz", 0);
			//noise speed		 
			Transforms[i].Add ("nsrx", 0);
			Transforms[i].Add ("nsry", 0);
			Transforms[i].Add ("nsrz", 0);
			//noise multiply	  
			Transforms[i].Add ("nmrx", 0);
			Transforms[i].Add ("nmry", 0);
			Transforms[i].Add ("nmrz", 0);

			//colors
			Transforms[i].Add ("cr", 1);
			Transforms[i].Add ("cg", 1);
			Transforms[i].Add ("cb", 1);
		}
	}

	public void Setup(string[] joints, string[] args, TREE tree){

		makeDefaults (joints.Length);
		root = tree.gameObject;

		for (int i = 0; i < joints.Length; i++) {

			List<int[]> firstList = TREEUtils.makeList  (joints[i], tree.GetComponent<TREE>());
			List<Vector3> initRotations = new List<Vector3>();

			for (int p = 0; p < firstList.Count; p++) {
				GameObject g = TREEUtils.findJoint (firstList[p], 0, root.transform.GetChild (0).gameObject);
				initRotations.Add(g.transform.localEulerAngles);
			}

			initialRotation.Add (initRotations);	
			SelectedJoints.Add(firstList);

			string[] arg = args[i].Split (new string[] { "," }, System.StringSplitOptions.None);
			for (int j = 0; j < arg.Length; j++) {
				string[] a = arg[j].Split (new string[] { ":" }, System.StringSplitOptions.None);
				if(a.Length>1)
					Transforms[i] [a [0]] = float.Parse (a [1]);
			}
		}
	}

	public void updateValue(int which, string control, float value){
		Transforms [which] [control] = value;
	}

	public void Animate(float timer){
		for (int i = 0; i < SelectedJoints.Count; i++) {
			for (int j = 0; j < SelectedJoints [i].Count; j++) {


				GameObject g = TREEUtils.findJoint (SelectedJoints [i] [j], 0, root.transform.GetChild (0).gameObject);
				int jointNumber = g.GetComponent<Joint> ().joint;
				int jointOffset = g.GetComponent<Joint> ().offset;
				Vector3 init = initialRotation [i] [j];

				sinRotate = Vector3.zero;
             	noiseRotate = Vector3.zero;
				rotateOffset = Vector3.zero;

				Vector3 rotate = new Vector3 (
					Transforms[i]["rx"],
					Transforms[i]["ry"],
					Transforms[i]["rz"]
				);
					

				if(Transforms [i] ["smrx"]!=0||Transforms [i] ["smry"]!=0||Transforms [i] ["smrz"]!=0)
					sinRotate.Set(
						((Transforms [i] ["sMult"]*jointNumber)+Transforms [i] ["smrx"]) * Mathf.Sin (((Transforms [i] ["ssrx"] * timer + Transforms [i] ["sorx"] + (Transforms [i] ["srorx"]*jointOffset)) + (Transforms [i] ["sfrx"] * jointNumber))),
						((Transforms [i] ["sMult"]*jointNumber)+Transforms [i] ["smry"]) * Mathf.Sin (((Transforms [i] ["ssry"] * timer + Transforms [i] ["sory"] + (Transforms [i] ["srory"]*jointOffset)) + (Transforms [i] ["sfry"] * jointNumber))),
						((Transforms [i] ["sMult"]*jointNumber)+Transforms [i] ["smrz"]) * Mathf.Sin (((Transforms [i] ["ssrz"] * timer + Transforms [i] ["sorz"] + (Transforms [i] ["srorz"]*jointOffset)) + (Transforms [i] ["sfrz"] * jointNumber)))
					);
				if(Transforms [i] ["nmrx"]!=0||Transforms [i] ["nmry"]!=0||Transforms [i] ["nmrz"]!=0)
					noiseRotate.Set (
						((Transforms [i] ["nMult"]*jointNumber)+Transforms [i] ["nmrx"]) * TREEUtils.Noise (((Transforms [i] ["nsrx"] * -timer) + Transforms [i] ["norx"] +  (Transforms [i] ["nrorx"]*jointOffset) + (Transforms [i] ["nfrx"] * jointNumber))),
						((Transforms [i] ["nMult"]*jointNumber)+Transforms [i] ["nmry"]) * TREEUtils.Noise (((Transforms [i] ["nsry"] * -timer) + Transforms [i] ["nory"] +  (Transforms [i] ["nrory"]*jointOffset) + (Transforms [i] ["nfry"] * jointNumber))),
						((Transforms [i] ["nMult"]*jointNumber)+Transforms [i] ["nmrz"]) * TREEUtils.Noise (((Transforms [i] ["nsrz"] * -timer) + Transforms [i] ["norz"] +  (Transforms [i] ["nrorz"]*jointOffset) + (Transforms [i] ["nfrz"] * jointNumber)))
					);

				rotateOffset.Set (Transforms [i] ["orx"] * timer, Transforms [i] ["ory"] * timer, Transforms [i] ["orz"] * timer);
				g.transform.localEulerAngles = rotate+init+sinRotate+noiseRotate+rotateOffset;

//				g.transform.Rotate (rotateOffset.x,rotateOffset.y,rotateOffset.z);
				Vector3 overallScale = 	new Vector3 (Transforms [i] ["scale"], Transforms [i] ["scale"], Transforms [i] ["scale"]);

				if (Transforms [i] ["cr"] != 1 || Transforms [i] ["cg"] != 1 || Transforms [i] ["cb"] != 1)
					g.GetComponent<Joint>().scalar.transform.GetChild(0).GetComponent<MeshRenderer> ().material.color = 
						new Color (
							Random.Range( Transforms [i] ["cr"],1), 
							Random.Range( Transforms [i] ["cg"],1), 
							Random.Range( Transforms [i] ["cb"],1));

				if(!overallScale.Equals(Vector3.one))
					g.transform.localScale = overallScale;// Vector3.Scale(overallScale , new Vector3 (Transforms [i] ["sx"], Transforms [i] ["sy"], Transforms [i] ["sz"]));
			}
		}

	}
	
}
