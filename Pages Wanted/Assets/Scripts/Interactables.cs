using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactables : MonoBehaviour {
	// I'm a little new to inheritence, so this structure is experimental. Feel free to suggest changes

	public Sprite interactIcon;

	public virtual void onInteract(Character_Inventory inv) {

	}
}
