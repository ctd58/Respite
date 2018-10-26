using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactables : MonoBehaviour {
	[Header("Interactable Parameters")]
	[Help("This should appear at the top of any interactable object.\nThe interact icon should be set to one of the retical images to show the play what will happen when they interact", UnityEditor.MessageType.None)]
	public Sprite interactIcon;

	public virtual void onInteract(Character_Inventory inv) {

	}
}
