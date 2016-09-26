using UnityEngine;
using System.Collections;


public class ChosenHeadset : MonoBehaviour {

	public Holojam.Network.Motive.Tag whichHeadset;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	public void setHeadset(int which){
		if (which == 1)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET1;
		else if (which == 2)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET2;
		else if (which == 3)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET3;
		else if (which == 4)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET4;
		else if (which == 5)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET5;
		else if (which == 6)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET6;
		else if (which == 7)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET7;
		else if (which == 8)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET8;
		else if (which == 9)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET9;
		else if (which == 10)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET10;
		else if (which == 11)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET11;
		else if (which == 12)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET12;
		else if (which == 13)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET13;
		else if (which == 14)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET14;
		else if (which == 15)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET15;
		else if (which == 16)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET16;
		else if (which == 17)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET17;
		else if (which == 18)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET18;
		else if (which == 19)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET19;
		else if (which == 20)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET20;
		else if (which == 21)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET21;
		else if (which == 22)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET22;
		else if (which == 23)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET23;
		else if (which == 24)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET24;
		else if (which == 25)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET25;
		else if (which == 26)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET26;
		else if (which == 27)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET27;
		else if (which == 28)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET28;
		else if (which == 29)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET29;
		else if (which == 30)
			whichHeadset = Holojam.Network.Motive.Tag.HEADSET30;
		else
			Debug.Log ("no headset");
	}

}
