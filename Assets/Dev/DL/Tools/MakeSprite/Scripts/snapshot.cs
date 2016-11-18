
using UnityEngine;

//using UnityEditor;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
	namespace SpriteMaker{

	public class snapshot : MonoBehaviour {
		
		public Camera cam;
		public int horizontalRes;
		public bool saveLocal = false;
		public string path = "Assets/";
		public string filename = "testScreenShot";

		public Material mat;
		public bool makeMaterial;

		void Update(){
			if (Input.GetKeyDown (KeyCode.R)) {
				takeSnapshot ();
			}
		}


			
		public void MakeMat(){
			Material newMat = Instantiate (mat);

		
			AssetDatabase.CreateAsset(newMat, path+filename+".mat");
			AssetDatabase.SaveAssets ();
	//		Debug.Log("Unity Editor");

		}

		public void takeSnapshot(){
			
			int width = horizontalRes;
			int height = horizontalRes;//(int)(horizontalRes*(9f/16f));
			RenderTexture renderTex = new RenderTexture(width,height,24);
			cam.targetTexture = renderTex;
			renderTex.antiAliasing = 8;
			Texture2D screenShot = new Texture2D(width,height, TextureFormat.ARGB32, false);
			screenShot.filterMode = FilterMode.Bilinear;
			cam.Render();
			RenderTexture.active = renderTex;
			screenShot.ReadPixels(new Rect(0, 0, width,height), 0, 0);
			cam.targetTexture = null;
			RenderTexture.active = null;
			if(saveLocal){
				byte[] bytes = screenShot.EncodeToPNG();
				string file = path+filename+".png";
				System.IO.File.WriteAllBytes(file, bytes);
				Debug.Log(string.Format("Took screenshot to: {0}", file));

			}

			if (makeMaterial)
				MakeMat ();
			
		}
	}
}
#endif
