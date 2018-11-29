using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour {

    private string[] dialogue;
    private string[] objectives;

    private static bool journalon; 
    private string button;
    public GameObject canvas; 
    private GameObject journal;
    private GameObject player;
    private JournalManager jm; 



	// Use this for initialization
	void Start () {
		if (player.tag == "P1") {
            button = "P1lbrb";
            //journal = canvas.GetComponent<P1_Screen>().GetComponent<Journal>(); 
        }
        if (player.tag == "P2") {
            button = "P2lbrb";
            //journal = canvas.GetComponent<P2_Screen>().GetComponent<Journal>();
        }
        dialogue = jm.GetDialogue();
        objectives = jm.GetObjective(); 
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown(button)) {
            ToggleJournal(); 
        }
	}

    void ToggleJournal() {
        if (journalon) {
            journalon = false;
            journal.SetActive(false); 
        }
        else {
            journalon = true;
            journal.SetActive(true); 
        }
    }
    

}
