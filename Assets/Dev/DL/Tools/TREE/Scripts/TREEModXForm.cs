using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TREESharp{
	
	public class TREEModXForm : TREEMod {
	
		public TreeTransform xForm { get; set; }

		public string[] selectJoints;
		public string[] transformJoints;

		public float counter { get; set; }
		public float countSpeed { get; set; }

		public float timeScale = 1;

		public override void Setup(){

			rebuild = false;
			
			if (this.gameObject.GetComponent<TreeTransform> () == null) {
				xForm = this.gameObject.AddComponent<TreeTransform> ();
			} else {
				xForm = this.gameObject.GetComponent<TreeTransform> ();
//				xForm.returnToInitialState ();
			}

			xForm.Setup (selectJoints, transformJoints, tree);

			Step ();
		}
			
		public override void Animate () {
			if (animate && !rebuild)
				xForm.Animate (Time.time * timeScale);
			if (rebuild) {
				Setup ();
			}
		}

		public void Step(){
			counter += countSpeed * timeScale;
			xForm.Animate (counter);
		}

		public void Step(float offset){
			counter += offset;
			xForm.Animate (counter);
		}
			
	}
}