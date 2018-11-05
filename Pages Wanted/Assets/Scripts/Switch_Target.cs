using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Target : MonoBehaviour {

	private Animator animator;

	void Start() {
		animator = GetComponent<Animator>();
	}

	public void onSwitchActivate () {
		animator.SetTrigger("open");
	}

	public void onSwitchDeactivate() {
		animator.SetTrigger("close");
	}
}
