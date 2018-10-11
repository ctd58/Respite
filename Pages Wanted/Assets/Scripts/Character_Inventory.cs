using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Inventory : MonoBehaviour {
	private Sprite normal;
	private Sprite makeNoise;
	private Sprite pickUp;
	private Sprite unlock;
	private string interact;
	private GameObject keyInRange;
	private GameObject doorInRange;
	public List<Key_Obj> playerKeys;
	public Camera playerCam;
	public Image uiIndicator;


	void Start () {
		playerKeys = new List<Key_Obj>();
		bool isP1 = this.gameObject.GetComponent<ControllerMovement>().isP1;
		interact = isP1 ? "P1xs button" : "P2xs button";
		//TODO: add logic to get camera automatically rather than needing it to be a public var
		//TODO: Same for uiIndicator
		Sprite[] sprites = Resources.LoadAll<Sprite>("UI_Sprites/PointerIcons_MASTER");
		normal = sprites[1];
		makeNoise = sprites[0];
		pickUp = sprites[2];
		unlock = sprites[3];
	}
	
	void Update () {
		doorInRange = null;
		keyInRange = null;
		
		CheckForInteractables();

		if (Input.GetButton(interact))
		{
			if (keyInRange != null) {
				CollectKeys();
			}
			//TODO: add collect script for non-key objects
			else if (doorInRange != null) {
				UnlockDoor();
			}
		}
	}

	void CheckForInteractables() {
		Debug.Log("Checking");
		Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 2)) {
			if (hit.transform.gameObject.GetComponent<Door_Unlock>()) {
				Debug.Log(unlock);
				uiIndicator.sprite = unlock;
				doorInRange = hit.transform.gameObject;
			} else if (hit.transform.gameObject.GetComponent<Interact_Key>()) {
				uiIndicator.sprite = pickUp;
				keyInRange = hit.transform.gameObject;
			} /*else if (hit.transform.gameObject.GetComponent<Interact_Noise>()) {
				Debug.Log("Found a noise making object! " + hit.transform.name);
				;
			} */ else {
				uiIndicator.sprite = normal;
			}
		} else {
			uiIndicator.sprite = normal;
		}
	}

	void CollectKeys() {
		GameObject collecting = keyInRange;
		Debug.Log("Collecting " + collecting);
		// Add this key to the list of keys in inventory
		Interact_Key keyData = collecting.GetComponent<Interact_Key>();
		Key_Obj newKey = new Key_Obj();
		newKey.keyId = keyData.getId();
		newKey.type = keyData.type;
		playerKeys.Add(newKey);
		// Destroy key in scene
		keyInRange = null;
		Destroy(collecting);
	}

	void UnlockDoor() {
		// Trigger unlock script in Door_Unlock
		if (playerKeys.Count > 0) {
			doorInRange.GetComponent<Door_Unlock>().UnlockLocks(playerKeys);
		}
	}
}


