using UnityEngine;
using System.Collections;

public class MaterialSwapperPingPong : MonoBehaviour {

	//** made for shader: RGBRemapSimpleTransparentFade

	public Texture[] textures;
	public int whichTexture;
	public MeshRenderer rend;
	MaterialPropertyBlock block;
	float fader = 1;
	public float speed = 1;

	public float count = 2;
	float counter = 0;

	Coroutine coSwap;

	void Start(){
		block = new MaterialPropertyBlock ();
	}

	void Update(){
		counter += Time.deltaTime * speed;
		if (counter>count) {
			counter = 0;
			swapMat ();
		}
	}

	public void swapMat(){
		
		whichTexture++;

		if (whichTexture >= textures.Length)
			whichTexture = 0;

		if (fader == 1) {
			block.SetTexture ("_MainTex" , textures [whichTexture]);
		}
		else if(fader==0){
			block.SetTexture ("_SecondTex" , textures[whichTexture]);
		}

		StartCoroutine (Swap ());
		StopCoroutine (Swap ());

	}

	IEnumerator Swap(){

		Debug.Log ("Swap");
		
		if (fader<=0) {
			block.SetTexture ("_SecondTex" , textures[whichTexture]);
			while (fader < 1) {
				fader += speed * Time.deltaTime;
				SwitchIt (fader);
				yield return new WaitForSeconds (Time.deltaTime);
			}
		}
		else {
			block.SetTexture ("_MainTex" , textures [whichTexture]);
			while (fader > 0) {
				fader -= speed * Time.deltaTime;
				SwitchIt (fader);
				yield return new WaitForSeconds (Time.deltaTime);
			}
		}

		yield return null;
	}

	void SwitchIt(float f){

		f = Mathf.Clamp (f, 0, 1);

		block.SetFloat ("_TextureFade", f);
		rend.SetPropertyBlock (block);

	}
}
