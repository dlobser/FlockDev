using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;


public class ViveWandCTRL : MonoBehaviour
{

    public bool left;
    HandRole hand;
    public bool triggerDown;
    public bool triggerUp;

     
    // Use this for initialization
    void Start()
    {
        if (left)
        {
            hand = HandRole.LeftHand;
        }
        else
        {
            hand = HandRole.RightHand;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ViveInput.GetPress(hand, ControllerButton.Trigger))
        {
            triggerDown = true;
        }
        else if (ViveInput.GetPressUp(hand, ControllerButton.Trigger))
        {
            triggerUp = true;
        }
    }

    
}
