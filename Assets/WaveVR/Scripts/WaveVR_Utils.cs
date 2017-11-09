// "WaveVR SDK 
// © 2017 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the WaveVR SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

#define SYSTRACE_NATIVE  // Systrace in native support multi-thread rendering.
using UnityEngine;
using System.Collections;
using wvr;
using System.Runtime.InteropServices;
using WaveVR_Log;
using AOT;

/// This class is mainly for common handling:
/// including event handling and pose data handling.
public static class WaveVR_Utils
{
    public static string LOG_TAG = "WVR_Utils";

    #region Strings
    [System.Obsolete("Use same variable in Event instead.")]
    public static string DEVICE_CONNECTED = "device_connected";
    [System.Obsolete("Use same variable in Event instead.")]
    public static string NEW_POSES = "new_poses";
    [System.Obsolete("Use same variable in Event instead.")]
    public static string AFTER_NEW_POSES = "after_new_poses";
    #endregion

    public enum DegreeField
    {
        DOF3,
        DOF6
    }

    public struct WVR_ButtonState_t
    {
        public ulong BtnPressed;
        public ulong BtnTouched;
    }

    public class Event
    {
        public static string DEVICE_CONNECTED = "device_connected";
        public static string NEW_POSES = "new_poses";
        public static string AFTER_NEW_POSES = "after_new_poses";
        public static string ALL_VREVENT = "all_vrevent";  // Called when had event from WVR_PollEventQueue()
        public static string BATTERY_STATUS_UPDATE = "BatteryStatus_Update";

        public delegate void Handler(params object[] args);

        public static void Listen(string message, Handler action)
        {
            var actions = listeners[message] as Handler;
            if (actions != null)
            {
                listeners[message] = actions + action;
            }
            else
            {
                listeners[message] = action;
            }
        }

        public static void Remove(string message, Handler action)
        {
            var actions = listeners[message] as Handler;
            if (actions != null)
            {
                listeners[message] = actions - action;
            }
        }

        public static void Send(string message, params object[] args)
        {
            var actions = listeners[message] as Handler;
            if (actions != null)
            {
                actions(args);
            }
        }

        private static Hashtable listeners = new Hashtable();
    }

    private static float _copysign(float sizeval, float signval)
    {
        return Mathf.Sign(signval) == 1 ? Mathf.Abs(sizeval) : -Mathf.Abs(sizeval);
    }

    public static Quaternion GetRotation(this Matrix4x4 matrix)
    {
        Quaternion q = new Quaternion();
        q.w = Mathf.Sqrt(Mathf.Max(0, 1 + matrix.m00 + matrix.m11 + matrix.m22)) / 2;
        q.x = Mathf.Sqrt(Mathf.Max(0, 1 + matrix.m00 - matrix.m11 - matrix.m22)) / 2;
        q.y = Mathf.Sqrt(Mathf.Max(0, 1 - matrix.m00 + matrix.m11 - matrix.m22)) / 2;
        q.z = Mathf.Sqrt(Mathf.Max(0, 1 - matrix.m00 - matrix.m11 + matrix.m22)) / 2;
        q.x = _copysign(q.x, matrix.m21 - matrix.m12);
        q.y = _copysign(q.y, matrix.m02 - matrix.m20);
        q.z = _copysign(q.z, matrix.m10 - matrix.m01);
        return q;
    }

    public static Quaternion GetRotation2(this Matrix4x4 matrix)
    {
        float tr = matrix.m00 + matrix.m11 + matrix.m22;
        float qw, qx, qy, qz;
        if (tr > 0) {
            float S = Mathf.Sqrt(tr + 1.0f) * 2; // S=4*qw
            qw = 0.25f * S;
            qx = (matrix.m21 - matrix.m12) / S;
            qy = (matrix.m02 - matrix.m20) / S;
            qz = (matrix.m10 - matrix.m01) / S;
        } else if ((matrix.m00 > matrix.m11) & (matrix.m00 > matrix.m22)) {
            float S = Mathf.Sqrt(1.0f + matrix.m00 - matrix.m11 - matrix.m22) * 2; // S=4*qx
            qw = (matrix.m21 - matrix.m12) / S;
            qx = 0.25f * S;
            qy = (matrix.m01 + matrix.m10) / S;
            qz = (matrix.m02 + matrix.m20) / S;
        } else if (matrix.m11 > matrix.m22) {
            float S = Mathf.Sqrt(1.0f + matrix.m11 - matrix.m00 - matrix.m22) * 2; // S=4*qy
            qw = (matrix.m02 - matrix.m20) / S;
            qx = (matrix.m01 + matrix.m10) / S;
            qy = 0.25f * S;
            qz = (matrix.m12 + matrix.m21) / S;
        } else {
            float S = Mathf.Sqrt(1.0f + matrix.m22 - matrix.m00 - matrix.m11) * 2; // S=4*qz
            qw = (matrix.m10 - matrix.m01) / S;
            qx = (matrix.m02 + matrix.m20) / S;
            qy = (matrix.m12 + matrix.m21) / S;
            qz = 0.25f * S;
        }
        return new Quaternion(qx, qy, qz, qw);
    }

    public static Vector3 GetPosition(this Matrix4x4 matrix)
    {
        var x = matrix.m03;
        var y = matrix.m13;
        var z = matrix.m23;

        return new Vector3(x, y, z);
    }

    // get new position and rotation from new pose
    [System.Serializable]
    public struct RigidTransform
    {
        public Vector3 pos;
        public Quaternion rot;

        public static RigidTransform identity
        {
            get { return new RigidTransform(Vector3.zero, Quaternion.identity); }
        }

        public RigidTransform(Vector3 pos, Quaternion rot)
        {
            this.pos = pos;
            this.rot = rot;
        }

        public RigidTransform(Transform t)
        {
            this.pos = t.position;
            this.rot = t.rotation;
        }

        public RigidTransform(WVR_Matrix4f_t pose)
        {
            var m = toMatrix44(pose);
            this.pos = m.GetPosition();
            this.rot = m.GetRotation2();
        }

        public static Matrix4x4 toMatrix44(WVR_Matrix4f_t pose)
        {
            var m = Matrix4x4.identity;

            m[0, 0] = pose.m0;
            m[0, 1] = pose.m1;
            m[0, 2] = -pose.m2;
            m[0, 3] = pose.m3;

            m[1, 0] = pose.m4;
            m[1, 1] = pose.m5;
            m[1, 2] = -pose.m6;
            m[1, 3] = pose.m7;

            m[2, 0] = -pose.m8;
            m[2, 1] = -pose.m9;
            m[2, 2] = pose.m10;
            m[2, 3] = -pose.m11;

            return m;
        }

        public void update(WVR_Matrix4f_t pose)
        {
            var m = toMatrix44(pose);
            this.pos = m.GetPosition();
            this.rot = m.GetRotation2();
        }

        public override bool Equals(object o)
        {
            if (o is RigidTransform)
            {
                RigidTransform t = (RigidTransform)o;
                return pos == t.pos && rot == t.rot;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return pos.GetHashCode() ^ rot.GetHashCode();
        }

        public static bool operator ==(RigidTransform a, RigidTransform b)
        {
            return a.pos == b.pos && a.rot == b.rot;
        }

        public static bool operator !=(RigidTransform a, RigidTransform b)
        {
            return a.pos != b.pos || a.rot != b.rot;
        }

        public static RigidTransform operator *(RigidTransform a, RigidTransform b)
        {
            return new RigidTransform
            {
                rot = a.rot * b.rot,
                pos = a.pos + a.rot * b.pos
            };
        }

        public void Inverse()
        {
            rot = Quaternion.Inverse(rot);
            pos = -(rot * pos);
        }

        public RigidTransform GetInverse()
        {
            var t = new RigidTransform(pos, rot);
            t.Inverse();
            return t;
        }

        public Vector3 TransformPoint(Vector3 point)
        {
            return pos + (rot * point);
        }

        public static Vector3 operator *(RigidTransform t, Vector3 v)
        {
            return t.TransformPoint(v);
        }

    }

#if SYSTRACE_NATIVE
    public static Queue TraceSessionNameQueue = new Queue(5);
#else
    public static AndroidJavaObject trace = new AndroidJavaObject("android.os.Trace");
#endif

    public class Trace {
        public static void BeginSection(string sectionName, bool inRenderThread = true)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
#if SYSTRACE_NATIVE
            if (inRenderThread) {
                lock (TraceSessionNameQueue)
                {
                    TraceSessionNameQueue.Enqueue(sectionName);
                }
                SendRenderEvent(RENDEREVENTID_Systrace_BeginSession);
            } else {
                TraceBeginSection(sectionName);
            }
#else
            trace.CallStatic("beginSection", sectionName);
#endif
#endif
        }

        public static void EndSection(bool inRenderThread = true)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
#if SYSTRACE_NATIVE
            if (inRenderThread) {
                SendRenderEvent(RENDEREVENTID_Systrace_EndSession);
            } else {
                TraceEndSection();
            }
#else
            trace.CallStatic("endSection");
#endif
#endif
        }
    }

    public static void notifyActivityUnityStarted()
    {
        AndroidJavaClass clazz = new AndroidJavaClass("com.htc.vr.unity.WVRUnityVRActivity");
        AndroidJavaObject activity = clazz.CallStatic<AndroidJavaObject>("getInstance");
        activity.Call("onUnityStarted");
    }

    public const int k_nRenderEventID_SubmitL = 1;
    public const int k_nRenderEventID_SubmitR = 2;
    public const int k_nRenderEventID_GraphicInitial = 8;
    public const int k_nRenderEventID_RenderEyeL = 0x100;
    public const int k_nRenderEventID_RenderEyeR = 0x101;
    public const int k_nRenderEventID_RenderEyeEndL = 0x102;
    public const int k_nRenderEventID_RenderEyeEndR = 0x103;

    [DllImportAttribute("wvrunity", CallingConvention = CallingConvention.Cdecl)]
    public static extern System.IntPtr GetRenderEventFunc();

    [DllImportAttribute("wvrunity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void NativeRenderEvent(int eventID);

    [DllImportAttribute("wvrunity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetTexture(System.IntPtr left, System.IntPtr right);

#if SYSTRACE_NATIVE
    [DllImportAttribute("wvrunity", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void TraceBeginSection(string name);

    [DllImportAttribute("wvrunity", CallingConvention = CallingConvention.Cdecl)]
    private static extern void TraceEndSection();
#endif

    [DllImportAttribute("wvr_api", EntryPoint = "WVR_IsATWActive", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool WVR_IsATWActive();

    [DllImportAttribute("wvr_api", EntryPoint = "WVR_SetATWActive", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WVR_SetATWActive(bool active);

    [DllImportAttribute("wvr_api", EntryPoint = "WVR_PauseATW", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WVR_PauseATW();

    [DllImportAttribute("wvr_api", EntryPoint = "WVR_ResumeATW", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WVR_ResumeATW();

    [DllImportAttribute("wvr_api", EntryPoint = "WVR_OnDisable", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WVR_OnDisable();

    [DllImportAttribute("wvr_api", EntryPoint = "WVR_OnApplicationQuit", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WVR_OnApplicationQuit();

    [DllImportAttribute("wvr_api", EntryPoint = "WVR_GetNumberOfTextures", CallingConvention = CallingConvention.Cdecl)]
    public static extern int WVR_GetNumberOfTextures();

    [DllImportAttribute("wvr_api", EntryPoint = "WVR_StoreRenderTextures", CallingConvention = CallingConvention.Cdecl)]
    public static extern System.IntPtr WVR_StoreRenderTextures(System.IntPtr[] texturesIDs, int size, bool eEye);

    [DllImportAttribute("wvr_api", EntryPoint = "WVR_GetAvailableTextureID", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint WVR_GetAvailableTextureID(System.IntPtr queue);

    [DllImportAttribute("wvr_api", EntryPoint = "WVR_IsPresentedOnExternalD", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool WVR_IsPresentedOnExternal();

    public const int RENDEREVENTID_INIT_GRAPHIC = 0;
    public const int RENDEREVENTID_Systrace_BeginSession = 4;
    public const int RENDEREVENTID_Systrace_EndSession = 5;
    public const int RENDEREVENTID_ATWEnable = 8;
    public const int RENDEREVENTID_ATWDisable = 9;
    public const int RENDEREVENTID_StartCamera = 21;
    public const int RENDEREVENTID_StopCamera = 22;
    public const int RENDEREVENTID_UpdateCamera = 23;

    [MonoPInvokeCallback(typeof(RenderEventDelegate))]
    private static void RenderEvent(int eventId)
    {
        switch (eventId)
        {
            case RENDEREVENTID_INIT_GRAPHIC:
                // Use native code to initial compositor then get the c# instance later.
                NativeRenderEvent(k_nRenderEventID_GraphicInitial);
                break;
            case RENDEREVENTID_Systrace_BeginSession:
                string sectionName;
                lock (TraceSessionNameQueue)
                {
                    try
                    {
                        sectionName = (string)TraceSessionNameQueue.Dequeue();
                    }
                    catch (System.InvalidOperationException)
                    {
                        sectionName = "Empty";
                    }
                }
                TraceBeginSection(sectionName);
                break;
            case RENDEREVENTID_Systrace_EndSession:
                TraceEndSection();
                break;
            case RENDEREVENTID_ATWEnable:
                WVR_ResumeATW();
                break;
            case RENDEREVENTID_ATWDisable:
                WVR_PauseATW();
                break;
            case RENDEREVENTID_StartCamera:
                {
                    uint rWidth = 0;
                    uint rHeight = 0;
                    var textureId = Interop.WVR_StartCamera(ref rWidth, ref rHeight);

                    WaveVR_Utils.Event.Send("StartCameraCompleted", textureId, rWidth, rHeight);
                }

                break;
            case RENDEREVENTID_StopCamera:
                {
                    Interop.WVR_StopCamera();
                }

                break;
            case RENDEREVENTID_UpdateCamera:
                {
                    var updatedtexture = Interop.WVR_UpdateFrameTexture();
                    WaveVR_Utils.Event.Send("UpdateCameraCompleted", updatedtexture);
                }

                break;
        }
    }

    private delegate void RenderEventDelegate(int eye);
    private static RenderEventDelegate RenderEventHandle = new RenderEventDelegate(RenderEvent);
    private static System.IntPtr RenderEventHandlePtr = Marshal.GetFunctionPointerForDelegate(RenderEventHandle);

    public static void SendRenderEvent(int eventId)
    {
        GL.IssuePluginEvent(RenderEventHandlePtr, eventId);
    }

    public static void SendRenderEventNative(int eventId)
    {
        GL.IssuePluginEvent(GetRenderEventFunc(), eventId);
    }
}
