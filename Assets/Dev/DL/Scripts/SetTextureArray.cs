using UnityEngine;
using System.Collections;

public class SetTextureArray : MonoBehaviour {

	public Texture[] textures;
	public Material mat;
	// Use this for initialization
	void Start () {
		
		Texture2DArray textureArray = new Texture2DArray( 2048, 2048, textures.Length, TextureFormat.ARGB32, true);
		for (int i = 0; i < textures.Length; i++) {
			Graphics.CopyTexture( textures[i], i, 0, textureArray, i, 0);

		}

		mat.SetTexture("Tex",  textureArray);
	}
	

}
