using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Switch : Interactables {

	// Public or Serialized Variables for Inspector -----------------
	#region Public Variables
	public List<Switch_Target> targets;
    public Animator myAnim;
	#endregion

	// Private Variables ---------------------------------------------
	#region Private Variables
	bool isActivated = false;
	#endregion

	new void Start() {
		base.Start();
		SetSprite(Interact_Icon_Type.PICKUP);
	}

	public override void onInteract(Character_Inventory inv) {
        base.onInteract(inv);
		foreach (Switch_Target target in targets) {
			if (!isActivated) target.onSwitchActivate();
			else target.onSwitchDeactivate();
		}
		isActivated = !isActivated;

        if (myAnim != null) {
            myAnim.Play("Pull");
        }
        
	}
}
