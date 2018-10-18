using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {

    public Transform target = null;
    public float baseMoveSpeed = 4.0f;
    public float rotSpeed, movSpeed;
    public float distance;
    public float sensePlayerDistance;
    public float loudestSound = 0.0f;
    public GameObject[] soundObjects;
    public List<GameObject> players;
    public float chaseTime = 0.0f;
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

    private enum STATE { WANDER, INSPECT, ATTACK, STUNNED }
    private STATE _currentState;

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        spawn = GameObject.FindGameObjectWithTag("DemonSpawn");
        // gameoverScreen.SetActive(false); 
        soundObjects = GameObject.FindGameObjectsWithTag("MakesSound");
        players.Add(GameObject.FindGameObjectWithTag("P1"));
        players.Add(GameObject.FindGameObjectWithTag("P2"));
        EnterStateWander ();
    }

    void Update () {
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

    private void EnterStateWander() {
        movSpeed = baseMoveSpeed; 
		_currentState = STATE.WANDER;
        // TODO: add code to setup wandering
        currentWaypoint = wayPoints[i];
        madeWaypoint = true; 
	}

	private void UpdateWander() {
        // TODO: add code for wandering here
        DetectPlayer();
        findTarget();
        counter++;
        if (counter > 400)
        {
            counter = 0;
            madeWaypoint = true; 
        }
        if (target != null)
        {
            EnterStateInspect(target); 
        }
        else
        {
            if (currentWaypoint != null && madeWaypoint == true)
            {
                Vector3 targetV = currentWaypoint.transform.position;
                navMeshAgent.SetDestination(targetV);
                madeWaypoint = false;
                i = (i + 1) % wayPoints.Count;
                Debug.Log(i); 
            }
            currentWaypoint = wayPoints[i]; 
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
        if(Vector3.Distance(target.position,this.transform.position) < sensePlayerDistance)
        {
            loudestSound = 0.0f;
            DetectPlayer();
            target = null;
            EnterStateWander();
        }
	}

	private void EnterStateAttack() {
		_currentState = STATE.ATTACK;
		// TODO: add code about setting up to chase player
	}

	private void UpdateAttack() {
        // TODO: add code about chasing player
	}

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

    //makes it go towards sound
    void FollowSound()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotSpeed * Time.deltaTime);
        transform.position += transform.forward * (movSpeed * slowSpeed) * Time.deltaTime;
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
            gameoverScreen.SetActive(true);
        }
    }

    void respawn()
    {
        this.transform.position = spawn.transform.position;
    }

    IEnumerator stun()
    {
        Debug.Log(Time.time);
        yield return new WaitForSecondsRealtime(inputDelay);
        canMove = true;
        _currentState = STATE.WANDER;
        movSpeed = baseMoveSpeed;
        Debug.Log(Time.time);
    }

    private void DetectPlayer() {
        if (Vector3.Distance( players[0].gameObject.transform.position, this.transform.position) < sensePlayerDistance ) { 
            target = players[0].transform;
            EnterStateAttack();
        } else if  (Vector3.Distance( players[1].gameObject.transform.position, this.transform.position) < sensePlayerDistance ) {
            target = players[1].transform;
            EnterStateAttack();
        }
    }
}
