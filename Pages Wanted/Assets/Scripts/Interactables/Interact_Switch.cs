using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Switch : Interactables {

	// Public or Serialized Variables for Inspector -----------------
	#region Public Variables
	public List<Switch_Target> targets;
	#endregion

	// Private Variables ---------------------------------------------
	#region Private Variables
	bool isActivated = false;
	#endregion

	new void Start() {
		base.Start();
	}

	public override void onInteract(Character_Inventory inv) {
		foreach (Switch_Target target in targets) {
			if (!isActivated) target.onSwitchActivate();
			else target.onSwitchDeactivate();
		}
		isActivated = !isActivated;
	}
}
