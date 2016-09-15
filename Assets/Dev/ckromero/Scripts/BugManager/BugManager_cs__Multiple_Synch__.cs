using UnityEngine;


namespace BugJam {
	public class MultiBugBuilder : MonoBehaviour {
		public MultiBug prefab;
	   	public int gridWidth = 2;
	   	public float scale = 0.5f;

	   	public float disableTime = 1;

	   	public bool init = false; //redundant?

		void Awake(){
			for(int x=0;x<gridWidth;++x)for(int y=0;y<gridWidth;++y){
				 Vector3 pos = transform.position + new Vector3(x*scale,0,y*scale);
				 GameObject bug = Instantiate(prefab.gameObject,pos,Quaternion.identity) as GameObject;
				 bug.transform.parent = transform;
				 string label = "Bug"+x+"."+y;
				 bug.name = label;
				 bug.GetComponent<Holojam.Tools.Synchronizable>().label = label;
			}
			init = true;
		}

		public void ProcessCollision(MultiBug b){
	      if(!init)return;
	      //do something about state here
	      //eg increment 'bugs eaten' var
	   }
	}
}