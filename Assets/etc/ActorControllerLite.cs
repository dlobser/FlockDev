using UnityEngine;

public class ActorControllerLite : Holojam.Tools.Actor{
   public Color motif = Holojam.Utility.Palette.Select(DEFAULT_COLOR);
   public GameObject mask;

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
}