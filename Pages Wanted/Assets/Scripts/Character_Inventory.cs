using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Inventory : MonoBehaviour {
	// Public or Serialized Variables for Inspector -----------------
	#region Public Variables
	public Camera playerCam;
	public Image uiIndicator; // TODO: create UI manager to handle this
	[SerializeField] [Range(100, 300)] private float playerInteractDistance = 200F;
    #endregion

	// Private Variables ---------------------------------------------
	#region Private Variables
	private bool isP1;
	private Sprite normal;
	private string interactButton;
	private Interactables interactable;
	private List<Key_Obj> playerKeys;
	#endregion

	void Start () {
		playerKeys = new List<Key_Obj>();
		if(this.tag == "P1") isP1 = true;
        else isP1 = false;
		interactButton = isP1 ? "P1ax button" : "P2xs button";
		normal = Interact_Icon.GetSprite(Interact_Icon_Type.NORMAL);
		// TODO: add logic to get camera automatically rather than needing it to be a public var
	}
	
	void Update () {

		CheckForInteractables();


		if (Input.GetButton(interactButton))
		{
            
			if (interactable != null) {
                Debug.Log("Here");
                interactable.onInteract(this);
			}
		}
	}

	void CheckForInteractables() {
		//Debug.Log("Checking");
		Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, playerInteractDistance)) {
			if (hit.transform.gameObject.GetComponent<Interactables>()) {
				Interactables interact = hit.transform.gameObject.GetComponent<Interactables>();
				uiIndicator.sprite = interact.GetSprite();
				interactable = interact;
			} else {
				uiIndicator.sprite = normal;
				interactable = null;
			}
		} else {
			uiIndicator.sprite = normal;
			interactable = null;
		}
	}

	public List<Key_Obj> getKeys() {
		return playerKeys;
	}

	public void addKey(Key_Obj newKey) {
		playerKeys.Add(newKey);
	}

}


