using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Unlock : Interactables {

	/* Note: apparently "lock" is a variable name reserved by unity.
	 * Using it in code will cause some really weird errors. */

	/* Lock_Obj is a class used only in this script to organize info about locks
	 *  - bool locked       - is lock still locked
	 *  - KeyTypes lockType - is the lock BRONZE, SILVER, GOLD, etc
	 *  - int keyUsedId     - the id of the key used to unlock
	 */
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
	private bool doorLocked = true;

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

	public override void onInteract(Character_Inventory inv) {
		if (doorLocked) {
			List<Key_Obj> playerKeys = inv.getKeys();
			if (playerKeys.Count > 0) {
				UnlockLocks(playerKeys);
			}
		} else {
			OpenDoor();
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
		// see if all the locks on the door are now open
		doorLocked = locks.Exists(l => l.locked.Equals(true));
	}

	// All locks have been unlocked, now open the door
	public void OpenDoor() {
		//TODO: This is where an unlock animation would go, but as a filler
		this.GetComponent<Sound>().sound = 1;
        this.GetComponent<AudioSource>().Play();
		Destroy(this.gameObject, 3F);
	}
}

