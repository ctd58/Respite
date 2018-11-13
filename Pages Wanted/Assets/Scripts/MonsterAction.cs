using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAction {
	public List<Interactables> triggerObjs;
	public MonsterState state;
	public bool teleport;
	public Transform teleportLocation;
}
