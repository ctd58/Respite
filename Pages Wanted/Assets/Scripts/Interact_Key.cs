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
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		// if character collider, show interaction prompt
		//TODO: add check for character collider
		//TODO: add text prompt?
		pickupSparkles.gameObject.SetActive(true);
	}

	void OnTriggerExit(Collider other)
	{
		// if character collider, remove interaction prompt
		//TODO: add check for character collider
		//TODO: add text prompt?
		pickupSparkles.gameObject.SetActive(false);
	}

	public int getId() { return id; }
}
