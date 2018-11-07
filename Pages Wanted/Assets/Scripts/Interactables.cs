using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour {


    // Public or Serialized Variables for Inspector -----------------
    #region Public Variables
    [Header("Sound Attributes")]
    [SerializeField] protected bool makesNoise = false;
    public Interact_MakeNoise noise; //Interactables has a makeNoise 
    #endregion

    // Private Variables ---------------------------------------------
    #region Private Variables
    protected Sprite spriteIcon;
    #endregion

    protected void Start() {
        if (makesNoise) {
            SetSprite(Interact_Icon_Type.MAKENOISE);
            noise.setAudioClip(GetComponent<AudioSource>());
            noise.setSoundLength();
            if (noise.makeRandomNoises) StartCoroutine(MakeRandomNoise());

        }
        else SetSprite(Interact_Icon_Type.PICKUP);
    }

    // Public Methods ------------------------------------------------
    #region Public Methods
    //onInteract version, can be overloaded
    public virtual void onInteract(Character_Inventory inv) {
        PlayAnimation();
        if (makesNoise) StartCoroutine(noise.MakeNoise());
    }

	public Sprite GetSprite() {
		return spriteIcon;
	}

	public void SetSprite(Interact_Icon_Type type) {
		spriteIcon = Interact_Icon.GetSprite(type);
	}
	#endregion

	// Private Methods -------------------------------------------------
	#region Private Methods
    protected virtual void PlayAnimation() {
		//TODO: stuff here
	}

	protected IEnumerator MakeRandomNoise() {
		yield return new WaitForSeconds(noise.randomNoiseDelayTime);
		StartCoroutine(noise.MakeNoise());
	}
	#endregion
}
