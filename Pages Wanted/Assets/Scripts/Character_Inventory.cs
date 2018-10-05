using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Inventory : MonoBehaviour {
    private string interact = "xs button";

	public List<Key_Obj> playerKeys;
	private List<GameObject> keysInRange;
	private GameObject doorInRange;

	void Start () {
		playerKeys = new List<Key_Obj>();
		keysInRange = new List<GameObject>();
	}
	
	void Update () {
		if (Input.GetButton(interact))
		{
			if (keysInRange.Count > 0) {
				CollectKeys();
			}
			//TODO: add collect script for non-key objects
			else if (doorInRange != null) {
				UnlockDoor();
			}
		}
	}

	// Maintain list of objects that are currently in range to be picked up
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<Interact_Key>()) {
			Debug.Log("Found a key! " + other);
			keysInRange.Add(other.gameObject);
		}
		//TODO: add collect behavior for non-key objects
		if (other.gameObject.GetComponent<Door_Unlock>()) {
			Debug.Log("Found a door! " + other);
			doorInRange = other.gameObject;
		}
	}

	// Maintain list of objects that are currently in range to be picked up
	void OnTriggerExit(Collider other)
	{
		// Check for keys in range
		if (other.gameObject.GetComponent<Interact_Key>()) {
			Debug.Log("Left a key!");
			keysInRange.Remove(other.gameObject);
		}
		//TODO: add collect behavior for non-key objects

		// Check for doors in range
		if (other.gameObject.GetComponent<Door_Unlock>()) {
			Debug.Log("Left a door! " + other);
			doorInRange = null;
		}
	}

	void CollectKeys() {
		// Get the first key in range (if we want all keys to be picked up on interact, we could change this)
		GameObject collecting = keysInRange[0];
		Debug.Log("Collecting " + collecting);
		// Add this key to the list of keys in inventory
		Interact_Key keyData = collecting.GetComponent<Interact_Key>();
		Key_Obj newKey = new Key_Obj();
		newKey.keyId = keyData.getId();
		newKey.type = keyData.type;
		playerKeys.Add(newKey);
		// Destroy key in scene
		keysInRange.RemoveAt(0);
		Destroy(collecting);
	}

	void UnlockDoor() {
		// Trigger unlock script in Door_Unlock
		if (playerKeys.Count > 0) {
			doorInRange.GetComponent<Door_Unlock>().UnlockLocks(playerKeys);
		}
	}
}


