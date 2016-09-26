using System;
using UnityEngine;
using UnityEngine.UI;
using Holojam.Tools;

class HUDManager : MonoBehaviour
{
//	public Canvas canvas;
//	public Sprite youMustEat = new Sprite ();
//	public Sprite timeToDie = new Sprite ();

	public void UpdateHUD (string HUDState)
	{
//		if (canvas == null) {
//			return;
//		}
		switch (HUDState) {
		case "hide":
			{
				VRDebug.disableDisplay ();
				VRDebug.clearConsole ();
				
//				canvas.enabled = false;
				break;
			}
		case "warn":
			{
				VRDebug.clearConsole ();
				VRDebug.print ("you must eat in order to live");
				VRDebug.enableDisplay ();

//				Image image = canvas.GetComponentsInChildren<Image> () [0];
//				if (youMustEat != null) {
//					image.sprite = youMustEat;
//				}
//				canvas.enabled = true;
				break;		
			}
		case "dying":
			{

				VRDebug.clearConsole ();
				VRDebug.enableDisplay ();
				VRDebug.print ("you are dying now");
//				Image image = canvas.GetComponentsInChildren<Image> () [0];
//				image.sprite = timeToDie;
//				canvas.enabled = true;
				break;
			}
		case "sessionComplete":
			{
				VRDebug.clearConsole ();
				VRDebug.enableDisplay ();
				VRDebug.print ("please remove your headset");
				break;
			}
		}
	}
}

