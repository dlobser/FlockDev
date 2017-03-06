using UnityEngine;

public class ActorControllerLite : Holojam.Tools.Actor{
   public Color motif = Holojam.Utility.Palette.Select(DEFAULT_COLOR);
   public GameObject mask;

	const float POSITION_DAMPING = 5;

	Vector3 lastPosition = Vector3.zero;

   protected override void UpdateTracking(){
      base.UpdateTracking();

      if(mask!=null)
         mask.SetActive(!isBuild);
   }


   void Start(){ApplyMotif();}

   void ApplyMotif(){
      debugColor = motif;
      if(Application.isPlaying)
         foreach(Renderer r in GetComponentsInChildren<Renderer>(true))
            if(r.gameObject.tag=="Motif")r.material.color = motif;
   }

//Smooth signal while minimizing perceived latency
Vector3 Smooth(Vector3 target, ref Vector3 last){
	target = Vector3.Lerp(
		target,last,(last-target).sqrMagnitude*POSITION_DAMPING
	);
	last = target;
	return target;
}
}