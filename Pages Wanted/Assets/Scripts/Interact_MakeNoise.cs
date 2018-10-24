using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_MakeNoise : Interactables {

	private AudioSource audioClip;
	protected Sound soundScript;
	[Range(0, 5)]
	public float soundVolume;
	public float soundLength;

	void Start()
	{
		audioClip = this.GetComponent<AudioSource>();
		soundScript = this.GetComponent<Sound>();
		if (soundLength.Equals(0)) {
			soundLength = audioClip.clip.length;
		}
		Debug.Log("SOUND LENGTH " + soundLength);
		MakeRandomNoise();
	}

	
	public override void onInteract(Character_Inventory inv) {
		
	}

	protected IEnumerator MakeNoise() {
		soundScript.sound = soundVolume;
		audioClip.Play();
		yield return new WaitForSeconds(soundLength);
		audioClip.Stop();
		soundScript.sound = 0;
	}

	public virtual void MakeRandomNoise() {}
}
