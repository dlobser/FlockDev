using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public LineParticleStrength lineParti1;
    public LineParticleStrength lineParti2;

    public GameObject fadeSphere;
    public GameObject score;
    public GameObject DoAndDontIndicator;

    public GameObject BugManager;

    //public Animator scoreAnim;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!lineParti1.UIisActive && !lineParti2.UIisActive && DoAndDontIndicator.activeInHierarchy)
        {
            DoAndDontIndicator.SetActive(false);
            BugManager.SetActive(true);
            //scoreAnim.SetTrigger("PLAY");
        }
	}
}
