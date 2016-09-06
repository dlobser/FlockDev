using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class collideToEndScene : MonoBehaviour {

	public string sceneToLoad;

	void OnTriggerEnter ( Collider other) { 
		if (other.CompareTag("sceneEnder")) { 
//			nowAction.TransitionTo (m_TransitionIn);
			Debug.Log ("entering sceneEnder");
			SceneManager.LoadScene (sceneToLoad);
		}
	}	


}
