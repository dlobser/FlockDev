using Holojam.Tools;
using UnityEngine;
using System.Collections;

namespace BugJam {
	public class MultiBug : Synchronizable{
		MultiBugBuilder bb;
	   Renderer r;
	   int active = 1;

	   protected override void Sync(){
	      if(sending){
	         synchronizedVector3=transform.position;

	         synchronizedInt = active;
	      }
	      else{
	         transform.position=synchronizedVector3;

	         active = synchronizedInt;
	      }

	      r.enabled = active==1;
	   }

	   void Awake(){
		  //Okay to do in Awake
			bb = GameObject.Find("BugManager").GetComponent<MultiBugBuilder>();
	      r = GetComponent<Renderer>();
	   }

	   //Nothing below here executes on the client.
	   void OnTriggerEnter(Collider c){
	      if(!sending || active!=1)return;
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