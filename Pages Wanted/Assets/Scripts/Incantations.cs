using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incantations : MonoBehaviour {


    //Certain lines may need to be moved to Narration/AudioManager

    //Certain lines may need to be moved to AnimationManager

    //Public variables
    [Range(1.0f, 10.0f)]public float castTime = 2.0f;

    //Private variables
    private OverallUIManager uiManager;
    private CharUIManager charManager; 
    private MonsterManager mm;
    private Sound pSound;
    private ControllerMovement playermove;
    private string[] incantationbuttons;
    private string[] incantationtypes;
    private int incantationcounter;
    private string playernum; 
    private bool wasCalled;
    private string lastIncantationUsed = "none"; 
    private float cooldownTimer = 10.0f; 


    //Public methods

    void Start() {
        //Hooks up all private variables
        mm = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<MonsterManager>();
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

    //Checks if there is a button that we mapped an incantation type to, triggers
    //coroutine if it is
    private void IncantationCounter() {
        for (int i = 0; i < incantationcounter; i++) {
            if (Input.GetButton(incantationbuttons[i])) {
                StartCoroutine(incantationtypes[i]); 
                break;
            } 
        }
    }

    //Possible coroutines

    //For the slow incantation
    IEnumerator Slow() {
        StartCoroutine(Cooldown());
        yield return new WaitForSecondsRealtime(castTime);
        StartCoroutine(mm.ChangeSpeed(20.0f, 5.0f));
        //Amount of time monster should be slowed -- Also should slow voice lines down for monster
        yield return new WaitForSecondsRealtime(15.0f);
        //Amount of time monster should get back to base form-- Also should speed up voice lines down for monster
        StartCoroutine(mm.ReturnToBaseSpeed(3.0f));
    }

    //For the stun incantation
    IEnumerator Stun() {
        StartCoroutine(Cooldown()); 
        yield return new WaitForSecondsRealtime(castTime);
        //Next three lines may need to be moved.
        StartCoroutine(mm.ChangeSpeed(0, 1f));
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(mm.ReturnToBaseSpeed(3f));

    }

    //Cooldown coroutine to my knowledge
    IEnumerator Cooldown() {
        //stopFun = true;
        wasCalled = true;
        charManager.SetCooldownCover(wasCalled); 
        yield return new WaitForSecondsRealtime(cooldownTimer);
        //stopFun = false;
        wasCalled = false;
        charManager.SetCooldownCover(wasCalled);
        //Debug.Log(Time.time);
    }
}
