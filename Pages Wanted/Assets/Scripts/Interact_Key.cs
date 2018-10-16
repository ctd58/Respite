using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact_Key : Interactables {

	public KeyTypes type;
	private int id;


	void Start () {
		id = this.gameObject.GetInstanceID();
	}

	// Add this key to the list of keys in inventory
	public override void onInteract(Character_Inventory inv) {
		Key_Obj newKey = new Key_Obj();
		newKey.keyId = id;
		newKey.type = type;
		inv.addKey(newKey);
		Destroy(this.gameObject);
	}

}
