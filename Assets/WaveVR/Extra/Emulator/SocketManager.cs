// "WaveVR SDK 
// © 2017 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the WaveVR SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using UnityEngine;
using System.Collections;
using proto;

/// @cond
namespace WaveVREmulator {
    class SocketManager : MonoBehaviour
    {
        private static SocketManager instance = null;
        public static SocketManager Instance
        {
            get {
                if (instance == null)
                {
                    Debug.Log ("SocketManager::Instance, create PhoneRemote GameObject");
                    var gameObject = new GameObject("PhoneRemote");
                    instance = gameObject.AddComponent<SocketManager>();
                    // This object should survive all scene transitions.
                    GameObject.DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }

        // ----------------------- Orientation Event begins ---------------------
        private EmulatorOrientationEvent currentOrientationEvent;
        private event OnOrientationEvent orientationEventListenersInternal;

        public delegate void OnOrientationEvent(EmulatorOrientationEvent orientationEvent);
        public event OnOrientationEvent orientationEventListeners {
            add {
                if (value != null) {
                    value(currentOrientationEvent);
                }
                orientationEventListenersInternal += value;
            }

            remove {
                orientationEventListenersInternal -= value;
            }
        }

        private void ProcessOrientationEvent(EmulatorOrientationEvent e) {
            currentOrientationEvent = e;
            if (orientationEventListenersInternal != null) {
                orientationEventListenersInternal(e);       // call to ControllerProvider.HandleOrientationEvent
            }
        }
        // ----------------------- Orientation Event ends ---------------------

        // ----------------------- Button Event begins ---------------------------
        EmulatorButtonEvent currentButtonEvent;
        private event OnButtonEvent buttonEventListenersInternal;

        public delegate void OnButtonEvent(EmulatorButtonEvent buttonEvent);
        public event OnButtonEvent buttonEventListeners {
            add {
                if (value != null) {
                    value(currentButtonEvent);
                }
                buttonEventListenersInternal += value;
            }

            remove {
                buttonEventListenersInternal -= value;
            }
        }

        private void ProcessButtonEvent(EmulatorButtonEvent e) {
            currentButtonEvent = e;
            if (buttonEventListenersInternal != null) {
                buttonEventListenersInternal(e);            // call to ControllerProvider.HandleButtonEvent
            }
        }
        // ----------------------- Button Event ends -----------------------------

        // -----------------------  Event Queue Handling begins -----------------------
        private Queue pendingEvents = Queue.Synchronized(new Queue());

        public void OnPhoneEvent(PhoneEvent e)
        {
            pendingEvents.Enqueue(e);
        }

        private void ProcessEventAtEndOfFrame(PhoneEvent e) {
            switch (e.Type) {
            case PhoneEvent.Types.Type.ORIENTATION:
                EmulatorOrientationEvent orientationEvent =
                    new EmulatorOrientationEvent(e.OrientationEvent);
                ProcessOrientationEvent(orientationEvent);
                break;
            case PhoneEvent.Types.Type.KEY:
                EmulatorButtonEvent buttonEvent = new EmulatorButtonEvent(e.KeyEvent);
                ProcessButtonEvent(buttonEvent);
                break;
            default:
                //Debug.Log("Unsupported PhoneEvent type: " + e.Type);
                break;
            }
        }

        IEnumerator EndOfFrame() {
            while (true) {
                yield return waitForEndOfFrame;
                lock (pendingEvents.SyncRoot) {
                    while (pendingEvents.Count > 0) {
                        PhoneEvent phoneEvent = (PhoneEvent) pendingEvents.Dequeue();
                        ProcessEventAtEndOfFrame(phoneEvent);
                    }
                }
            }
        }
        // -----------------------  Event Handling ends -----------------------

        private SocketClient socket;

        public bool Connected {
            get {
                return socket != null && socket.connected;
            }
        }

        private IEnumerator emulatorUpdate;
        private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        // Use this for initialization
    	void Start ()
        {
            socket = gameObject.AddComponent<SocketClient>();
            socket.Init(this);
            emulatorUpdate = EndOfFrame();
            StartCoroutine(emulatorUpdate);
    	}
    }
}
/// @endcond