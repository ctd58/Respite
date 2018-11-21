using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour {

    public Monster monster;
    public ControllerMovement player;
    public Sound pSound;
    public bool wasCalled = false; 
    public bool canStun = true;
    public bool canSlow = true;
    public float slowSpeedInc = 0.1f;
    public float maxSlowSpeed = 0.5f;
    public float stunLen = 2.0f;
    public float castLen = 0.0f; //sec
    public float maxCastLen = 5.0f;//sec
    public float inputDelay = 2.0f; //sec
    public bool stopFun = false;
    public string button = "";
    public float soundInc = 0.001f;

    // Use this for initialization
    void Start()
    {
        monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<Monster>();
        player = this.GetComponent<ControllerMovement>();
        pSound = this.GetComponent<Sound>();
        if(this.tag == "P1")
        {
            button = "P1bo button";
            canSlow = true;
            canStun = false;
        }
        if(this.tag == "P2")
        {
            button = "P2bo button";
            canSlow = false;
            canStun = true;
        }
        

        
    }

    // Update is called once per frame
    void Update()
    {
        //What is going on here - how come you have to hold down
        if (Input.GetButton(button) && !player.GetCanMove())
        {
            
            ability1();
        }
        else
        {
            if (this.GetComponent<AudioSource>().isPlaying)
                this.GetComponent<AudioSource>().Stop();
            castLen = 0;
            pSound.sound = 0;
        }
    }

    public bool WasAbilityCalled() {
        return wasCalled; 
    }

    private void ability1()
    {
        maxCastLen = 5.0f;
        //Debug.Log("Got the button for " + pTag);
        if (!stopFun)
        {
            if (!this.GetComponent<AudioSource>().isPlaying) {
                this.GetComponent<AudioSource>().Play();
                wasCalled = true; 
            }
            castLen += Time.deltaTime;

            pSound.sound = pSound.sound + soundInc;
            //Need to figure out how loud something can get

            //here is where monster can be slowed. 
            if (canSlow && monster.slowSpeed > maxSlowSpeed)
            {
                monster.slowSpeed -= slowSpeedInc;
            }
        }
        if (castLen >= maxCastLen)
        {
            this.GetComponent<AudioSource>().Stop();
            pSound.sound = 0.0f;
            castLen = 0;
            monster.slowSpeed = 1.0f;
            if (canStun) {
                //monster.Stun();
            }
            StartCoroutine(stopInput());
            pSound.sound = 0.0f;

        }
    }

    IEnumerator stopInput() 
    {
        stopFun = true;
        wasCalled = true;
        yield return new WaitForSecondsRealtime(inputDelay);
        stopFun = false;
        wasCalled = false; 
        //Debug.Log(Time.time);
    }
}
