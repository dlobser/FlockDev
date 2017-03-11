using System;
using System.Collections;
using System.Collections.Generic;
using Holojam.Tools;
using Holojam.Network;
using UnityEngine;

public class ActorSyncer : Controller
{
  public override void ResetData() {
    data = new Flake(0, 0, 0, 0, 0, true);
  }

  public override string Label{get{return "ActorSyncer";}}
  public override bool Sending{get{return BuildManager.IsMasterClient();}}

  private ActorSetJSON capturedASJ;
  private ActorSetJSON revisedASJ;
  private ActorJson naj;
  private List<ActorJson> ls;
  private List<int> bugsEatenTime;
  private string cachedSync = "";
  private int cachedBugsEaten = 0;
  // Use this for initialization
  void Start ()
  {
    resetSync();
    capturedASJ = new ActorSetJSON ();
    revisedASJ = new ActorSetJSON ();
    naj = new ActorJson ();
    ls = new List<ActorJson> ();
    bugsEatenTime = new List<int> ();

  }

  public void PrintSyncString ()
  {
    DebugX.Log (data.text);
  }

  public void resetSync ()
  { 
    ResetData();
  }

  public void SendSyncStringData (string s)
  { 
    if (s != null && s != "") {  

      //search the current synchronized string and see if any data is associated with the current actor (data).
      if (data.text != null && data.text != "") {
        capturedASJ = JsonUtility.FromJson<ActorSetJSON> (data.text);
        int casjLength = capturedASJ.actors.Length;
        bool foundExistingActor = false;
        for (int i = 0; i < casjLength; i++) {
          if (capturedASJ.actors [i].actorIndex == s) {
            capturedASJ.actors [i].bugsEaten++; 
            capturedASJ.actors [i].bugTime.Add ((int)Time.time);
            revisedASJ = capturedASJ;
            foundExistingActor = true;
            break;
          }
        }

        //data is already stored for other actors but not for this one yet. 
        if (!foundExistingActor) {
//          naj = default(ActorJson);

          naj.actorIndex = s;
          naj.bugsEaten = 1;
          naj.bugTime = new List<int> ();
          naj.bugTime.Add ((int)Time.time);

//          List<ActorJson> ls = new List<ActorJson> ();
          ls.Clear(); 
          ls.Add (naj);
          int ct = capturedASJ.actors.Length;

          for (int i = 0; i < ct; i++) {
            ls.Add (capturedASJ.actors [i]);
          }

          revisedASJ.actors = ls.ToArray ();
        }
      } else { 
        //initiate the synchronized string by adding the current actor
//        ActorJson naj = new ActorJson ();
//        naj = default(ActorJson);
        naj.actorIndex = s;
        naj.bugsEaten = 1;
        naj.bugTime = new List<int> ();
        naj.bugTime.Add ((int)Time.time);

        ls.Add (naj);
      revisedASJ.actors = ls.ToArray ();
      }

      data.text = JsonUtility.ToJson(revisedASJ);
      PrintSyncString ();
    }
  }

  public void ResetActor (string currentActor)
  { 
    ActorSetJSON newASJ =  new ActorSetJSON ();
    //remove the actor from the syncstring, all instances just in case!
    if (data.text != null && data.text != "") {
      capturedASJ = JsonUtility.FromJson<ActorSetJSON> (data.text);
//      newASJ = capturedASJ;
//      newASJ.actors.Initialize();
//      revisedASJ=default(revisedASJ);
      int casjLength = capturedASJ.actors.Length;

      //TODO: ActorJSON value array needs thought
      newASJ.actors = new ActorJson[casjLength-1];

      int j=0;
      for (int i = 0; i < casjLength; i++) {
        if (capturedASJ.actors [i].actorIndex != currentActor) {
          newASJ.actors [j] = capturedASJ.actors [i];
          j++;
        }
      } 
    }
    data.text = JsonUtility.ToJson(newASJ);
    PrintSyncString ();
  }

  public int checkBugsEaten (string actor)
  { 
    if (data.text != cachedSync) {
      capturedASJ = default (ActorSetJSON); 
      if (data.text != null && data.text != "") {
        capturedASJ = JsonUtility.FromJson<ActorSetJSON> (data.text);
        int casjLength = capturedASJ.actors.Length;
        for (int i = 0; i < casjLength; i++) {
          if (capturedASJ.actors [i].actorIndex == actor) {
            cachedBugsEaten = capturedASJ.actors [i].bugsEaten; 
          }
        }
      }
      cachedSync = data.text;
    } 

    return cachedBugsEaten;
  }

  public int checkBugsEatenSince (string actor, float sinceTime)
  {
    //access actor 
    capturedASJ = default( ActorSetJSON );
    bugsEatenTime = default (List<int>);
    bool foundTimeList = false;

    if (data.text != null && data.text != "") {
      capturedASJ = JsonUtility.FromJson<ActorSetJSON> (data.text);
      int casjLength = capturedASJ.actors.Length;
      for (int i = 0; i < casjLength; i++) {
        if (capturedASJ.actors [i].actorIndex == actor) {
          bugsEatenTime = capturedASJ.actors [i].bugTime; 
          foundTimeList = true;
        }
      }
    }
        
    if (!foundTimeList) {
      return 0;
    }

    int bugTimeLength = bugsEatenTime.Count;
    int bugsEatenSinceTimeCounter = 0;
    float timeWindow = Time.time - sinceTime; 

    for (int i = 0; i < bugTimeLength; i++) {
      if (bugsEatenTime [i] > timeWindow) {
        bugsEatenSinceTimeCounter++;
      }
    }
    return bugsEatenSinceTimeCounter;
  }
}
