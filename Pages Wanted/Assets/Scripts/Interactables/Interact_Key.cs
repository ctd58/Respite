using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact_Key : Interactables {
	// Public or Serialized Variables for Inspector -----------------
	public KeyTypes type;
	// Private Variables ---------------------------------------------
	private int id;

	// Setup Methods -------------------------------------------------
	new void Start () {
		base.Start();
		SetSprite(Interact_Icon_Type.PICKUP);
		id = this.gameObject.GetInstanceID();
	}

	// Private Methods -------------------------------------------------
	// Add this key to the list of keys in inventory
	public override void onInteract(Character_Inventory inv) {
		base.onInteract(inv);
		Key_Obj newKey = new Key_Obj();
		newKey.keyId = id;
		newKey.type = type;
		inv.addKey(newKey);
		Destroy(this.gameObject, noise.GetAudioClipLength());
	}
}
