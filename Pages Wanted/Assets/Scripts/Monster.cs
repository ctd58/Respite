﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour {
    // Public or Serialized Variables for Inspector -----------------
    [SerializeField] [Range(2.0F, 10.0F)] private float sensePlayerDistance = 2;
    [SerializeField] [Range(0.0F, 3.5F)] private float chaseTime = 25.0f;
    [SerializeField] [Range(3.0F, 8.0F)] private float baseMoveSpeed = 4.0f;
    [SerializeField] [Range(3.0F, 10.0F)] private float stunDelay = 5.0f;
    [SerializeField] [Range(10.0F, 20.0F)] private float rotSpeed = 15.0f;
    public List<Transform> wayPoints = new List<Transform>();
    public float slowSpeed = 1.0f;
    public int playerLives = 4;
    public GameObject health1;
    public GameObject health2;
    public GameObject health3;
    public GameObject health4;
    public GameObject gameoverScreen;
    
    // Private Variables ----------------------------------------------
    private Transform currentWaypoint;
    private int i = 0;
    private int counter = 0;
    private bool madeWaypoint;
    private NavMeshAgent navMeshAgent;     
    private float currentMovSpeed;
    private Transform target = null;
    private GameObject[] soundObjects;
    private List<GameObject> players = new List<GameObject>();
    private GameObject[] spawn;
    private enum STATE { WANDER, INSPECT, ATTACK, STUNNED }
    private STATE _currentState;

    void Start()
    {
        /*
        GameObject other = GameObject.Find("TitleScreenNav");
        TitleScreenNav titleScreenNav = other.GetComponent<TitleScreenNav>();
        if (titleScreenNav != null)
        {
            baseMoveSpeed = titleScreenNav.monsterBaseSpeed.value;
            sensePlayerDistance = titleScreenNav.monsterSense.value;
        }
        */
        Debug.Log(PlayerPrefs.GetFloat("monsterbasespeed") + "  " + PlayerPrefs.GetFloat("monstersense"));
        checkprefs(); 
        baseMoveSpeed = PlayerPrefs.GetFloat("monsterbasespeed");
        sensePlayerDistance = PlayerPrefs.GetFloat("monstersense"); 
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        spawn = GameObject.FindGameObjectsWithTag("DemonSpawn");
        // gameoverScreen.SetActive(false); 
        soundObjects = GameObject.FindGameObjectsWithTag("MakesSound");
        players.Add(GameObject.FindGameObjectWithTag("P1"));
        players.Add(GameObject.FindGameObjectWithTag("P2"));
        EnterStateWander ();
    }

    void checkprefs()
    {
        //Eventually make max and min v
        if (PlayerPrefs.GetFloat("monsterbasespeed") == 0.0f || PlayerPrefs.GetFloat("monsterbasespeed") > 8.0f || PlayerPrefs.GetFloat("monsterbasespeed") < 4.0f)
        {
            PlayerPrefs.SetFloat("monsterspeed", 5f);
        }
        if (PlayerPrefs.GetFloat("monstersense") == 0.0f || PlayerPrefs.GetFloat("monstersense") < 3.0f || PlayerPrefs.GetFloat("monstersense") > 8.0f)
        {
            PlayerPrefs.SetFloat("monstersense", 6f);
        }
    }

    void Update () {
        Debug.Log(PlayerPrefs.GetFloat("monsterbasespeed") + "  " + PlayerPrefs.GetFloat("monstersense"));
        switch (_currentState) {
		case STATE.WANDER:
			UpdateWander ();
			break;
		case STATE.INSPECT:
			UpdateInspect ();
			break;
		case STATE.ATTACK:
			UpdateAttack ();
			break;
		case STATE.STUNNED:
			UpdateStun();
			break;
		}
        // Widchard use this to call game over screen
	}

    // WANDER STATE ---------------------------------------------------------------------

    // code to setup wandering
    private void EnterStateWander() {
        currentMovSpeed = baseMoveSpeed; 
		_currentState = STATE.WANDER;
        currentWaypoint = wayPoints[i];
        madeWaypoint = true; 
	}

	private void UpdateWander() {
        counter++;
        if (counter > 400) {
            counter = 0;
            madeWaypoint = true; 
        } else {
            if (currentWaypoint != null && madeWaypoint == true) {
                Vector3 targetV = currentWaypoint.transform.position;
                //Need to make this based on speed, so the idea is maybe a while loop
                //where we tell the demon to move to a certain distance with base speed, and
                //if it no longer needs to move there because it is there, set the next waypoint.
                navMeshAgent.SetDestination(targetV);
                madeWaypoint = false;
                i = (i + 1) % wayPoints.Count;
                Debug.Log(i); 
            }
            currentWaypoint = wayPoints[i]; 
        }
        if (DetectPlayer()) { EnterStateAttack(); }
        findLoudestSound();
        if (target != null) {
            EnterStateInspect(target); 
        }
	}

    //Determines what object is making the loudest noise and goes to it
    void findLoudestSound() {
        float temp = 0.0f;
        float loudest = 0.0F;
        foreach (GameObject noise in soundObjects) {
            temp = noise.GetComponent<Sound>().sound;
            if (temp > loudest)
            {
                loudest = temp;
                target = noise.transform;
            }
        }
        foreach(GameObject noise in players)
        {
            temp = noise.GetComponent<Sound>().sound;
            if (temp > loudest)
            {
                loudest = temp;
                target = noise.transform;
            }
        }
    }

    private bool DetectPlayer()
    {
        if (players != null)
        {
            if (Vector3.Distance(players[0].gameObject.transform.position, this.transform.position) < sensePlayerDistance)
            {
                target = players[0].transform;
                return true;
            }
            else if (Vector3.Distance(players[1].gameObject.transform.position, this.transform.position) < sensePlayerDistance)
            {
                target = players[1].transform;
                return true;
            }
        }
        return false;
    }

    // INSPECT STATE ---------------------------------------------------------------------
    public void CreateNewWaypoint(){
        //Update function to do smart pathing. 
        if (target != null){
            wayPoints.Add(target);
        }
    }

    public void DeleteNewWaypoint(Transform badwaypoint){
        if (wayPoints.Contains(badwaypoint)){
            //tell that waypoint to die
            wayPoints.Remove(badwaypoint);
        }
    }


    private void EnterStateInspect(Transform target) {
		_currentState = STATE.INSPECT;
		//_currentTarget = target;
        // TODO: add code to set target positition

	}

	private void UpdateInspect() {
        //Finds if a new target is louder
        findLoudestSound();
        //Increases speed if it has been chasing for a multiple of 2.5 seconds
        chaseTime += Time.deltaTime;
        if (chaseTime / 2.5f > 1.0f) {
            chaseTime -= 2.5f;
            currentMovSpeed += 0.75f;
            if(currentMovSpeed> 6.0f) {
                currentMovSpeed = 6.0f;
            }
        }
        FollowSound();
        //Check if it is on top of the targets position
        if(Vector3.Distance(target.position,this.transform.position) < sensePlayerDistance) {
            target = null;
            EnterStateWander();
        }
        if (DetectPlayer()) { EnterStateAttack(); }
	}

    //makes it go towards sound
    void FollowSound()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotSpeed * Time.deltaTime);
        transform.position += transform.forward * (currentMovSpeed * slowSpeed) * Time.deltaTime;
    }

    // ATTACK STATE ---------------------------------------------------------------------

	private void EnterStateAttack() {
		_currentState = STATE.ATTACK;
	}

	private void UpdateAttack() {
        if (DetectPlayer() == true){
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotSpeed * Time.deltaTime);
            transform.position += transform.forward * (currentMovSpeed * slowSpeed) * Time.deltaTime;
        }
    }

    // STUN STATE ---------------------------------------------------------------------

    // Player ability script should call this function
	public void Stun() {
		EnterStateStun ();
	}

	private void EnterStateStun() {
		_currentState = STATE.STUNNED;
		StartCoroutine("stun");
	}

	private void UpdateStun() {
	} // do nothing here, the stun coroutine will set state back to wander

    IEnumerator stun()
    {
        Debug.Log(Time.time);
        yield return new WaitForSecondsRealtime(stunDelay);
        _currentState = STATE.WANDER;
        currentMovSpeed = baseMoveSpeed;
        Debug.Log(Time.time);
    }

    // If they catch the player ---------------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("I gotcha boi");
        if(other.gameObject.tag == "P1" || other.gameObject.tag == "P2")
        {
            target = null;
            playerLives -= 1;
            respawn();
            hideHealth(); 
        }
    }

    private void hideHealth()
    {
        Debug.Log("no health for you");
        health1.SetActive(false);
        if (health1.activeSelf == false && health2.activeSelf == true && health3.activeSelf == true && health4.activeSelf == true)
        {
            health2.SetActive(false);
        }
        else if(health1.activeSelf == false && health2.activeSelf == false && health3.activeSelf == true && health4.activeSelf == true)
        {
            health3.SetActive(false); 
        }
        else if(health1.activeSelf == false && health2.activeSelf == false && health3.activeSelf == false && health4.activeSelf == true)
        {
            health4.SetActive(false); 
        }
        if (playerLives == 0)
        {
            Debug.Log("Game Over");
            //gameoverScreen.SetActive(true);
            SceneManager.LoadScene("GameOverScreen"); 
        }
    }

    void respawn() {
        this.transform.position = spawn[0].transform.position;
        // TODO: add code here that will strategically pick from a number of spawn locations
    }
}
