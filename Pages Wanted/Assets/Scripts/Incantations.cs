using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incantations : MonoBehaviour {

    //Public variables
    

    //Private variables
    private GameObject player;
    private MonsterManager mm;
    private Sound pSound; 

    //hello

    //hello1

    //Public methods

    void Start() {
        //Hooks up all private variables
        mm = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();
        playermove = this.GetComponent<ControllerMovement>();
        pSound = this.GetComponent<Sound>();
        uiManager = GameObject.Find("Canvas").GetComponent<OverallUIManager>(); 
        //Should only have 1 incantation at the start, if statement decides what type of incantation
        incantationcounter = 1; 
        if (this.tag == "P1") {
            charManager = uiManager.P1UI; 
            playernum = "P1"; 
            incantationbuttons = new string [] {"P1bo button"};
            incantationtypes = new string[] { "Slow" }; 
        }
        if (this.tag == "P2") {
            charManager = uiManager.P2UI;
            playernum = "P2"; 
            incantationbuttons = new string[] { "P2bo button" };
            incantationtypes = new string[] { "Stun" };
        }

    }



    void Update() {
    //If an incantation has not been called at the moment, check to see if one will be called
        if (!wasCalled && !playermove.CanMove()) {
            IncantationCounter(); 
        }
    }

    bool WasIncantationCalled() {
        return wasCalled;
    }

    string getCurrentIncantationName() {
        return lastIncantationUsed; 
    }
    //Private methods


    //Possible coroutines
}
