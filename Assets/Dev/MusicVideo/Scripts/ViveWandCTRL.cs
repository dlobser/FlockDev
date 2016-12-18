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

    public GameObject wandEnd;
    public GameObject wandScale;
    public Vector2 WandEndMinMax;

    public LineParticleStrength lineParti;
    public ControllerTweakBallStrength triggerBall;
    bool ballTriggerable = false;
     
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
            if (!lineParti.triggered)
                lineParti.triggered = true;
            if (ballTriggerable)
            {
                triggerBall.Ping();
                ballTriggerable = false;
            }
        }
        else if (ViveInput.GetPressUp(hand, ControllerButton.Trigger))
        {
            triggerUp = true;
            ballTriggerable = true;
        }
        else if (!ViveInput.GetPadPressAxis(hand).Equals(Vector2.zero))
        {
            float newZ = Mathf.Clamp(wandEnd.transform.localPosition.z + ViveInput.GetPadPressAxis(hand).y*.1f, WandEndMinMax.x,WandEndMinMax.y);
            wandEnd.transform.localPosition = new Vector3(0, 0,newZ);
            wandScale.transform.localScale = new Vector3(wandScale.transform.localScale.x, wandScale.transform.localScale.y, newZ);
        }

    }

    
}
