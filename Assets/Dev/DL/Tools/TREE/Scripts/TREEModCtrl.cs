using UnityEngine;
using System.Collections;

namespace TREESharp{
	public class TREEModCtrl : MonoBehaviour {

		public TREEMod[] mods;
		public bool rebuild = true;
		public bool animate = true;
		TREE tree;

		public void Start () {
//			mods = GetComponents<TREEMod> ();
			tree = GetComponent<TREE> ();

		}

		public void Build(){
			for (int i = 0; i < mods.Length; i++) {
				mods [i].tree = tree;
				mods [i].animate = false;
				mods [i].Setup ();
			}
			rebuild = false;
		}
		
		// Update is called once per frame
		public void Update () {
			if (rebuild) {
				Build ();
			}
			if (animate) {
				for (int i = 0; i < mods.Length; i++) {
					mods [i].Animate ();
				}
			}
		}
	}
}
