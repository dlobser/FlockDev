using System;
using UnityEngine;

	public class AudioManager :MonoBehaviour
	{
	public AudioClip[] audioClips;
	public AudioSource audioSource;

	private ActorDataSync actorDataSync;

	bool audioChanged=false;

	void Start(){
		audioSource.clip = audioClips [0];
		audioSource.Play ();
		actorDataSync = GameObject.Find ("ActorSynchronizableManager").GetComponent<ActorDataSync> ();

	}

	void Update(){

		int bugsEaten = actorDataSync.ActorBugsEaten ();

		if (bugsEaten > 5 && !audioChanged) {
			audioSource.clip = audioClips [1];
			audioSource.Play ();
			audioChanged = true;
		}

	}

	}
