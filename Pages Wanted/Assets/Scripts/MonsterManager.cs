using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

    private Transform target = null;
    [SerializeField] [Range(0.0f, 1.0f)] private float fallOffStrength = 0.01f;
    [SerializeField] [Range(50F, 1000F)] private float sensePlayerDistance = 100f;
    public Room startRoom;
	private Room currentRoom;
	private Monster monster;
	private List<Transform> spawnPoints = new List<Transform>();
    private List<GameObject> players = new List<GameObject>();
    private GameObject[] soundObjects;

	// Use this for initialization
	void Start () {
		currentRoom = startRoom;
		spawnPoints = currentRoom.spawnPoints;
		monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<Monster>();
		monster.TriggerWander(startRoom.GetWanderWaypoints());
        players.Add(GameObject.FindGameObjectWithTag("P1"));
        players.Add(GameObject.FindGameObjectWithTag("P2"));
        StartCoroutine("findLoudestSound");
        soundObjects = GameObject.FindGameObjectsWithTag("MakesSound");
        sensePlayerDistance = 200;
    }
	
	// Update is called once per frame
	void Update () {
	}

	public void TriggerMonsterAction(MonsterAction action) {
		if (action.teleport) {
			SetRoom(action.teleportLocation);
			monster.Teleport(action.teleportLocation);
		}
		SetState(action.state);
	}

	public void SetState(MonsterState state) {
		switch(state) {
			case MonsterState.WANDER:
				monster.TriggerWander(currentRoom.GetWanderWaypoints());
				break;
		}
	}

	public Transform GetSpawnPoint() {
		if (spawnPoints.Count == 0) { 
			Debug.LogError("no spawn points set in current Room.");
			return this.transform;
		}
		//TODO: add intelligent code here that picks a spawn point the player isn't currently looking at
		return spawnPoints[0];
	}

	private void SetRoom(Transform target) {
		currentRoom = target.GetComponentInParent<Room>();
		spawnPoints = currentRoom.spawnPoints;
	}


    public bool DetectPlayer() {
        if (players != null) {
            float playerOneDistance = Vector3.Distance(players[0].gameObject.transform.position, this.transform.position);
            float playerTwoDistance = Vector3.Distance(players[1].gameObject.transform.position, this.transform.position);
            if (playerOneDistance < sensePlayerDistance) {
                target = players[0].transform;
                return true;
            }
            else if (playerTwoDistance < sensePlayerDistance && playerTwoDistance > playerOneDistance) {
                target = players[1].transform;
                return true;
            }
        }
        return false;
    }

    public Transform getTarget() {
        return target; 
    }
    private float GetSoundWithFallOff(GameObject noiseObj) {
        float sound = noiseObj.GetComponent<Sound>().sound;
        float distance = Vector3.Distance(noiseObj.transform.position, this.gameObject.transform.position);
        return (sound - (distance / 100 * fallOffStrength));
    }

    //Determines what object is making the loudest noise and goes to it
    IEnumerator findLoudestSound() {
        while (true) {
            yield return new WaitForSeconds(1f);
            Debug.Log("checking sound");
            if (monster.GetState() != Monster.STATE.STUNNED) {
                float temp = 0.0f;
                float loudest = 0.0f;
                foreach (GameObject noise in soundObjects) {
                    float sound = noise.GetComponent<Sound>().sound;
                    temp = (sound > 0) ? GetSoundWithFallOff(noise) : 0f;
                    if (temp > loudest) {
                        loudest = temp;
                        target = noise.transform;
                        monster.TriggerInspect();
                    }
                }
                foreach (GameObject noise in players) {
                    temp = GetSoundWithFallOff(noise);
                    if (temp > loudest) {
                        loudest = temp;
                        target = noise.transform;
                        monster.TriggerInspect();
                    }
                }
            }
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