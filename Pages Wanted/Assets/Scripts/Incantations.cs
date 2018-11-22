using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incantations : MonoBehaviour {

    //Public variables
    [Range(1.0f, 10.0f)]public float castTime = 2.0f; 

    //Private variables
    private MonsterManager mm;
    private Sound pSound;
    private ControllerMovement playermove;
    private string[] incantationbuttons;
    private string[] incantationtypes;
    private int incantationcounter;
    private bool wasCalled;
    private string lastIncantationUsed = "none"; 
    private float cooldownTimer = 2.0f; 


    //Public methods

    void Start() {
        //Hooks up all private variables
        mm = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<MonsterManager>();
        playermove = this.GetComponent<ControllerMovement>();
        pSound = this.GetComponent<Sound>();
        //Should only have 1 incantation at the start, if statement decides what type of incantation
        incantationcounter = 1; 
        if (this.tag == "P1") {
            incantationbuttons = new string [] {"P1bo button"};
            incantationtypes = new string[] { "Slow" }; 
        }
        if (this.tag == "P2") {
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

    //Checks if there is a button that we mapped an incantation type to, triggers
    //coroutine if it is
    private void IncantationCounter() {
        for (int i = 0; i < incantationcounter; i++) {
            if (Input.GetButton(incantationbuttons[i])) {
                StartCoroutine(incantationtypes[i]);
                wasCalled = true; 
                break;
            } 
        }
    }

    //Possible coroutines

    //For the slow incantation
    IEnumerator Slow() {
        yield return new WaitForSecondsRealtime(castTime);
        StartCoroutine(mm.ChangeSpeed(20.0f, 5.0f)); 
    }

    //For the stun incantation
    IEnumerator Stun() {
        yield return new WaitForSecondsRealtime(castTime);
        //StartCoroutine(mm.Stun(5.0f));
    }

    //Cooldown coroutine to my knowledge
    IEnumerator Cooldown() {
        //stopFun = true;
        wasCalled = true;
        yield return new WaitForSecondsRealtime(cooldownTimer);
        //stopFun = false;
        wasCalled = false;
        //Debug.Log(Time.time);
    }
}
