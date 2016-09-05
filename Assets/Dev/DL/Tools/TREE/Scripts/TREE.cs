﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TREE : MonoBehaviour {

	public GameObject defaultJoint;
	private bool defaultJointExists = true;

	public Trait trait;
	public int ID = 0;
	private string _name = "TREE";
	Traits traits;
	GameObject root;
	public GameObject[] jointChildren;


	public Dictionary<int[],GameObject> jointDictionary;
	int jCounter = 0;
	int[] JCounter;

	public string name {
		set{
			if (value is string)
				_name = value;
			else 
				_name = "TREE";
		}
		get{ return _name; }
	}

	Hashtable genome = new Hashtable();
		

	void Awake(){
		
		JCounter = new int[1];
		jointDictionary = new Dictionary<int[],GameObject> ();
		traits = Traits.Instance;
		traits.build ();

		if (defaultJoint == null)
			defaultJointExists = false;

		if (GetComponent<Trait> () == null)
			trait = this.gameObject.AddComponent<Trait> ();
		else
			trait = this.gameObject.GetComponent<Trait> ();


		
		trait.makeDefault ();

		Joint j = this.gameObject.AddComponent<Joint> ();
		Trait rootTrait = trait;// new Trait ();
		rootTrait.makeDefault ();
		rootTrait.jointMesh = null;
		rootTrait.ballMesh = null;
		j.trait = rootTrait;
		this.name = "root";
		root = this.gameObject;
	}

	public void setDefaultJoint(GameObject Joint){
		defaultJoint = Joint;
		if(defaultJoint!=null)
			defaultJointExists = true;

	}

	GameObject makeRoot(){
		Trait rootTrait = new Trait ();
		rootTrait.makeDefault ();
		rootTrait.jointMesh = null;
		rootTrait.ballMesh = null;
		GameObject root = makeJoint (rootTrait);
		root.name = "root";
		return root;
	}

	public GameObject makeJoint(Trait t){

		if (!defaultJointExists)
			return TREEUtils.JointFactory (t);
		else {
			GameObject G = Instantiate (defaultJoint);
			G.name = "joint_" + t.id;
			Joint J = G.GetComponent<Joint> ();
			J.setTrait (t);
			J.setScale (t.jointScale);
			J.joint = t.id;
			return G;
		}
	}

	public GameObject Branch(params object[] args){
		
		int amount = 10;
		Transform container = root.transform;

		Trait t;

		if (GetComponent<Trait> () == null)
			t = this.gameObject.AddComponent<Trait> ();
		else
			t = this.gameObject.GetComponent<Trait> ();
		
		t.Apply (trait);

		int[] dictName = new int[1];

		for (int i = 0; i < args.Length; i++) {
			if(args[i].GetType() == typeof(int))
				amount = (int)args[i];
			if(args[i].GetType() == typeof(Trait))
				t = (Trait)args[i];
			if (args [i].GetType () == typeof(Transform)) {
				container = (Transform)args [i];
			}
			if(args[i].GetType() == typeof(int[]))
				dictName = (int[])args[i];
		}
			
		t.joints = amount;
		return RecursiveAdd (amount, 0, container, t, dictName);
	}

	public GameObject RecursiveAdd (int amount ,int counter, Transform obj, Trait trait, int[] dict){
		Trait t;

		if (GetComponent<Trait> () == null)
			t = this.gameObject.AddComponent<Trait> ();
		else
			t = this.gameObject.GetComponent<Trait> ();
		
		t.Apply (trait);
		t.id = counter;
		GameObject j = makeJoint (t);
		j.transform.parent = obj;
		obj.GetComponent<Joint> ().childJoint = j;
		j.GetComponent<Joint> ().dictionaryName = dict;

		float yOffset = t.jointScale;

		if (counter == 0) {
			yOffset = 0;
		}

		j.GetComponent<Joint> ().joints = amount;

		j.GetComponent<Joint> ().offset = (int)t.offset;

		j.transform.localPosition = new Vector3 (0, yOffset, 0);

		if (counter == amount ) {
			t.endJoint = true;
			GameObject end = Instantiate (j.GetComponent<Joint> ().rotator.transform.GetChild (0).gameObject);
			end.transform.parent = j.GetComponent<Joint>().rotator.transform;
			end.transform.localPosition = new Vector3(0,t.jointScale,0);

		}
		if (counter < amount) {
			RecursiveAdd (amount, ++counter, j.transform,t,dict);
		}
		return j;
	}

	public void generate(params string[] gene){
		
		genome = GenomeUtils.fixGenome (gene);
		Genome g = GenomeUtils.getGenome (new Genome (), genome);

		Trait t;

		if (GetComponent<Trait> () == null)
			t = this.gameObject.AddComponent<Trait> ();
		else
			t = this.gameObject.GetComponent<Trait> ();
		
		t.Apply (trait);
		GameObject tempRoot = makeJoint (t);
		tempRoot.transform.parent = transform;


		for (int i = 0; i < g.rads[0]; i++) {

			t.jointScale = g.length [0];

			GameObject thisRoot = Branch ((int)g.joints [0]-1,tempRoot.transform, t);
			thisRoot.transform.Rotate( new Vector3 (0, i * 360 / g.rads [0], g.angles [0]));

			tempRoot.GetComponent<Joint> ().limbs.Add (thisRoot);

			this.GetComponent<Joint>().limbs.Add(thisRoot);

			if(g.joints.Length>1)
				recursiveBranch (g, 1, thisRoot);
		}

	}

	public void recursiveBranch(Genome g,  int counter, GameObject joint){

		GameObject newBranch, kidJoint;

		int end = (int)g.end [counter];
		if ((int)g.end [counter] == -1)
			end = joint.GetComponent<Joint> ().joints+ 1;

		if (joint.GetComponent<Joint> ().joints < end)
			end = joint.GetComponent<Joint> ().joints + 1;

		for (int i = (int)g.start[counter]; i < end ; i+=(int)g.divs[counter]) {
			
			kidJoint = TREEUtils.findJointOnBranch (joint, i);
			kidJoint.name = "branchRoot_"+i;

			int[] jcounter = {++jCounter};
			jointDictionary.Add (jcounter, kidJoint);

			for (int j = 0; j < (int)g.rads[counter]; j++) {

				Trait t;

				if (GetComponent<Trait> () == null)
					t = this.gameObject.AddComponent<Trait> ();
				else
					t = this.gameObject.GetComponent<Trait> ();
				
				t.Apply(trait);

				t.offset = kidJoint.gameObject.GetComponent<Joint> ().joint;

				t.jointScale = g.length [counter];

				newBranch = Branch ((int)g.joints [counter]-1, t);
				newBranch.transform.parent = kidJoint.transform;
				TREEUtils.zeroTransforms (newBranch);
				newBranch.transform.localEulerAngles = ( new Vector3 (0, j * 360 / g.rads [counter], g.angles [counter]));
				newBranch.transform.localPosition = (new Vector3 (0,kidJoint.GetComponent<Joint> ().trait.jointScale,0));
				kidJoint.GetComponent<Joint> ().limbs.Add (newBranch);

			}
			if (counter+1 < g.joints.Length) {
				for (int k = 0; k < (int)kidJoint.GetComponent<Joint> ().limbs.Count; k++) {
					recursiveBranch (g, counter + 1, kidJoint.GetComponent<Joint> ().limbs [k]);
				}
			}
		}
	}
}
