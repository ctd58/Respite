using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interact_MakeNoise {
    // Public or Serialized Variables for Inspector -----------------
    #region Public Variables
    [SerializeField] [Range(0, 5)] protected float soundVolume;
	public bool makeRandomNoises = true;
	public float randomNoiseDelayTime = 60F;
	#endregion

	// Private Variables ---------------------------------------------
	#region Private Variables
	private AudioSource audioSource;
	private Sound soundScript;
	protected float soundLength;
    #endregion
    
    // Setup Methods -------------------------------------------------
    #region Setup Methods
    public void setSoundLength() {
		soundLength = audioSource.clip.length;
	}

	public void setAudioClip(AudioSource newSource) {
		audioSource = newSource;
	}

	public void setSoundScript(Sound newSoundScript) {
		soundScript = newSoundScript;
	}
	#endregion

	// Public Methods -------------------------------------------------
	#region Public Methods
	// public float getSound() {
    //     return sound;
    // }

	public void setSound(float newSound) {
		soundScript.sound = newSound;
	}

	public IEnumerator MakeNoise() {
		soundScript.sound = soundVolume;
		audioSource.Play();
		yield return new WaitForSeconds(soundLength);
		audioSource.Stop();
		soundScript.sound = 0;
	}
	#endregion
}
