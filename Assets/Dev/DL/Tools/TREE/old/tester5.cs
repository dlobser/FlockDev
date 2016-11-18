using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TREESharp;

public class tester5: MonoBehaviour {
	
	TREE tree;
	public GameObject defaultJoint;
	TreeTransform xForm;

	public string joints,rads,angles,length,divs,start,end;

	public string[] selectJoints;
	public string[] transformJoints;

	void Start () {

		tree = this.gameObject.AddComponent<TREE>();
		tree.setDefaultJoint( defaultJoint);

		Debug.Log (
			"joints"+	joints+
			"rads"+	rads+
			"angles"+	angles+
			"length"+	length+
			"divs"+		divs+
			"start"+	start+
			"end"+		end
		);

		tree.generate (
			"joints",	joints,
			"rads",		rads,
			"angles",	angles,
			"length",	length,
			"divs",		divs,
			"start",	start,
			"end",		end
		);
	
		xForm = new TreeTransform ();

		xForm.Setup(selectJoints,transformJoints,tree);

	}
		
	void Update () {
		xForm.Animate (Time.time);
	}
		
}