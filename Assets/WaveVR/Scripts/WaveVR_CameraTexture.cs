// "WaveVR SDK 
// © 2017 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the WaveVR SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveVR_Log;
using System;
using wvr;

[RequireComponent(typeof(MeshRenderer))]
public class WaveVR_CameraTexture : MonoBehaviour
{
    private static string LOG_TAG = "WVR_CameraTexture";
    private MeshRenderer meshrenderer;

    private IntPtr natTextureId;
    private uint Textureid;
    private int mWidth = 0;
    private int mHeight = 0;
    bool texUpdated = false;
    bool texStarted = false;
    public WVR_CameraSource CameraSource = WVR_CameraSource.WVR_CameraSource_Android;
    Texture2D _texture = null;

    private void OnStartCameraCompleted(params object[] args)
    {
        Log.d(LOG_TAG, "OnStartCameraCompleted start tid: " + System.Threading.Thread.CurrentThread.ManagedThreadId);

        Textureid = (uint)args[0];
        natTextureId = (System.IntPtr)Textureid;
        uint tmpW = (uint)args[1];
        mWidth = (int)tmpW;
        uint tmpH = (uint)args[2];
        mHeight = (int)tmpH;
        texStarted = true;

        Log.d(LOG_TAG, "OnStartCameraCompleted, native texture = " + natTextureId.ToInt32() + " width = " + mWidth + " height = " + mHeight);
    }

    private void OnUpdateCameraCompleted(params object[] args)
    {
        texUpdated = (bool)args[0];
    }

    private void play()
    {
        WaveVR_Utils.Event.Listen("StartCameraCompleted", OnStartCameraCompleted);
        WaveVR_Utils.Event.Listen("UpdateCameraCompleted", OnUpdateCameraCompleted);
        Interop.WVR_SetCameraSource(CameraSource);
        WaveVR_Utils.SendRenderEvent(WaveVR_Utils.RENDEREVENTID_StartCamera);
    }

    private void stop()
    {
        WaveVR_Utils.Event.Remove("StartCameraCompleted", OnStartCameraCompleted);
        WaveVR_Utils.Event.Remove("UpdateCameraCompleted", OnUpdateCameraCompleted);
        WaveVR_Utils.SendRenderEvent(WaveVR_Utils.RENDEREVENTID_StopCamera);
    }

    private void CameraUpdate()
    {
        if (texStarted)
        {
            if (texUpdated)
            {
                texUpdated = false;
                if (_texture == null)
                {
                    _texture = Texture2D.CreateExternalTexture(mWidth, mHeight, TextureFormat.ARGB32, false, false, natTextureId);
                    meshrenderer.material.mainTexture = _texture;
                }
                else
                {
                    _texture.UpdateExternalTexture(natTextureId);
                }
            }
            WaveVR_Utils.SendRenderEvent(WaveVR_Utils.RENDEREVENTID_UpdateCamera);
        }
    }

    // Use this for initialization
    void Start()
    {
#if UNITY_EDITOR
        if (Application.isEditor)
        {
            return;
        }
#endif
        meshrenderer = GetComponent<MeshRenderer>();
        play();
    }

    void OnDisable()
    {
        stop();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Application.isEditor)
        {
            return;
        }
#endif
        CameraUpdate();
    }
}
