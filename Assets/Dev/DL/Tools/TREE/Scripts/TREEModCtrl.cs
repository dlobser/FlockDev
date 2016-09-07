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
			tree = GetComponent<TREE> ();
			for (int i = 0; i < mods.Length; i++) {
				mods [i].tree = tree;
//				Debug.Log (mods [i]);
//				Debug.Log ((mods [i].tree));
				mods [i].animate = false;
			}
			StartCoroutine(reBuild());
			rebuild = false;
		}

		IEnumerator reBuild()
		{
			//This is a coroutine
			for (int i = 0; i < mods.Length; i++) {
				mods [i].tree = tree;
				if (i > 0)
					mods [i - 1].animate = true;
				mods[i].HardReset();
				mods [i].Setup ();
				yield return null;
			}
			mods [mods.Length - 1].animate = true;
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
