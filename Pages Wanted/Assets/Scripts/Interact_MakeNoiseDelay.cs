using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_MakeNoiseDelay : Interact_MakeNoise {
	// Public or Serialized Variables for Inspector -----------------
	#region Public Variables
	public AudioSource delayNoise;
	[Range(0,5)]
	public float delayNoiseVolume;
	public int delayTime;
	public float randomNoiseDelayTime = 60F;
	#endregion

	// Public Methods -------------------------------------------------
	#region Public Methods
	public override void onInteract(Character_Inventory inv) {
		PlayAnimation();
		delayNoise.Play();
		soundScript.sound = delayNoiseVolume;
		StartCoroutine("timer");
	}
	#endregion

	// Private Methods -------------------------------------------------
	#region Private Methods
	private IEnumerator timer() {
		yield return new WaitForSeconds(delayTime);
		delayNoise.Stop();
		StartCoroutine("MakeNoise");
	}

	protected override void MakeRandomNoise() {
		StartCoroutine("ChimeRegularly");
	}

	private IEnumerator ChimeRegularly() {
		yield return new WaitForSeconds(randomNoiseDelayTime);
		StartCoroutine("MakeNoise");
	}
	#endregion
}
