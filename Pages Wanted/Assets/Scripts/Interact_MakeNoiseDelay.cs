using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_MakeNoiseDelay : Interact_MakeNoise {

	//TODO: delay noise
	public AudioSource delayNoise;
	[Range(0,5)]
	public float delayNoiseVolume;

	public int delayTime;

	public override void onInteract(Character_Inventory inv) {
		PlayAnimation();
		// delayNoise.Play();
		// soundScript.sound = delayNoiseVolume;
		StartCoroutine("timer");
	}

	public void PlayAnimation() {
		//TODO: stuff here
	}

	IEnumerator timer() {
		yield return new WaitForSeconds(delayTime);
		MakeNoise();
	}
}
