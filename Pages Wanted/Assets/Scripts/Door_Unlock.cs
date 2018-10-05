using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Unlock : MonoBehaviour {

	/* Note: apparently "lock" is a variable name reserved by unity.
	 * Using it in code will cause some really weird errors. */

	private class Lock_Obj {
		public bool locked;
		public KeyTypes lockType;
		public int keyUsedId;

		public Lock_Obj() {
			locked = true;
			keyUsedId = 0;
		}
	}

	public List<KeyTypes> keysNeeded;
	private List<Lock_Obj> locks;

	void Start()
	{
		// Populate private list of locks from public keysNeeded
		locks = new List<Lock_Obj>();
		foreach ( KeyTypes key in keysNeeded) {
			Lock_Obj newLock = new Lock_Obj();
			newLock.lockType = key;
			locks.Add(newLock);
		}
	}

	// Triggered by player interacting with door, checks players keys to unlock locks
	public void UnlockLocks(List<Key_Obj> playerKeys) {
		foreach( Lock_Obj l in locks) {
			if (l.locked) {
				// Get all matching keys
				List<Key_Obj> keysOfRightType;
				keysOfRightType = playerKeys.FindAll(
					key => key.type.Equals(l.lockType)
				);
				// Check and make sure key wasn't used already at this door
				bool matchExists = true;
				int i = 0;
				do {
					matchExists = locks.Exists( 
						m => m.locked.Equals(false) && 
							m.keyUsedId.Equals(keysOfRightType[i].keyId)
					);
					if (matchExists) { i++; }
				} while ( i < keysOfRightType.Count && matchExists.Equals(true));
				// If found a key of the right type that hasn't already been used
				if (keysOfRightType.Count > 0 && !matchExists) {
					Key_Obj matchKey = keysOfRightType[i];
					l.locked = false;
					l.keyUsedId = matchKey.keyId;
				}
			}
		}
		bool stillLocked = locks.Exists(l => l.locked.Equals(true));
		if (!stillLocked) {
			UnlockDoor();
		}
	}

	// All locks have been unlocked, now open the door
	public void UnlockDoor() {
		//TODO: This is where an unlock animation would go, but as a filler
		Destroy(this.gameObject);
	}
}

