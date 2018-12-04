using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour {


    // Public or Serialized Variables for Inspector -----------------
    #region Public Variables
    [Header("Sound Attributes")]
    [SerializeField] protected bool makesNoise = false;
    // The MakeNoise script is a non-MonoBehavior script that collects many sound-related settings into one place
    // It in turn has reference to the Sound script
    // This means that any objects with MakeNoise that have makesNoise set to true need to also have:
    //   - a Sound script component
    //   - an Audio Source component
    // Otherwise errors will be thrown
    public Interact_MakeNoise noise; 
    // TODO: the MakeNoise script was made before I realized non-MonoBehaviors cannot call coroutines, so the 
    //    logic there is a little needlessly complex. This inheritance structure needs to be simplified with 
    //     these limitations in mind.
    #endregion

    // Private Variables ---------------------------------------------
    #region Private Variables
    protected Sprite spriteIcon;
    // spriteIcon stores which icon should appear in the reticle UI when the object is hovered over
    protected MonsterAction action = null;
    // action stores a MonsterAction to be triggered when this object is interacted with
    // TODO: just realized that under this system, an object can only trigger a single monster action
    //    This limitation is not a problem yet, but should be kept in mind when designing levels
    //    Changing this to an array should possibly mitigate this issue, to look into later
    #endregion

    // Start sets up everything necessary for the MakeNoise script to function properly, as well as
    //    setting the spriteIcon to the appropriate symbol (default is the ear icon)
    protected void Start() {
        if (makesNoise) {
            SetSprite(Interact_Icon_Type.MAKENOISE);
            noise.setAudioClip(GetComponent<AudioSource>());
            noise.setSoundLength();
            noise.setSoundScript(GetComponent<Sound>());
            if (noise.makeRandomNoises) StartCoroutine(MakeRandomNoise());

        }
        else SetSprite(Interact_Icon_Type.PICKUP);
    }

    // Public Methods ------------------------------------------------
    #region Public Methods
    //onInteract is called by the Character_Inventory scrupt, often overloaded in subclasses
    public virtual void onInteract(Character_Inventory inv) {
        // If this function must be overwritten in subclass, add these following lines to ensure that
        //    animations, sounds and monster actions are still triggered properly
        PlayAnimation();
        // if it makes noise, trigger that noise
        if (makesNoise) StartCoroutine(noise.MakeNoise());
        // if there is a monster action to trigger, trigger that action
        if (action!=null) { 
            GameObject.Find("MonsterManager").GetComponent<MonsterManager>().TriggerMonsterAction(action);
        }
    }

	public Sprite GetSprite() {
		return spriteIcon;
	}

	public void SetSprite(Interact_Icon_Type type) {
		spriteIcon = Interact_Icon.GetSprite(type);
	}

    public void SetMonsterAction(MonsterAction newAction) {
        action = newAction;
    }
	#endregion

	// Private Methods -------------------------------------------------
	#region Private Methods
    protected virtual void PlayAnimation() {
		//TODO: stuff here
        // Chris, re: Cross script, it is entirely likely that you don't even need to add a 
        //    new script, you might be able to just create an animation and then add the logic here
        //    to get the Animation Manager of the object and play the animation.
        // Just be sure to add checks to ensure there is an animation manager to account for other
        //    Interactable objects that don't yet have animation managers attached.
	}

	protected IEnumerator MakeRandomNoise() {
		yield return new WaitForSeconds(noise.randomNoiseDelayTime);
		StartCoroutine(noise.MakeNoise());
	}
	#endregion
}
