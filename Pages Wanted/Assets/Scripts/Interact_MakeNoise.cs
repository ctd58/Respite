using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_MakeNoise : Interactables {

	private AudioSource audioClip;
	private Sound soundScript;
	[Range(0, 5)]
	public float soundVolume;

	void Start()
	{
		audioClip = this.GetComponent<AudioSource>();
		soundScript = this.GetComponent<Sound>();
	}

	
	public override void onInteract(Character_Inventory inv) {
		
	}

	protected void MakeNoise() {
		soundScript.sound = soundVolume;
		audioClip.Play();
	}

	
}
