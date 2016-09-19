
using UnityEngine;

public class BugManager : MonoBehaviour {
//	public Bug bug;
	public Bug[] bugPrefabs;
	public int gridWidth = 2;
	public float scale = 0.5f;

	public float disableTime = 1;

	public bool init = false; //redundant?

	int amount;

	public Bug[] bugs;
	Vector3[] targets;

	float noiseCounter = 0;
	public float noiseSpeed = 1f;
	public float noiseScale = 2.2f;
	public float noiseWorldScale = .1f;

	public float bugHeight = 0;

	ImageTools.Core.PerlinNoise PNoise;
	public ParticleSystem part;


	void Awake(){
		if(!Application.isPlaying)return;

		amount = gridWidth * gridWidth;
		bugs = new Bug[amount];
		targets = new Vector3[amount];

		PNoise = new ImageTools.Core.PerlinNoise (1);


		for(int x = 0 ; x < gridWidth ; ++x ){
			for (int y = 0 ; y < gridWidth ; ++y) {
				Vector3 posA = new Vector3 ((x-gridWidth/2) * scale, bugHeight, (y-gridWidth/2) * scale);
				Vector3 pos = transform.position + posA;
				targets [x + gridWidth * y] = pos;
				GameObject b = bugPrefabs [(int)Mathf.Floor (Random.value * bugPrefabs.Length)].gameObject;
				GameObject myBug = Instantiate (b.gameObject, pos, Quaternion.identity) as GameObject;
				myBug.transform.parent = transform;
				string label = "Bug" + x + "." + y;
				myBug.name = label;
				bugs [x + gridWidth * y] = myBug.GetComponent<Bug> ();
				bugs [x + gridWidth * y].label = label;
				bugs [x + gridWidth * y].origin = pos;
			}
		}

		init = true;
	}

	void Update(){
		if(init)
			UpdateBugPosition ();


	}

	void UpdateBugPosition(){
		for (int i = 0; i < amount; i++) {
			float off = .5f;
			float scale = noiseScale*.01f;
			float wScale = noiseWorldScale;
			noiseCounter += noiseSpeed * Time.deltaTime;
			Vector3 newPos = bugs [i].origin;
			newPos = newPos + new Vector3 (
				scale * (float)PNoise.Noise (wScale * newPos.x + noiseCounter + off, noiseCounter + newPos.y * wScale, noiseCounter + wScale * newPos.z),
				scale * (float)PNoise.Noise (wScale * newPos.x + noiseCounter, wScale * newPos.y + noiseCounter + off, noiseCounter + wScale * newPos.z),
				scale * (float)PNoise.Noise (wScale * newPos.x + noiseCounter, wScale * newPos.y + noiseCounter, noiseCounter + wScale * newPos.z + off));
			bugs [i].transform.position = newPos;


		} 

	}
	void UpdateBugAppearance(Bug b){


//		b.renderers[0].material=
		Debug.Log (b);
	}


	public void ProcessCollision(Bug b){
		if (!init)
			return;
		else {
			ParticleSystem p = Instantiate (part, b.transform.position, Quaternion.identity) as ParticleSystem;
			p.Emit (10);
//			Debug.Log (c.name + " collided with Bug " + b.name);

		}
		//do something about state here
		//eg increment 'bugs eaten' var
	}
}