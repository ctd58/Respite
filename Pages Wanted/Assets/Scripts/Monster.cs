using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private enum STATE { WANDER, INSPECT, ATTACK, STUNNED }
    private STATE _currentState;

    void Start()
    {
        spawn = GameObject.FindGameObjectWithTag("DemonSpawn");
<<<<<<< HEAD
        sounds.Add(GameObject.FindGameObjectWithTag("P1"));
        sounds.Add(GameObject.FindGameObjectWithTag("P2"));
<<<<<<< HEAD
        gameoverScreen.SetActive(false); 
    }

    void Update()
    {
        //Widchard use this to call game over screen
        if(playerLives == 0)
        {
            Debug.Log("Game Over");
            gameoverScreen.SetActive(true);
        }
=======
=======
        soundObjects = GameObject.FindGameObjectsWithTags("MakesSound");
        players.Add(GameObject.FindGameObjectWithTag("P1"));
        players.Add(GameObject.FindGameObjectWithTag("P2"));
>>>>>>> master
        EnterStateWander ();
    }

    // void Update()
    // {
    //     //Widchard use this to call game over screen
    //     if(playerLives == 0)
    //     {
    //         Debug.Log("Game Over");
    //     }

    //     findTarget();
    //     //target = GameObject.FindGameObjectWithTag("P1").transform;
    //     //if (Vector3.Distance(target.position, gameObject.transform.position) <= maxDistance)
    //     if (target != null && canMove)
    //     {
    //         FollowSound();
    //         chaseTime += Time.deltaTime;
    //     }
    //     else if(canMove)
    //     {
    //         chaseTime = 0.0f;
    //     }
    //     else if(!canMove)
    //     {
    //         Debug.Log("STUNNED");
    //         StartCoroutine(stun());
    //     }
    // }

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
	}

    private void EnterStateWander() {
		_currentState = STATE.WANDER;
		// TODO: add code to setup wandering
	}

	private void UpdateWander() {
		// TODO: add code for wandering here
	}

	private void EnterStateInspect(GameObject target) {
		_currentState = STATE.INSPECT;
		//_currentTarget = target;
        // TODO: add code to set target positition

	}

	private void UpdateInspect() {
        // TODO: add code to move toward target

        FollowSound();
        if(Vector3.distance(target.position,this.transform.position)==0)
        {

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
>>>>>>> master


    //makes it go towards sound
    void FollowSound()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotSpeed * Time.deltaTime);
        transform.position += transform.forward * (movSpeed * slowSpeed) * Time.deltaTime;
    }

    //Determines what object is making the loudest noise and goes to it
    void findTarget()
    {
        //every 2.5 secs the monster is chasing it gets faster by .75
        if (chaseTime / 2.5f > 1.0f)
        {
            chaseTime -= 2.5f;
            movSpeed += 0.75f;
        }
        //If not chasing rest speed
        else if (chaseTime == 0.0f)
        {
            movSpeed = baseMoveSpeed;
        }

        //Determines what sound is the loudest and sets it as a target
        //PLEASE try to get the AI to remember the location (waypoint) of where the last sound came from and go to that
        //I cant get it to remember it forgets once the player moves off of the floor board
        
        
        float temp = 0.0f;
        foreach (GameObject noise in sounds)
        {
            temp = noise.GetComponent<Sound>().sound;
            if (temp > highest)
            {
                highest = temp;
                target = noise.transform;
            }
        }
        
        if(target != null)
        {
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("I gotcha boi");
        if(other.gameObject.tag == "P1" || other.gameObject.tag == "P2")
        {
            playerLives -= 1;
            respawn();
            hideHealth(); 
        }
    }

    private void hideHealth()
    {
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
        Debug.Log(Time.time);
    }

    private void DetectPlayer() {
        
    }
}
