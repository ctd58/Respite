using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

	public Room startRoom;
	private Room currentRoom;
	private Monster monster;

	// Use this for initialization
	void Start () {
		monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<Monster>();
		currentRoom = startRoom;
		monster.TriggerWander(startRoom.GetWanderWaypoints());
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

	private void SetRoom(Transform target) {
		currentRoom = target.GetComponentInParent<Room>();
	}
}

public enum MonsterState {
	CURRENT,
	WANDER,
	IDLE,
	HIDE,
	HUNT,
	TRACK,
	PATROL
}