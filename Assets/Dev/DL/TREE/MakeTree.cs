using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MakeTree : MonoBehaviour {
	
	TREE tree;
	public GameObject defaultJoint;
	TreeTransform xForm;

	public string joints= "10",rads= "1",angles= "0",length= "1",divs= "1",start = "0",end = "-1";

	public string[] selectJoints;
	public string[] transformJoints;

	void Start () {

		tree = this.gameObject.AddComponent<TREE>();
		tree.setDefaultJoint( defaultJoint);

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