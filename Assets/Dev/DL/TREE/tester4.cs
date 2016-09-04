using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tester4 : MonoBehaviour {
	
	TREE tree;
	public GameObject defaultJoint;
	TreeTransform xForm;

	public string joints,rads,angles,length,divs,start,end;

	public string[] selectJoints;
	public string[] transformJoints;

	// Use this for initialization
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
		
	// Update is called once per frame
	void Update () {
		xForm.Animate (Time.time);
	}
		
}