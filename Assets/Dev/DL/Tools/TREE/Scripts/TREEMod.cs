using UnityEngine;
using System.Collections;

namespace TREESharp{
	public class TREEMod : MonoBehaviour {

		public TREE tree;
		public bool rebuild = false;
		public bool animate = false;
		public virtual void Setup (){
		}
		public virtual void HardReset(){
		}
		public virtual void Animate (){
		}
	}
}
