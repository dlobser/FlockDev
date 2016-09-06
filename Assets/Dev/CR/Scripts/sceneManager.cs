using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class sceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Scene scene = SceneManager.GetActiveScene ();
		Debug.Log("active scene is " + scene.name);
	
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	void OnMouseDown(){
		Debug.Log ("mouse clicked");

		//SceneManager.LoadScene ("sceneTwo");

		SceneManager.LoadScene ("0-standby", LoadSceneMode.Additive);

	}
}
