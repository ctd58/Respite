using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSound : MonoBehaviour {

    public Sound fSound;

	// Use this for initialization
	void Start () {
        fSound = this.GetComponent<Sound>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "P1")
        {
            Debug.Log("P1 here, i entered");
            fSound.sound = 1;
            this.GetComponent<AudioSource>().Play();
        }
        if (other.gameObject.tag == "P2")
        {
            Debug.Log("P2 here, i entered");
            fSound.sound = 1;
            this.GetComponent<AudioSource>().Play();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "P1")
        {
            Debug.Log("P1 here, im leaving");
            fSound.sound = 0;
        }

        if (other.gameObject.tag == "P2")
        {
            Debug.Log("P2 here, im leaving");
            fSound.sound = 0;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
