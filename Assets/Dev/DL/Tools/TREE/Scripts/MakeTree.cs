using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MakeTree : MonoBehaviour {
	
	TREE tree;
	public GameObject defaultJoint;
	public TreeTransform xForm { get; set; }

	public string joints= "10",rads= "1",angles= "0",length= "1",divs= "1",start = "0",end = "-1";

	public string[] selectJoints;
	public string[] transformJoints;

	public bool animate = true;

	public float counter { get; set; }
	public float countSpeed { get; set; }

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
		if (animate)
			xForm.Animate (Time.time);
		else
			Animate ();
	}

	public void Animate(){
		counter += countSpeed;
		xForm.Animate (counter);
	}
		
}