using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class Character_Inventory : MonoBehaviour {
	// Public or Serialized Variables for Inspector -----------------
	#region Public Variables
	public bool debug;
	public Image uiIndicator; // TODO: create UI manager to handle this
	[SerializeField] [Range(100, 300)] private float playerInteractDistance = 200F;
    #endregion

	// Private Variables ---------------------------------------------
	#region Private Variables
	private int playerId; 
    // The Rewired player id of this character
    private Player player; 
    // The Rewired Player
	private bool isP1;
	private Camera playerCam;
	private Sprite normal;
	private string interactButton;
	private Interactables interactable;
	private List<Key_Obj> playerKeys;
	#endregion

	void Start () {
		playerKeys = new List<Key_Obj>();

		isP1 = (this.tag == "P1");

		playerId = (isP1) ? 0 : 1;
        player = ReInput.players.GetPlayer(playerId);

		string playerTag = (isP1) ? "P1" : "P2";
        playerCam = GameObject.Find(playerTag + "Camera").GetComponent<Camera>();
		
		normal = Interact_Icon.GetSprite(Interact_Icon_Type.NORMAL);
	}
	
	void Update () {
		// Check to see if an interactable is in range of player
		CheckForInteractables();

		if (player.GetButton("Interact")) { 
			if (interactable != null) {
                if (debug) Debug.Log("Interacting with " + interactable.name);
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


