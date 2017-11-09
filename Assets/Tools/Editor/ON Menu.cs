using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ONMenu : MonoBehaviour {

	[MenuItem("ON/Take Screenshot %#&s")]
	static void TakeScreenshot()
	{
		string filename = "Screen Shot " + System.DateTime.Now.ToString("yyyy-MM-dd a\\t HH.mm.ss") + ".png";
		ScreenCapture.CaptureScreenshot(filename);
	}


}
