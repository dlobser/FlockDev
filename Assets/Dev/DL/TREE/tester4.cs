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

//		xForm.Setup(
//			new string[]{
//				"0|-1|-1|-1|-1|-1|-1",
//				"0|-1|-1|-1|-1",
//				"0|-1|-1",
//			},
//			new string[]{
//				"ory:90,orx:0",
//				"ssrx:-3.3,sfrx:.2,smrx:0,sMult:1.4,sorx:.5",
//				"nsrx:0,nfrx:.2,nmrx:10,nMult:1.4"
//			},tree);

	}



	// Update is called once per frame
	void Update () {
		xForm.Update ();
	}
		
}