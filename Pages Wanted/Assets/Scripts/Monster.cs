using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour {

    public Transform target = null;
    [Range(3.0F, 8.0F)] public float baseMoveSpeed = 4.0f;
    public float rotSpeed, movSpeed;
    public float distance;
    [SerializeField]
    [Range(2.0F, 10.0F)] public float sensePlayerDistance = 2;
    public float loudestSound = 0.0f;
    public GameObject[] soundObjects;
    public List<GameObject> players;
    [Range(0.0F, 3.5F)] public float chaseTime = 0.0f;
    //chaseTime range 0.0f to 3.5f 
    public float slowSpeed = 1.0f;
    public bool canMove = true;
    public float inputDelay = 0.0f;
    public int playerLives = 4;
    public GameObject spawn;
    [SerializeField]
    public GameObject health1;
    [SerializeField]
    public GameObject health2;
    [SerializeField]
    public GameObject health3;
    [SerializeField]
    public GameObject health4;
    [SerializeField]
    public GameObject gameoverScreen;
    [SerializeField]
    List<Transform> wayPoints = new List<Transform>();
    [SerializeField]
    Transform currentWaypoint;
    int i = 0;
    int counter = 0;
    bool madeWaypoint;
    NavMeshAgent navMeshAgent;

    Slider testslider;
     

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
        spawn = GameObject.FindGameObjectWithTag("DemonSpawn");
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
        movSpeed = baseMoveSpeed; 
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
        findTarget();
        if (target != null) {
            EnterStateInspect(target); 
        }
	}

    //Determines what object is making the loudest noise and goes to it
    void findTarget()
    { //every 2.5 secs the monster is chasing it gets faster by .75
        
        //If not chasing rest speed
        

        //Determines what sound is the loudest and sets it as a target
        //PLEASE try to get the AI to remember the location (waypoint) of where the last sound came from and go to that
        //I cant get it to remember it forgets once the player moves off of the floor board
        
        float temp = 0.0f;
        foreach (GameObject noise in soundObjects)
        {
            temp = noise.GetComponent<Sound>().sound;
            if (temp > loudestSound)
            {
                loudestSound = temp;
                target = noise.transform;
            }
        }
        foreach(GameObject noise in players)
        {
            temp = noise.GetComponent<Sound>().sound;
            if (temp > loudestSound)
            {
                loudestSound = temp;
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
        // TODO: add code to move toward target
        //Finds if a new target is louder
        findTarget();

        //Increases speed if it has been chasing for a multiple of 2.5 seconds
        chaseTime += Time.deltaTime;
        if (chaseTime / 2.5f > 1.0f)
        {
            chaseTime -= 2.5f;
            movSpeed += 0.75f;
            if(movSpeed> 6.0f)
            {
                movSpeed = 6.0f;
            }
        }
        
        FollowSound();

        //Check if it is on top of the targets position
        if(Vector3.Distance(target.position,this.transform.position) < sensePlayerDistance) {
            loudestSound = 0.0f;
            target = null;
            EnterStateWander();
        }
        if (DetectPlayer()) { EnterStateAttack(); }
	}

    //makes it go towards sound
    void FollowSound()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotSpeed * Time.deltaTime);
        transform.position += transform.forward * (movSpeed * slowSpeed) * Time.deltaTime;
    }

    // ATTACK STATE ---------------------------------------------------------------------

	private void EnterStateAttack() {
		_currentState = STATE.ATTACK;
		// TODO: add code about setting up to chase player
	}

	private void UpdateAttack() {
        // TODO: add code about chasing player
        if (DetectPlayer() == true){
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotSpeed * Time.deltaTime);
            transform.position += transform.forward * (movSpeed * slowSpeed) * Time.deltaTime;
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
        yield return new WaitForSecondsRealtime(inputDelay);
        canMove = true;
        _currentState = STATE.WANDER;
        movSpeed = baseMoveSpeed;
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

    void respawn()
    {
        this.transform.position = spawn.transform.position;
    }
}
