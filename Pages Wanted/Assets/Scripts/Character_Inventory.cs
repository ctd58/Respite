using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Inventory : MonoBehaviour {
	private Sprite normal;
	private string interactButton;
	private Interactables interactable;
	private List<Key_Obj> playerKeys;
	public Camera playerCam;
	public Image uiIndicator;
    public bool isP1;
	public float playerInteractDistance = 200F;
   


	void Start () {
		playerKeys = new List<Key_Obj>();
		if(this.tag == "P1")
        {
            isP1 = true;
        }
        else
        {
            isP1 = false;
        }
		interactButton = isP1 ? "P1ax button" : "P2xs button";
		//TODO: add logic to get camera automatically rather than needing it to be a public var
		//TODO: Same for uiIndicator
		Sprite[] sprites = Resources.LoadAll<Sprite>("UI_Sprites/PointerIcons_MASTER");
		normal = sprites[1];
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
				uiIndicator.sprite = interact.interactIcon;
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


