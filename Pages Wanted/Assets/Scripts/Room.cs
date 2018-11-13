using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public List<MonsterAction> actions;
	public List<Transform> patrolRoute;
	public List<Transform> spawnPoints;

	private Transform[] waypoints;

	// Use this for initialization
	void Awake () {
		waypoints = GetComponentsInChildren<Transform>(false);
		SetUpActions();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<Transform> GetWanderWaypoints() { 
		List<Transform> listPoints = new List<Transform>();
		//Debug.Log("WAYPOINTS" + waypoints.Length);
		foreach(Transform point in waypoints) {
			listPoints.Add(point);
		}
		return listPoints;
	}

	private void SetUpActions() {
		foreach (MonsterAction action in actions) {
			action.SetRoom(this.GetComponent<Room>());
			foreach(Interactables obj in action.triggerObjs) {
				obj.SetMonsterAction(action);
			}
			// TODO: add more types script that could have actions
		}
	}

}
