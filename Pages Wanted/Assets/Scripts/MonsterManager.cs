using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

    // Public or Serialized Variables for Inspector -----------------
    #region Public Variables
    [Header("General Variables")]
    public bool debug;
    public Room startRoom;

    [Header("Wander State Variables")]
    [SerializeField] [Range(1f, 100f)] private float wanderSpeed = 50f;
    [SerializeField] [Range(0.1f, 20f)] private float secondsToWanderSpeed = 2f;
    [SerializeField] private Color wanderParticleColor;

    [Header("Inspect State Variables")]
    // TODO: figure out how we want to increase inspect speed
    // [SerializeField] [Range(1f, 100f)] private float inspectSpeed = 50f;
    // [SerializeField] [Range(0.1f, 20f)] private float secondsToInspectSpeed = 2f;
    [SerializeField] private Color inspectParticleColor;

    [Header("Attack State Variables")]
    [SerializeField] [Range(100, 1000)] private float attackDistance = 300f;
    // TODO: figure out how we want to increase attack speed
    // [SerializeField] [Range(1f, 100f)] private float attackSpeed = 60f;
    // [SerializeField] [Range(0.1f, 20f)] private float secondsToAttackSpeed = 5f;
    [SerializeField] private Color attackParticleColor;

    [Header("Patrol State Variables")]
    [SerializeField] [Range(1f, 100f)] private float patrolSpeed = 50f;
    [SerializeField] [Range(2.0f, 30.0f)] private float patrolTime = 15f;
    [SerializeField] [Range(0.1f, 20f)] private float secondsToPatrolSpeed = 2f;
    [SerializeField] private Color patrolParticleColor;

    [Header("Hunt State Variables")]
    [SerializeField] [Range(1f, 100f)] private float huntSpeed = 50f;
    [SerializeField] [Range(2.0f, 30.0f)] private float huntTime = 15f;
    [SerializeField] [Range(0.1f, 20f)] private float secondsToHuntSpeed = 2f;
    [SerializeField] private Color huntParticleColor;

    [Header("Idle State Variables")]
    [SerializeField] [Range(2.0f, 30.0f)] private float idleTime = 15f;
    [SerializeField] private Color idleParticleColor;

    [Header("Hide State Variables")]
    [SerializeField] [Range(2.0f, 30.0f)] private float hideTime = 15f;
     [SerializeField] private Color hideParticleColor;
   #endregion

    // Private Variables ---------------------------------------------
    #region Private Variables
	private Monster monster;
    private List<Transform> wanderPoints = new List<Transform>();
    private List<Transform> patrolPoints = new List<Transform>();
	private List<Transform> spawnPoints = new List<Transform>();
    private List<GameObject> players = new List<GameObject>();
    private GameObject[] soundObjects;
    private float fallOffStrength = 0.01f;
    private float currentSpeed;
    private enum STATE {
        WANDER,
        //Wander - Move around the room in a random way
        INSPECT,
        //Inspect - Move to an object that made noise
        ATTACK,
        //Attack - Move to the players if they are within the playerSenseDistance
        IDLE,
        //Idle - Look/rotate around in the same spot of a room
        HIDE,
        //Hide - Become more transparent, stay still in one spot
        HUNT,
        //Hunt - Go aggressively after player, speeding up unless interrupted
        PATROL,
        //Patrol - Go in a specified patrol sequence deemed by developers. 
        STUN
        //Stun - caused by player ability, stops monster from moving, attacking or inspecting
    } 
    private STATE currentState;
    private Transform target = null;
    private bool canLeaveState = true;
    private float canNotSwitchStatesTime = 2;
    #endregion

	// Setup Methods -------------------------------------------------
    #region Setup Methods
	void Start () {
        // Get Reference to Monster
		monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<Monster>();
        monster.SetSpeed(wanderSpeed);
        // Setup Starting Room
		wanderPoints = startRoom.GetWanderWaypoints();
        patrolPoints = startRoom.patrolRoute;
        spawnPoints = startRoom.spawnPoints;
        // Setup Reference to Players
        players.Add(GameObject.FindGameObjectWithTag("P1"));
        players.Add(GameObject.FindGameObjectWithTag("P2"));
        // Setup Reference to Sound Objects
        //TODO: make it search out objects with component<Sound>;
        soundObjects = GameObject.FindGameObjectsWithTag("MakesSound");
        // Start the monster wandering
        EnterStateWander();
        // Start coroutines to check for reaction states (inspect and attack)
        StartCoroutine("CheckForAttack");
        StartCoroutine("CheckForInspect");
    }
    #endregion


    public Transform GetNewTarget() {
        switch (currentState) {
            case STATE.PATROL: target = GetPatrolWaypoint(); break;
            case STATE.WANDER: target = GetWanderWaypoint(); break;
            // if any other State, go back to wandering
            default: EnterStateWander(); break;
        }
        return target;
    }

    public void SetMonsterTarget() {
        monster.SetTarget(target);
    }

	// Wander State ------------------------------------------------------
    // Wandering is the base state of the monster, the state that it
    //   will return from all other states if their end condition is
    //   met. Wander is also the starting state of the monster.
    #region Wander Methods
    private void EnterStateWander() {
        currentState = STATE.WANDER;
        target = GetWanderWaypoint();
        StartCoroutine(ReturnToBaseSpeed(secondsToWanderSpeed));
        monster.ChangeParticleColor(wanderParticleColor);
        SetMonsterTarget();
    }

    private Transform GetWanderWaypoint() {
        return wanderPoints[Random.Range(0, wanderPoints.Count-1)];
    }
    #endregion

    // Reaction States -------------------------------------------------
    // Inspect and Attack are Reaction states, meaning that the
    //    monster will enter them only in response to a specific
    //    condition being met.
    #region Reaction States
    // Attack State -------------------------
    // Monster will move toward players until it either reaches
    //    them and then respawns, or until they players move 
    //    out of range, which will cause it to move to where it
    //    last saw them, then go back to wandering.
    IEnumerator CheckForAttack() {
        while (true) {
            yield return new WaitForSeconds(0.1f);
            if (canLeaveState && currentState != STATE.HUNT) {
                float playerOneDistance = Vector3.Distance(players[0].gameObject.transform.position, monster.transform.position);
                float playerTwoDistance = Vector3.Distance(players[1].gameObject.transform.position, monster.transform.position);
                if (debug) Debug.Log("Distance to P1: " + playerOneDistance);
                if (debug) Debug.Log("Distance to P2: " + playerTwoDistance);
                if (playerOneDistance < attackDistance) {
                    target = players[0].transform;
                    SetMonsterTarget();
                }
                else if (playerTwoDistance < attackDistance) {
                    target = players[1].transform;
                    SetMonsterTarget();
                }
            }
        }
    }

    // Inspect State -------------------------
    // Monster will move toward an object that makes noise,
    //    and upon reaching it, will go back to wandering.
    IEnumerator CheckForInspect() {
        while (true) {
            yield return new WaitForSeconds(0.25f);
            if (debug) Debug.Log("checking sound");
            if (canLeaveState) {
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
            if (debug) Debug.Log("target: " + target.gameObject.name);
            }
        }
    }

    private float GetSoundWithFallOff(GameObject noiseObj) {
        float sound = noiseObj.GetComponent<Sound>().sound;
        float distance = Vector3.Distance(noiseObj.transform.position, this.gameObject.transform.position);
        return (sound - (distance / 100 * fallOffStrength));
    }
    #endregion

    // Triggered States -------------------------------------------------
    // Hunt, Hide, Idle and Patrol are Triggered States, meaning
    //    that the monster will only enter them upon an action
    //    being triggered.
    #region Triggered States
    // Called by interactables triggering a monster action
	public void TriggerMonsterAction(MonsterAction action) {
		if (action.teleport) {
			SetRoom(action.teleportLocation);
			monster.Teleport(action.teleportLocation);
		}
		SetState(action.state);
	}

    // If the interactables cause the monster to teleport to 
    //    a new waypoint, set that waypoint's parent room as
    //    the current room.
    private void SetRoom(Transform target) {
		Room newRoom = target.GetComponentInParent<Room>();
        wanderPoints = newRoom.GetWanderWaypoints();
        patrolPoints = newRoom.patrolRoute;
		spawnPoints = newRoom.spawnPoints;
	}

    // If the interactables cause a state change, do that
	public void SetState(MonsterState state) {
		switch(state) {
			case MonsterState.WANDER: EnterStateWander(); break;
            case MonsterState.PATROL: EnterStatePatrol(); break; 
            case MonsterState.HUNT: EnterStateHunt(); break;
            case MonsterState.IDLE: EnterStateIdle(); break;
            case MonsterState.HIDE: EnterStateHide(); break;
		}
	}

    // Patrol State -------------------------
    // Monster will move to the patrol points set for a
    //    room, in sequence, until patrolTime is up, or
    //    until Attack or Inspect state is entered
    private void EnterStatePatrol() {
        currentState = STATE.PATROL;
        target = GetPatrolWaypoint();
        SetMonsterTarget();
        StartCoroutine(StopStateChangeForSeconds(canNotSwitchStatesTime));
        StartCoroutine(ChangeSpeed(patrolSpeed, secondsToPatrolSpeed));
        monster.ChangeParticleColor(patrolParticleColor);
        StartCoroutine(RevertToWandering(patrolTime));
    }
    
    private Transform GetPatrolWaypoint() {
        int nextIndex = patrolPoints.IndexOf(target) + 1;
        if (nextIndex == patrolPoints.Count) return patrolPoints[0];
        else return patrolPoints[nextIndex];
    }

    // Hunt State -------------------------
    // Monster will move toward the players no matter
    //    where they are, until it hits them and respawns,
    //    huntTime is up, or the Inspect state is entered
    private void EnterStateHunt() {
        currentState = STATE.HUNT;
        StartCoroutine("HuntPlayers");
        StartCoroutine(StopStateChangeForSeconds(canNotSwitchStatesTime));
        StartCoroutine(ChangeSpeed(huntSpeed, secondsToHuntSpeed));
        monster.ChangeParticleColor(huntParticleColor);
        StartCoroutine(RevertToWandering(huntTime, "HuntPlayers"));
    }

    IEnumerator HuntPlayers() {
        while(true) {
            yield return new WaitForSeconds(0.25f);
            if (players != null) {
                float playerOneDistance = Vector3.Distance(players[0].gameObject.transform.position, monster.transform.position);
                float playerTwoDistance = Vector3.Distance(players[1].gameObject.transform.position, monster.transform.position);
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

    // Idle State -------------------------
    // Monster will stand still and look around 
    //    until idleTime is up, or until Attack 
    //    or Inspect state is entered
    private void EnterStateIdle() {
        currentState = STATE.IDLE;
        StartCoroutine("IdleLook");
        StartCoroutine(StopStateChangeForSeconds(canNotSwitchStatesTime));
        StartCoroutine(ChangeSpeed(0, 1f));
        monster.ChangeParticleColor(idleParticleColor);
        StartCoroutine(RevertToWandering(idleTime, "IdleLook"));
    }

    private IEnumerator IdleLook() {
        while(true) {
            Vector3 newLookDir = new Vector3(Random.Range(-1000f, 1000f), monster.transform.position.y, (Random.Range(-1000f, 1000f)));
            monster.LookAt(newLookDir);
            yield return new WaitForSeconds(Random.Range(2,10));
        }
    }

    // Hide State -------------------------
    // Monster will stand still until hideTime is up, 
    //    or until Attack or Inspect state is entered
    private void EnterStateHide() {
        currentState = STATE.IDLE;
        StartCoroutine(ChangeSpeed(0, 0.1f));
        monster.ChangeParticleColor(hideParticleColor);
        StartCoroutine(StopStateChangeForSeconds(canNotSwitchStatesTime));
        StartCoroutine(RevertToWandering(hideTime));
    }

    // Returns the monster to wandering once triggered state has
    //    reached set time for stopping
    IEnumerator RevertToWandering(float seconds) {
        yield return new WaitForSeconds(seconds);
        EnterStateWander();
    }

    IEnumerator RevertToWandering(float seconds, string coroutine) {
        yield return new WaitForSeconds(seconds);
        EnterStateWander();
        StopCoroutine(coroutine);
    }
    #endregion

    #region Player-Caused States
    // Stun State -------------------------
    // Monster will stand still and look around 
    //    until stunTime is up. Will not Attack
    //    or Inspect during this time
    public void EnterStateStun(float stunTime) {
        currentState = STATE.STUN;
        StartCoroutine(ChangeSpeed(0, 1f));
        StartCoroutine(RevertToWandering(stunTime));
        StartCoroutine(StopStateChangeForSeconds(stunTime));
    }
    #endregion

    public Transform GetSpawnPoint() {
        // if no spawn points set at current room, return monster to startRoom
        // this is a temporary fix
		if (spawnPoints.Count == 0) { 
			if (debug) Debug.LogError("no spawn points set in current Room.");
			return startRoom.GetWanderWaypoints()[0];
		}
		//TODO (WS2) add intelligent code here that picks a spawn point the player isn't currently looking at
		return spawnPoints[0];
	}

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
        float speedIncrement = (currentSpeed-wanderSpeed)/(seconds/0.1f);
        for (float i = 0; i < seconds; i += 0.1f) {
            yield return new WaitForSeconds(0.1f);
            currentSpeed = currentSpeed -= speedIncrement;
            monster.SetSpeed(currentSpeed);            
        }
    }

    private IEnumerator StopStateChangeForSeconds(float seconds) {
        canLeaveState = false;
        yield return new WaitForSeconds(seconds);
        canLeaveState = true;
    }

    //TODO: add methods for turning off and on the danger particles
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