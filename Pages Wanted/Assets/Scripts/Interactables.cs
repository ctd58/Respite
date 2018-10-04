using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactables : MonoBehaviour {

	public Text interactionText;
	public Collider approachArea;
	public ParticleSystem pickupSparkles;

	// I'm a little new to inheritence, so this structure is experimental. Feel free to suggest changes

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) { } // for displaying interaction prompt

	void OnTriggerExit(Collider other) { } // for removing interaction prompt
}
