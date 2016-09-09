using UnityEngine;
using System.Collections;
using TREESharp;
#if UNITY_EDITOR

namespace SpriteMaker{
	public class MakeSpriteSheet : MonoBehaviour {

		public int X = 4;
		public int Y = 4;
		RenderTexture[] RTex;
		Material[] materials;
		public RenderTexture InTexture;
		public GameObject Quad;
		GameObject[] quads;
		float timeCap = 1;
		float counter = 0;
		int count = 0;
		public Material InMat;
		public snapshot snap;
		bool snapshotted = false;
		public TransformUniversalSetTime[] transformer;
		public MakeTree[] trees;
		public TREEModXForm[] treeMods;

		bool gridMade = false;
		// Use this for initialization
		void Start () {
			timeCap = 1f / (float)(X * Y);
			Application.targetFrameRate = 55;
			RTex = new RenderTexture[X*Y];
			materials = new Material[X*Y];
			quads = new GameObject[X*Y];
			MakeGrid ();

		}

		void MakeGrid(){
			int c = -1;
			for (int i = 0; i < X; i++) {
				for (int j = 0; j < Y; j++) {
					quads [++c] = Instantiate (Quad);
					quads [c].transform.localPosition = new Vector3 (
						(float)j / (float)Y - .5f + ((1f/(float)Y)/2f), 
						((1f-((float)i / (float)X)) -.5f - ((1f/(float)X)/2f))-1000, 0);
					quads [c].transform.localScale = new Vector3(1f/(float)Y,1f/(float)X,1);
					quads [c].name = c.ToString();

				}
			}
	//		gridMade = true;
		}

		
		// Update is called once per frame
		void Update () {
			
			if (count < X * Y && gridMade) {
				
					RTex [count] = new RenderTexture (InTexture.width, InTexture.height, 24);
					RTex [count].antiAliasing = InTexture.antiAliasing;
					Graphics.Blit (InTexture, RTex [count]);
					materials [count] = Instantiate (InMat);
					materials [count].SetTexture ("_MainTex", RTex [count]);
					quads [count].GetComponent<MeshRenderer> ().material = materials [count];

					count++;

				for (int i = 0; i < trees.Length; i++) {
					trees [i].countSpeed = timeCap;
					trees [i].Animate ();
				}
				for (int i = 0; i < treeMods.Length; i++) {
					treeMods [i].countSpeed = timeCap;
					treeMods [i].Step ();
				}

				counter += timeCap;
			} else if (!snapshotted && gridMade) {
				snap.takeSnapshot ();
				snapshotted = true;
			}
			if (Input.GetKeyUp (KeyCode.A)) {
				gridMade = true;
				for (int i = 0; i < transformer.Length; i++) {
					transformer[i].currentTime = timeCap;
				}
		
				for (int i = 0; i < trees.Length; i++) {
					trees [i].countSpeed = timeCap;
				}
				for (int i = 0; i < treeMods.Length; i++) {
					treeMods [i].countSpeed = timeCap;
				}
			}
		}
	}
}
#endif