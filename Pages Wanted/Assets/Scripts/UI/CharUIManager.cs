﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CharUIManager : MonoBehaviour {
 
    //Will be made private in the future
    public GameObject incantationPanel;
    public GameObject cameraCover;
    private GameObject player;
    private PauseScreenNav pauseScreen;
    private int counter = 0; 

	// Use this for initialization
	void Start () {
        if (this.tag == "P1Panel") {
            player = GameObject.FindGameObjectWithTag("P1");
            incantationPanel.SetActive(false);
        }
        else {
            player = GameObject.FindGameObjectWithTag("P2");
            incantationPanel.SetActive(false);
        }
        SetCooldownCover(false);
        //StartCoroutine("ShowInsignia");
    }
	
    //Coroutine for flashing damage indicator not working, brute forcing through monster for now
    /*
    IEnumerator ShowInsignia() {
        if (player.GetComponent<ControllerMovement>().imageBrand.enabled == true) {
            player.GetComponent<ControllerMovement>().imageBrand.enabled = true;
        }
        yield return new WaitForSeconds(5);
       player.GetComponent<ControllerMovement>().imageBrand.enabled = false;
    }
    */
    
    public string GetPlayer() {
        return player.tag; 
    }
    
    public void SetCooldownCover(bool cover) {
        if (cover == true) {
            incantationPanel.SetActive(true);
        }
        else {
            incantationPanel.SetActive(false);
        }
    }

    /* private void showIncantationCooldown() {
        bool check = player.GetComponent<Abilities>().WasAbilityCalled(); 
        if (check == true) {
            incantationPanel.SetActive(true); 
        }
        else {
            incantationPanel.SetActive(false); 
        }
    }*/

    private void showCameraCover() {
       if (player.GetComponent<ControllerMovement>().CanMove() == true) {
            cameraCover.SetActive(false); 
        }
        else {
            cameraCover.SetActive(true); 
        }
    }

	// Update is called once per frame
	void Update () {
        showCameraCover();
        //showIncantationCooldown(); 
	}
}
