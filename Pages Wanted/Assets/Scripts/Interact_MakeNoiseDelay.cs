using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_MakeNoiseDelay : Interactables {
	// Public or Serialized Variables for Inspector -----------------
	#region Public Variables
	public AudioSource delayNoise;
	[Range(0,5)]
	public float delayNoiseVolume;
	public int delayTime;
	#endregion

	new void Start() {
		base.Start();
	}

	// Public Methods -------------------------------------------------
	#region Public Methods
	public override void onInteract(Character_Inventory inv) {
		PlayAnimation();
		delayNoise.Play();
		noise.setSound(delayNoiseVolume);
		StartCoroutine("timer");
	}
	#endregion

	// Private Methods -------------------------------------------------
	#region Private Methods
	private IEnumerator timer() {
		yield return new WaitForSeconds(delayTime);
		delayNoise.Stop();
		noise.MakeNoise();
	}
	#endregion
}
