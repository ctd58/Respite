using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Inventory : MonoBehaviour {
    private string interact = "xs button";

	public List<Key_Obj> keys;
	private List<GameObject> keysInRange;

	void Start () {
		keys = new List<Key_Obj>();
		keysInRange = new List<GameObject>();
	}
	
	void Update () {
		if (Input.GetButton(interact))
		{
			if (keysInRange.Count > 0) {
				CollectKeys();
			}
			//TODO: add collect script for non-key objects
		}
	}

	// Maintain list of objects that are currently in range to be picked up
	void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.GetComponent<Interact_Key>().Equals(null)) {
			Debug.Log("Found a key! " + other);
			keysInRange.Add(other.gameObject);
		}
		//TODO: add collect behavior for non-key objects
	}

	// Maintain list of objects that are currently in range to be picked up
	void OnTriggerExit(Collider other)
	{
		if (!other.gameObject.GetComponent<Interact_Key>().Equals(null)) {
			Debug.Log("Left a key!");
			keysInRange.Remove(other.gameObject);
		}
		//TODO: add collect behavior for non-key objects
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
		keys.Add(newKey);
		// Destroy key in scene
		keysInRange.RemoveAt(0);
		Destroy(collecting);
	}
}


