using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Interact_Icon_Type {
    /// <summary>
	/// Public enum, controls dropdowns for interact icons
    /// </summary>

	MAKENOISE,
	NORMAL,
	PICKUP,
	UNLOCK
};

public static class Interact_Icon {
    /// <summary>
	/// Public object to encapsulate getting and setting Interact Icon
    /// </summary>
	public static Sprite GetSprite(Interact_Icon_Type type) {
		Object[] sprites;
        sprites = Resources.LoadAll ("UI_Sprites/PointerIcons_MASTER");
		switch(type) {
			case Interact_Icon_Type.MAKENOISE:
				return (Sprite)sprites[1];
			case Interact_Icon_Type.NORMAL:
				return (Sprite)sprites[2];
			case Interact_Icon_Type.PICKUP:
				return (Sprite)sprites[3];
			case Interact_Icon_Type.UNLOCK:
				return (Sprite)sprites[4];
			default:
				return (Sprite)sprites[2];
		}
	}
}