using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wvr;
using WaveVR_Log;

public class WaveVR_ControllerLoader : MonoBehaviour {
    private static string LOG_TAG = "WaveVR_ControllerLoader";
    public enum ControllerHand
    {
        Controller_Right,
        Controller_Left
    };

    public enum CTrackingSpace
    {
        CTS_3DOF,
        CTS_6DOF
    };

    public enum CProject
    {
        Link
    };

    public enum ControllerModel  // temp solution
    {
        Ximmerse
    };

    public enum CComponent
    {
        COM_OneBone,
        COM_MultiComponent
    };

    public CTrackingSpace TrackingMethod = CTrackingSpace.CTS_6DOF;   // If we can get value from runtime or assigned by developer 
    public ControllerHand WhichHand = ControllerHand.Controller_Right;
    public CProject WhichProject = CProject.Link; // If we can get value from runtime or assigned by developer 
    public ControllerModel WhichModel = ControllerModel.Ximmerse; // Must get from runtime/device service, TODO!
    public CComponent CTRComponents = CComponent.COM_MultiComponent;
    //public Vector3 positionOffset = new Vector3(0.0f, 0.0f, 0.0f);
    //public Quaternion rotationOffset = Quaternion.identity;

    private GameObject controllerPrefab = null;
    private GameObject originalControllerPrefab = null;
    private string controllerFileName = "";
    private string controllerModelFoler = "Controller/";
    private string genericControllerFileName = "Generic_";
    // Use this for initialization
    void Start () {
        // Make up file name
        // Rule = 
        // Project_ControllerModel_TrackingMethod_CComponent_Hand

        switch (WhichProject)
        {
            case CProject.Link:
                controllerFileName += "Link_";
                break;
            default:
                Log.e(LOG_TAG, "unknown project name");
                break;
        }

        switch (WhichModel)
        {
            case ControllerModel.Ximmerse:
                controllerFileName += "Ximmerse_";
                break;
            default:
                Log.e(LOG_TAG, "unknown control model name");
                break;
        }

        if (TrackingMethod == CTrackingSpace.CTS_6DOF)
        {
            controllerFileName += "6DOF_";
        } else
        {
            controllerFileName += "3DOF_";
        }

        if (CTRComponents == CComponent.COM_MultiComponent)
        {
            controllerFileName += "MC_";
        }
        else
        {
            controllerFileName += "OB_";
        }

        if (WhichHand == ControllerHand.Controller_Right)
        {
            controllerFileName += "R";
        }
        else
        {
            controllerFileName += "L";
        }

        Log.i(LOG_TAG, "controller file name is " + controllerFileName);

        originalControllerPrefab = Resources.Load(controllerModelFoler + controllerFileName) as GameObject;
        var found = true;

        if (originalControllerPrefab == null)
        {
            if (TrackingMethod == CTrackingSpace.CTS_6DOF)
            {
                genericControllerFileName += "6DOF_MC_";
            }
            else
            {
                genericControllerFileName += "3DOF_MC_";
            }

            if (WhichHand == ControllerHand.Controller_Right)
            {
                genericControllerFileName += "R";
            }
            else
            {
                genericControllerFileName += "L";
            }
            Log.w(LOG_TAG, "cant find preferred controller model, load generic controller : " + genericControllerFileName);
            Log.i(LOG_TAG, "Please download controller model from .... to have better experience!");
            originalControllerPrefab = Resources.Load(controllerModelFoler + genericControllerFileName) as GameObject;
            if (originalControllerPrefab == null)
            {
                Log.e(LOG_TAG, "Cant load generic controller model, Please check file under Resources/" + controllerModelFoler + genericControllerFileName + ".prefab is existed!");
                found = false;
            } else
            {
                Log.i(LOG_TAG, genericControllerFileName + " controller model is found!");
            }
        } else
        {
            Log.i(LOG_TAG, controllerFileName + " controller model is found!");
        }

        if (found)
        {
            controllerPrefab = Instantiate(originalControllerPrefab, transform.position, transform.rotation);
            controllerPrefab.transform.parent = this.transform.parent;
        }
    }
	
	// Update is called once per frame
	void Update () {
    }
}
