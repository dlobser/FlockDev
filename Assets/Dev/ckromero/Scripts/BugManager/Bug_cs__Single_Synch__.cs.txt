using UnityEngine;
using System.Collections;

using Holojam.Tools;

namespace BugJam{ 
	public class SingleBug : MonoBehaviour{
	   SingleBugBuilder bb;
	   Renderer r;
	   public int active = 1;
	   public int index;

	   void Update(){
	      if(!Holojam.Tools.BuildManager.IsMasterPC())active = int.Parse(bb.bitties[index].ToString());
	      r.enabled = active==1;
	   }

	   void Awake(){
		  //This is okay to do in Awake, but you should really just cache it anyway
			bb = GameObject.Find("BugManager").GetComponent<SingleBugBuilder>();
	      r = GetComponent<Renderer>();
	   }

	   //None of the stuff below executes on the client.
	   void OnTriggerEnter(Collider c){
	      if(!Holojam.Tools.BuildManager.IsMasterPC() || active!=1)return;
	      Holojam.Tools.Actor a = c.GetComponent<Holojam.Tools.Actor>();
	      if(a!=null){
	         bb.SendMessage("ProcessCollision",this); //Callback
	         StartCoroutine(DisableThis());
	      }
	   }

	   IEnumerator DisableThis(){
	      active = 0;
	      yield return new WaitForSeconds(bb.disableTime);
	      active = 1;
	   } 
	}
}
