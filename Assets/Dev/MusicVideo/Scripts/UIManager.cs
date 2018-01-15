using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public LineParticleStrength lineParti1;
//    public LineParticleStrength lineParti2;

    public GameObject fadeSphere;
    public GameObject score;
    public GameObject DoAndDontIndicator;

    public GameObject BugManager;

    public int pressed = 0;

    public GameObject[] hideLeftTrigger;
    public GameObject[] showLeftTrigger;

//    public GameObject[] hideRightTrigger;
//    public GameObject[] showRightTrigger;

    bool leftHandled = false;
//    bool rightHandled = false;

    public GameObject[] hideBothTriggers;
    public GameObject[] showBothTriggers;

    public WaveVR_TrackedButtons LeftCTRL;
//    public ViveWandCTRL rightCTRL;

    //public Animator scoreAnim;

    // Use this for initialization
    void Start () {
		
	}

    public void handleLeft() {
        for (int i = 0; i < hideLeftTrigger.Length; i++)
        {
            hideLeftTrigger[i].SetActive(false);
        }
        for (int i = 0; i < showLeftTrigger.Length; i++)
        {
            showLeftTrigger[i].SetActive(true);
        }
//		lineParti1.triggered = true;
        leftHandled = true;
    }

    public void handleBoth()
    {
        for (int i = 0; i < hideBothTriggers.Length; i++)
        {
            hideBothTriggers[i].SetActive(false);
        }
        for (int i = 0; i < showBothTriggers.Length; i++)
        {
            showBothTriggers[i].SetActive(true);
        }

    }

//    public void handleRight() {
//
//        for (int i = 0; i < hideRightTrigger.Length; i++)
//        {
//            hideRightTrigger[i].SetActive(false);
//        }
//        for (int i = 0; i < showRightTrigger.Length; i++)
//        {
//            showRightTrigger[i].SetActive(true);
//        }
//        rightHandled = true;
//    }
	
	// Update is called once per frame
	void Update () {

//        if(!rightHandled && rightCTRL.triggerDown)
//        {
//            handleRight();
//        }

        if (!leftHandled && LeftCTRL.triggerPressed)
        {
            handleLeft();
        }

//		Debug.Log (lineParti1.UIisActive);

        if (!lineParti1.UIisActive && DoAndDontIndicator.activeInHierarchy)
        {
            handleBoth();
            DoAndDontIndicator.SetActive(false);
            BugManager.SetActive(true);
            //scoreAnim.SetTrigger("PLAY");
        }
	}
}
