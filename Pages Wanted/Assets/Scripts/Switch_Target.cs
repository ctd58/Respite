using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Target : MonoBehaviour {
	// Public or Serialized Variables for Inspector -----------------
	#region Public Variables
	public bool activateOnTrigger;
	public bool animateOnTrigger;
    public MeshRenderer mesh;
	#endregion
	
	// Private Variables ---------------------------------------------
	#region Private Variables
	private Animator animator;
	#endregion

	void Start() {
		animator = GetComponent<Animator>();
	}

	public void onSwitchActivate () {
		if (animateOnTrigger) animator.SetTrigger("open");
        if (activateOnTrigger)
        {
            this.gameObject.SetActive(true);
            mesh.enabled = true;
        }

	}

	public void onSwitchDeactivate() {
		if (animateOnTrigger) animator.SetTrigger("close");
		if (activateOnTrigger) this.gameObject.SetActive(false);	
	}
}