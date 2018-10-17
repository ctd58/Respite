using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_MakeNoiseNow : Interact_MakeNoise {
	
	public override void onInteract(Character_Inventory inv) {
		PlayAnimation();
		MakeNoise();
	}

	public void PlayAnimation() {
		//TODO: stuff here
	}
}
