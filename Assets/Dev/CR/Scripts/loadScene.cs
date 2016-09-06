using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {
	public string sceneToLoad;

		
	public void loadTheScene() {
		Debug.Log ("click received");
		SceneManager.LoadScene (sceneToLoad);
	}
}
