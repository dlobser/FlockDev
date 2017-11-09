using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WaveVR_Log;
using wvr;

public class IMEDemo : MonoBehaviour
{
    public WVR_DeviceType Controller1Index = WVR_DeviceType.WVR_DeviceType_Controller_Right;
    public WVR_DeviceType Controller2Index = WVR_DeviceType.WVR_DeviceType_Controller_Left;
    public TextMesh Target;
    private static string LOG_TAG = "IMETest";
    private bool showKeyboard_ = false;
    private bool initialized = false;
    private WaveVR_IMEManager pmInstance = null;
    private string inputContent = null;

    public void inputDoneCallback(WaveVR_IMEManager.InputResult results)
    {
        Log.d(LOG_TAG, "inputDoneCallback:" + results.InputContent);
        inputContent = results.InputContent;
        showKeyboard_ = false;
        //updateInputField(targetInput,inputContent);
    }
    public void UpdateTargetText(string str)
    {
        //Log.d(LOG_TAG, "UpdateTargetText:" + str);
        if (Target != null && str != null)
        {
            Target.text = "Result: " + str;
        }
    }
    private void InitializeKeyboards()
    {
        pmInstance = WaveVR_IMEManager.instance;
        initialized = pmInstance.isInitialized();
        showKeyboard_ = false;
        if (initialized)
            Log.d(LOG_TAG, "InitializeKeyboards: done");
        else
            Log.d(LOG_TAG, "InitializeKeyboards: failed");

    }
    private void hideKeyboard()
    {
        if (showKeyboard_ && initialized)
        {
            Log.i(LOG_TAG, "hideKeyboard: done");
            pmInstance.hideKeyboard();
            showKeyboard_ = false;
        }
    }
    private void showKeyboard(int inputType, int mode)
    {
        if (!showKeyboard_ && initialized)
        {
            Log.i(LOG_TAG, "showKeyboard: done");
            pmInstance.showKeyboard(inputType, mode, inputDoneCallback);
            showKeyboard_ = true;
        }
    }
    public void showKeyboardEng()
    {
        if (!initialized)
            InitializeKeyboards();
        showKeyboard(1, 0);

    }
    public void showKeyboardPassword()
    {
        showKeyboard(2, 0);
    }
    public void showKeyboardNumber()
    {
        showKeyboard(0, 0);
    }
    // Use this for initialization
    void Start()
    {
        UpdateTargetText("Hello IME");
        InitializeKeyboards();
    }

    // Update is called once per frame
    void Update()
    {
        bool rightTriggerDown = false, rightTriggerUp = false, leftTriggerDown = false, leftTriggerUp = false;

        //rightTriggerUp |= WaveVR_Controller.Input (Controller1Index).GetPressUp (WVR_InputId.WVR_InputId_Alias1_Touchpad);
#if UNITY_EDITOR
        /// We don't need to set #if UNITY_EDITOR condition.
        /// In editor mode, XXXTriggerDown value will be overwritten by WaveVR_Controller
        /// In WaveVR_Controller.Update, it checks whether editor mode or not.
        /// In editor mode -> call to emulator provider, otherwise call to SDK.
        rightTriggerDown = Input.GetMouseButtonDown(1);  // mouse right key
        rightTriggerUp = Input.GetMouseButtonUp(1);
        leftTriggerDown = Input.GetMouseButtonDown(0);  // mouse left key
        leftTriggerUp = Input.GetMouseButtonUp(0);

        if (rightTriggerDown)
        {
            UpdateTargetText("showKeyboardEng");
        }
        if (leftTriggerDown)
        {
            UpdateTargetText("hideKeyboard");
        }
#endif

        rightTriggerDown |= WaveVR_Controller.Input(Controller1Index).GetPressDown(WVR_InputId.WVR_InputId_Alias1_Touchpad);
        leftTriggerDown |= WaveVR_Controller.Input(Controller1Index).GetPressDown(WVR_InputId.WVR_InputId_Alias1_Menu);
        if (rightTriggerDown)
        {
            showKeyboardEng();
        }
        if (leftTriggerDown)
        {
            hideKeyboard();
        }
    }
    void LateUpdate()
    {
        UpdateTargetText(inputContent);
        //inputContent = "";
    }
}