using System;
using UnityEngine;
using UnityEngine.UI;
using Holojam.Tools;

class HUDManager : MonoBehaviour
{
	public Canvas canvas;
	public Sprite youMustEat = new Sprite ();
	public Sprite timeToDie = new Sprite ();

	public void UpdateHUD (string HUDState)
	{
		if (canvas == null) {
			return;
		}
		switch (HUDState) {
		case "hide":
			{
				canvas.enabled = false;
				break;
			}
		case "warn":
			{
				VRDebug.print ("warning to vrconsole");
				Image image = canvas.GetComponentsInChildren<Image> () [0];
				if (youMustEat != null) {
					image.sprite = youMustEat;
				}
				canvas.enabled = true;
				break;		
			}
		case "die":
			{
				Image image = canvas.GetComponentsInChildren<Image> () [0];
				image.sprite = timeToDie;
				canvas.enabled = true;
				break;
			}
		}
	}
}

