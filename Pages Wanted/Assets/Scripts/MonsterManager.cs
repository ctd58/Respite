using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

    // Public or Serialized Variables for Inspector -----------------
    #region Public Variables
    public bool debug;
    public Room startRoom;
    [SerializeField] [Range(50f, 2000f)] private float sensePlayerDistance = 1000f;
    [SerializeField] [Range(1f, 100f)] private float baseMonsterSpeed = 50f;
    [SerializeField] [Range(0.0f, 1.0f)] private float fallOffStrength = 0.01f;
    #endregion

    // Private Variables ---------------------------------------------
    #region Private Variables
	private Monster monster;
    private List<Transform> wanderPoints = new List<Transform>();
    private List<Transform> patrolPoints = new List<Transform>();
	private List<Transform> spawnPoints = new List<Transform>();
    private List<GameObject> players = new List<GameObject>();
    private GameObject[] soundObjects;
    private float currentSpeed;
    private enum STATE {
        WANDER,
        //Wander - Move around the room in a random way
        INSPECT,
        //Wander - Move around the room in a random way
        ATTACK,
        //Wander - Move around the room in a random way
        IDLE,
        //Idle - Look/rotate around in the same spot of a room
        HIDE,
        //Hide - Become more transparent, stay still in one spot
        HUNT,
        //Hunt - Go aggressively after player, speeding up unless interrupted
        PATROL
        //Patrol - Go in a specified patrol sequence deemed by developers. 
    } 
    private STATE currentState;
    private Transform target = null;
    #endregion

	// Setup Methods -------------------------------------------------
    #region Setup Methods
	void Start () {
		monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<Monster>();
		wanderPoints = startRoom.GetWanderWaypoints();
        patrolPoints = startRoom.patrolRoute;
        spawnPoints = startRoom.spawnPoints;
        players.Add(GameObject.FindGameObjectWithTag("P1"));
        players.Add(GameObject.FindGameObjectWithTag("P2"));
        //TODO: make it search out objects with component<Sound>;
        soundObjects = GameObject.FindGameObjectsWithTag("MakesSound");
        // Start the monster wandering
        EnterStateWander();
        // Start coroutines
        StartCoroutine("findLoudestSound");
        StartCoroutine("DetectPlayer");
    }
    #endregion

    // Set Enter New State --------------------------------------------
    #region Set New State
    private void EnterStateWander() {
        currentState = STATE.WANDER;
        target = GetWanderWaypoint();
        SetMonsterTarget();
    }

    private void EnterStatePatrol() {
        currentState = STATE.PATROL;
        target = GetPatrolWaypoint();
        SetMonsterTarget();
    }

    private void EnterStateHunt() {
        currentState = STATE.HUNT;
        StartCoroutine("HuntPlayers");
    }
    #endregion

    // Set New Target -------------------------------------------------
    #region Set New Target
    public Transform GetNewTarget() {
        switch (currentState) {
            case STATE.WANDER: target = GetWanderWaypoint(); break;
            case STATE.PATROL: target = GetPatrolWaypoint(); break;
        }
        return target;
    }

    public void SetMonsterTarget() {
        monster.SetTarget(target);
    }

    private Transform GetWanderWaypoint() {
        return wanderPoints[Random.Range(0, wanderPoints.Count-1)];
    }

    private Transform GetPatrolWaypoint() {
        int nextIndex = patrolPoints.IndexOf(target) + 1;
        if (nextIndex == patrolPoints.Count) return patrolPoints[0];
        else return patrolPoints[nextIndex];
    }

    public Transform GetSpawnPoint() {
		if (spawnPoints.Count == 0) { 
			//if (debug) Debug.LogError("no spawn points set in current Room.");
			return this.transform;
		}
		//TODO: add intelligent code here that picks a spawn point the player isn't currently looking at
		return spawnPoints[0];
	}
    #endregion
	
	public void TriggerMonsterAction(MonsterAction action) {
		if (action.teleport) {
			SetRoom(action.teleportLocation);
			monster.Teleport(action.teleportLocation);
		}
		SetState(action.state);
	}

	public void SetState(MonsterState state) {
		switch(state) {
			case MonsterState.WANDER: EnterStateWander(); break;
            case MonsterState.PATROL: EnterStatePatrol(); break; 
            case MonsterState.HUNT: EnterStateHunt(); break;
		}
	}

	private void SetRoom(Transform target) {
		Room newRoom = target.GetComponentInParent<Room>();
        wanderPoints = newRoom.GetWanderWaypoints();
        patrolPoints = newRoom.patrolRoute;
		spawnPoints = newRoom.spawnPoints;
	}

    // Coroutines -------------------------------------------------
    #region Coroutines
    IEnumerator DetectPlayer() {
        while (true) {
            yield return new WaitForSeconds(0.1f);
            if (players != null) {
                float playerOneDistance = Vector3.Distance(players[0].gameObject.transform.position, this.transform.position);
                float playerTwoDistance = Vector3.Distance(players[1].gameObject.transform.position, this.transform.position);
                if (playerOneDistance < sensePlayerDistance) {
                    target = players[0].transform;
                    SetMonsterTarget();
                }
                else if (playerTwoDistance < sensePlayerDistance) {
                    target = players[1].transform;
                    SetMonsterTarget();
                }
            }
        }
    }

    //Determines what object is making the loudest noise and goes to it
    IEnumerator findLoudestSound() {
        while (true) {
            yield return new WaitForSeconds(0.25f);
            if (debug) Debug.Log("checking sound");
            //if (currentState != STATE.STUNNED) {
                float temp = 0.0f;
                float loudest = 0.0f;
                foreach (GameObject noise in soundObjects) {
                    float sound = noise.GetComponent<Sound>().sound;
                    temp = (sound > 0) ? GetSoundWithFallOff(noise) : 0f;
                    if (temp > loudest) {
                        loudest = temp;
                        target = noise.transform;
                        SetMonsterTarget();
                    }
                }
                foreach (GameObject noise in players) {
                    temp = GetSoundWithFallOff(noise);
                    if (temp > loudest) {
                        loudest = temp;
                        target = noise.transform;
                        SetMonsterTarget();
                    }
                }
            if (debug) Debug.Log("loudest sound: " + loudest);
            //}
            // If it didn't hear any noise, go back to wandering after getting ot the current target
            if (loudest == 0.0f) {
                currentState = STATE.WANDER;
            }
        }
    }

    private float GetSoundWithFallOff(GameObject noiseObj) {
        float sound = noiseObj.GetComponent<Sound>().sound;
        float distance = Vector3.Distance(noiseObj.transform.position, this.gameObject.transform.position);
        return (sound - (distance / 100 * fallOffStrength));
    }

    IEnumerator HuntPlayers() {
        while(true) {
            yield return new WaitForSeconds(0.25f);
            if (players != null) {
                float playerOneDistance = Vector3.Distance(players[0].gameObject.transform.position, this.transform.position);
                float playerTwoDistance = Vector3.Distance(players[1].gameObject.transform.position, this.transform.position);
                if (playerOneDistance < playerTwoDistance) {
                    target = players[0].transform;
                    SetMonsterTarget();
                }
                else {
                    target = players[1].transform;
                    SetMonsterTarget();
                }
            }
        }
    }
    #endregion

    // Slows the monster down or speeds it up smoothly 
    // newSpeed = the new speed we want the monster to be moving 
    // seconds  = the number of seconds the monster will take to reach that new speed 
    public IEnumerator ChangeSpeed(float newSpeed, float seconds) {
        float speedIncrement = (currentSpeed-newSpeed)/(seconds/0.1f);
        for (float i = 0; i < seconds; i += 0.1f) {
            yield return new WaitForSeconds(0.1f);
            currentSpeed = currentSpeed -= speedIncrement;
            monster.SetSpeed(currentSpeed);
        }
    }

    // Reverts the monster to its base speed smoothly 
    // seconds  = the number of seconds the monster will take to reach baseSpeed 
    public IEnumerator ReturnToBaseSpeed(float seconds) {
        float speedIncrement = (currentSpeed-baseMonsterSpeed)/(seconds/0.1f);
        for (float i = 0; i < seconds; i += 0.1f) {
            yield return new WaitForSeconds(0.1f);
            currentSpeed = currentSpeed -= speedIncrement;
            monster.SetSpeed(currentSpeed);            
        }
    }
}

public enum MonsterState {
    //Current - Stay in it's original state
	CURRENT,
    //Wander - Move around the room in some fashion
	WANDER,
    //Idle - Look/rotate around in the same spot of a room
	IDLE,
    //Hide - Become more transparent, stay still in one spot
	HIDE,
    //Hunt - Go aggressively after player, speeding up unless interrupted
	HUNT,
    //Track - Track only a sound that a player has made (workshop 2)
	//TRACK,
    //Patrol - Go in a specified patrol sequence deemed by developers. 
	PATROL
}