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
using wvr;
using WaveVR_Log;
using System;

public class WaveVR_IMEManager {
	private static string LOG_TAG = "WVR_IMEManager";
	private const string IME_MANAGER_CLASSNAME = "com.htc.vr.unity.IMEManager";
	private AndroidJavaObject imeManager = null;

	private static WaveVR_IMEManager mInstance = null;
	private bool inited = false;
	public static WaveVR_IMEManager instance {
		get
		{
			if (mInstance == null)
			{
				mInstance = new WaveVR_IMEManager();
			}

			return mInstance;
		}
	}

	private AndroidJavaObject javaArrayFromCS(string[] values)
	{
		AndroidJavaClass arrayClass = new AndroidJavaClass("java.lang.reflect.Array");
		AndroidJavaObject arrayObject = arrayClass.CallStatic<AndroidJavaObject>("newInstance", new AndroidJavaClass("java.lang.String"), values.Length);
		for (int i = 0; i < values.Length; ++i)
		{
			arrayClass.CallStatic("set", arrayObject, i, new AndroidJavaObject("java.lang.String", values[i]));
		}

		return arrayObject;
	}

	private void initializeJavaObject()
	{
		Log.d(LOG_TAG, "initializeJavaObject");
		AndroidJavaClass ajc = new AndroidJavaClass(IME_MANAGER_CLASSNAME);

		if (ajc == null)
		{
			Log.e(LOG_TAG, "AndroidJavaClass is null");
			return;
		}
		// Get the IMEManager object
		imeManager = ajc.CallStatic<AndroidJavaObject>("getInstance");
		if (imeManager != null)
		{
			Log.d(LOG_TAG, "imeManager get object success");
		} else
		{
			Log.e(LOG_TAG, "imeManager get object failed");
		}
	}

	public bool isInitialized()
	{
		if (imeManager == null)
		{

			initializeJavaObject();
		}

		if (imeManager == null)
		{
			Log.e(LOG_TAG, "isInitialized failed because fail to get imeManager object");
			return false;
		}

		inited = imeManager.Call<bool>("isInitialized");
		return inited;
	}

	public void showKeyboard(int inputType, int mode, inputCompleteCallback cb)
	{
		Log.d(LOG_TAG, "showKeyboard");

		if (imeManager == null)
		{
			initializeJavaObject();
		}

		if (imeManager == null)
		{
			Log.e(LOG_TAG, "isInitialized failed because fail to get imeManager object");
			return;
		}

		mCallback = cb;

		imeManager.Call("showKeyboard",inputType,mode, new RequestCompleteHandler());
	}
	public void hideKeyboard()
	{
		Log.d(LOG_TAG, "hideKeyboard");

		if (imeManager == null)
		{
			Log.e(LOG_TAG, "hideKeyboard() failed because fail to get imeManager object");
			return ;
		}

		imeManager.Call("hideKeyboard");
	}

	public class InputResult
	{
		private string mContent;
		private int mErrorCode;

		public InputResult(string content, int errorCode)
		{
			mContent = content;
			mErrorCode = errorCode;
		}
		public string InputContent
		{
			get { return mContent; }
		}

		public int ErrorCode
		{
			get { return mErrorCode; }
		}
	}


	public delegate void inputCompleteCallback(InputResult results);

	private static inputCompleteCallback mCallback = null;

	class RequestCompleteHandler : AndroidJavaProxy
	{
		internal RequestCompleteHandler() : base(new AndroidJavaClass("com.htc.vr.unity.IMECallback")) {
		}

		public void onInputCompletedwithObject(AndroidJavaObject resultObject)
		{
			Log.i(LOG_TAG, "unity callback with result object");
			if (mCallback == null)
			{
				Log.w(LOG_TAG, "unity callback but user callback is null ");
			}

			int errorCode = resultObject.Get<int>("errorCode");
			string inputContent =  resultObject.Get<string>("inputContent");

			InputResult inputResult = new InputResult(inputContent,errorCode);
			mCallback(inputResult);
		}
	}
}
