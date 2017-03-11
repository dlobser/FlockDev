using UnityEngine;
using System.Collections;
using Holojam.Tools;

public class Bug : SynchronizableTrackable {
  public Vector3 lastPosition { get; set; }
  public Vector3 origin { get; set; }
  public float scaleSpeed = .1f;
  public int collisionWindow=2;

  public string label; // hack

  public override bool Host { get { return false; } }
  public override bool AutoHost { get { return true; } }

  public override string Label { get { return label; } }

  Renderer[] renderers;
  BugManager bb;
  Vector3 scalar = Vector3.one;
  ActorDataSync actorDataSync;
  public MaterialSwapperContinuous materialSwapper;

  float scale;
  float initScale;
  int active = 1;
  bool doneScalingDown = false;
  bool doneScalingUp = false;
  bool collisionAllowed = true;
  bool materialSwapAllow = false;

  int prevEmit = -1;
  int emit = 0;

  protected override void Awake() {
    base.Awake();

    //Okay to do in Awake
    bb = GameObject.Find("BugManager").GetComponent<BugManager>();

    renderers = GetComponentsInChildren<Renderer> ();
    scale = this.transform.localScale.x;
    initScale = scale;


    //prep for reporting on actor collisions
    actorDataSync = GameObject.Find ("ActorSynchronizableManager").GetComponent<ActorDataSync> ();

  }

  public override void ResetData() {
    data = new Holojam.Network.Flake(2, 1, 0, 2);
  }

  protected override void Sync() {
    base.Sync();

    if (Sending) {
      data.vector3s[1] = transform.localScale;
      data.ints[0] = active;
      data.ints[1] = emit;
    } else {
      transform.localScale = data.vector3s[1];
      active = data.ints[0];
      emit = data.ints[1];
    }

    if (prevEmit == 0 && emit == 1) {
      bb.SendMessage("ProcessCollision", this); //Callback
      bb.SendMessage("SwapTexture", this); //Callback
      bb.SendMessage("LevelUp", this);
    }

    prevEmit = emit;

    int bugsEaten = actorDataSync.ActorBugsEaten(); 
//    if (bugsEaten>2){
//      if (materialSwapAllow) {
//        materialSwapper.swapMat ();
//        Debug.Log (this.name + " swapped to material " + renderers [0].material.name);
//        materialSwapAllow = false;
//      }
//    }

    ActivateDeactivateRenderer ();
  }

  void ActivateDeactivateRenderer(){
    foreach (Renderer r in renderers) {
      r.enabled = active==1;
    }
  }

  //Nothing below here executes on the client.
  void OnTriggerEnter(Collider c){

    if(!Sending || active!=1)return;
    Holojam.Tools.Actor a = c.GetComponent<Holojam.Tools.Actor>();

    if(a!=null){

      if( c.GetComponent<SphereCollider> ()!=null);
        c.GetComponent<SphereCollider> ().enabled = false;
//      if( c.GetComponent<BoxCollider> ()!=null);
//        c.GetComponent<BoxCollider> ().enabled = false;
//      Debug.Log ("Poof");

      // add to bugsEaten for the colliding actor!!!!
      if (Holojam.Tools.BuildManager.IsMasterClient()) {
        if (Time.time > 1 && collisionAllowed ) {
          StartCoroutine(CloseCollisionWindow ());
          if (c.name != null && c.name!="") {
//            Debug.Log (this.name + " collided with " + c.transform.parent.name + " at " + Time.time);
            actorDataSync.UpdateActor( c.name,this.name,1);
          }
        }
      } 
      emit = 1;
      //This handles particle placement
      bb.SendMessage("ProcessCollision", this); //Callback
      StartCoroutine(DisableThis());
    }
  }

  //trying to prevent multiple collisions against the same object. 
  //Could also try looking at the same objects within a given window.
  IEnumerator CloseCollisionWindow() { 
    collisionAllowed = false;
    yield return new WaitForSeconds (collisionWindow);
    collisionAllowed = true;
  }

  IEnumerator DisableThis(){
    while(!doneScalingDown){
      if (scale > .001f) {
        scale = Mathf.MoveTowards(scale, 0f, scaleSpeed);
        scalar.Set (scale, scale, scale);
        this.transform.localScale = scalar;
        yield return null;
      } else
        doneScalingDown = true;
    }
    active = 0;
    doneScalingDown = false;
    yield return new WaitForSeconds(bb.disableTime);
    bb.SendMessage ("ResetPosition", this);
    materialSwapper.swapMat ();
    active = 1;
    emit = 0;
    while (!doneScalingUp) {
      if (scale < initScale) {
        scale = Mathf.MoveTowards (scale, initScale, scaleSpeed);
        scalar.Set (scale, scale, scale);
        this.transform.localScale = scalar;
        yield return null;
      } else
        doneScalingUp = true;
    }
    doneScalingUp = false;
    scale = initScale;


  }
}
