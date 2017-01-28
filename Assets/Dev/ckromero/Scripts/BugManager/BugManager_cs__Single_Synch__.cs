using UnityEngine;
using Holojam.Tools;

namespace BugJam
{
	public class SingleBugBuilder : Synchronizable
	{
		protected override bool hasText{get{return true;}}

		public SingleBug prefab;
		public int gridWidth = 2;
		public float scale = 0.5f;

		public float disableTime = 1;

		public bool init = false;
		//redundant?

		public SingleBug[] bugs;
		public string bitties;

		void Awake ()
		{
			if (!Application.isPlaying)
				return;
			bugs = new SingleBug[gridWidth * gridWidth];
			for (int x = 0; x < gridWidth; ++x)
				for (int y = 0; y < gridWidth; ++y) {
					Vector3 pos = transform.position + new Vector3 (x * scale, 0, y * scale);
					GameObject bug = Instantiate (prefab.gameObject, pos, Quaternion.identity) as GameObject;
					bug.transform.parent = transform;
					string label = "Bug" + x + "." + y;
					bug.name = label;
					bugs [x + gridWidth * y] = bug.GetComponent<SingleBug> ();
					bug.GetComponent<SingleBug> ().index = x + gridWidth * y;
				}
			init = true;
		}

		protected override void Sync ()
		{
			if (sending) {
				bitties = "";
				foreach (SingleBug b in bugs)
					bitties += b.active.ToString ();
				UpdateText(bitties);
			} else {
				bitties = GetText();
			}
		}

		public void ProcessCollision (SingleBug b)
		{
			if (!init)
				return;
			//do something about state here
			//eg increment 'bugs eaten' var
		}
	}
}

