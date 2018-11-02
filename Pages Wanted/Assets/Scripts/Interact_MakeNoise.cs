using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_MakeNoise : Interactables {
	// Public or Serialized Variables for Inspector -----------------
	#region Public Variables
	[SerializeField] [Range(0, 5)] protected float soundVolume;
	[SerializeField] protected float soundLength;
	[SerializeField] protected bool randomNoises = true;
	#endregion

	// Private Variables ---------------------------------------------
	#region Private Variables
	private AudioSource audioClip;
	protected Sound soundScript;
	#endregion

	// Setup Methods -------------------------------------------------
	#region Setup Methods
	protected void Start() {
		SetSprite(Interact_Icon_Type.MAKENOISE);
		audioClip = this.GetComponent<AudioSource>();
		soundScript = this.GetComponent<Sound>();
		if (soundLength.Equals(0)) {
			soundLength = audioClip.clip.length;
		}
		if (randomNoises) MakeRandomNoise();
	}
	#endregion

	// Public Methods -------------------------------------------------
	#region Public Methods
	public override void onInteract(Character_Inventory inv) {
		PlayAnimation();
		StartCoroutine("MakeNoise");
	}
	#endregion

	// Private Methods -------------------------------------------------
	#region Private Methods
	protected virtual void MakeRandomNoise() {}

	protected virtual void PlayAnimation() {
		//TODO: stuff here
	}

	protected IEnumerator MakeNoise() {
		soundScript.sound = soundVolume;
		audioClip.Play();
		yield return new WaitForSeconds(soundLength);
		audioClip.Stop();
		soundScript.sound = 0;
	}
	#endregion
}
