using System;
using UnityEngine;
using UnityEngine.UI;
using Holojam.Tools;

class HUDManager : MonoBehaviour
{

	public string hudState="hide";

//	public Canvas canvas;
//	public Sprite youMustEat = new Sprite ();
//	public Sprite timeToDie = new Sprite ();

	public void UpdateHUD (string HUDState)
	{
//		if (canvas == null) {
//			return;
//		}
		switch (HUDState) {
		//TODO: an enumeration would be better here
		case "hide":
			{
				VRDebug.disableDisplay ();
				VRDebug.clearConsole ();
				hudState = "hide";
				//				canvas.enabled = false;
				break;
			}
		case "warn":
			{
				VRDebug.clearConsole ();
				VRDebug.print ("you must eat in order to live");
				VRDebug.enableDisplay ();

				hudState = "warn";
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
				hudState = "dying";

//				Image image = canvas.GetComponentsInChildren<Image> () [0];
//				image.sprite = timeToDie;
//				canvas.enabled = true;
				break;
			}
		case "ascensionNest":
			{
				VRDebug.clearConsole ();
				VRDebug.enableDisplay ();
				VRDebug.print ("Please go to the Ascension Nest");
				hudState = "ascensionNest";
				break;
			}
		case "sessionComplete":
			{
				VRDebug.clearConsole ();
				VRDebug.enableDisplay ();
				VRDebug.print ("please remove your headset");
				hudState = "sessionComplete";

				break;
			}
		}
	}
}

