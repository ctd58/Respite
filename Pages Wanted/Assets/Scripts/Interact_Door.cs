using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interact_Door : Interact_MakeNoise {
	[Header("Door Parameters")]
	//[Help("This script should go on every door object to allow the door to unlock.", UnityEditor.MessageType.None)]
	//TODO: fix this plugin

	// Public or Serialized Variables for Inspector -----------------
	#region Public Variables
	public List<KeyTypes> keysNeeded;
	#endregion

	// Private Variables ---------------------------------------------
	#region Private Variables
	private bool doorLocked = true;
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
	private List<Lock_Obj> locks;
	#endregion

	/* Note: apparently "lock" is a variable name reserved by unity.
	 * Using it in code will cause some really weird errors. */

	// Setup Methods -------------------------------------------------
	#region Setup Methods
	new void Start()
	{
		base.Start();
		SetSprite(Interact_Icon_Type.UNLOCK);
		// Populate private list of locks from public keysNeeded
		locks = new List<Lock_Obj>();
		foreach ( KeyTypes key in keysNeeded) {
			Lock_Obj newLock = new Lock_Obj();
			newLock.lockType = key;
			locks.Add(newLock);
		}
	}
	#endregion

	// Public Methods -------------------------------------------------
	#region Public Methods
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
	#endregion

	// Private Methods -------------------------------------------------
	#region Private Methods
	// Triggered by player interacting with door, checks players keys to unlock locks
	private void UnlockLocks(List<Key_Obj> playerKeys) {
		foreach( Lock_Obj l in locks) {
			if (l.locked) {
				// Get all matching keys
				List<Key_Obj> keysOfRightType;
				keysOfRightType = playerKeys.FindAll(
					key => key.type.Equals(l.lockType)
				);
				// Check and make sure key wasn't used already at this door
				if (keysOfRightType.Count > 0) {
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
					if (!matchExists) {
						Key_Obj matchKey = keysOfRightType[i];
						l.locked = false;
						l.keyUsedId = matchKey.keyId;
					}
				}
			}
		}
		// see if all the locks on the door are now open
		doorLocked = locks.Exists(l => l.locked.Equals(true));
	}

	// All locks have been unlocked, now open the door
	private void OpenDoor() {
		PlayAnimation();
		Destroy(this.GetComponent<Collider>());
		StartCoroutine("MakeNoise");
	}

	protected override void PlayAnimation() {
		this.GetComponent<Animator>().SetTrigger("openDoor");
	}
	#endregion
}

