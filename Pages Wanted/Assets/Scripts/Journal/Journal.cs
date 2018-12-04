using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour {

    private string[] dialogue;
    private string[] objectives;

    public TextAsset textfile;
    private static bool journalon = false; 
    //private bool journalUpdated = true; 
    private string button;
    private KeyCode key; 
    private GameObject journal; 
    public GameObject player; 
    private ControllerMovement pmove; 
    private Text dialogueBox; 
    private Text objectiveBox;  
    private JournalManager jm; 



	// Use this for initialization
	void Start () {
        if (player.tag == "P1") {
            pmove = GameObject.Find("Player1").GetComponent<ControllerMovement>();
            button = "P1lbrb";
            key = KeyCode.J; 
            journal = GameObject.Find("JournalP1");
            dialogueBox = GameObject.Find("DialogueTextboxP1").GetComponent<Text>();
            objectiveBox = GameObject.Find("ObjectiveTextboxP1").GetComponent<Text>();
        }
        if (player.tag == "P2") {
            pmove = GameObject.Find("Player2").GetComponent<ControllerMovement>();
            button = "P2lbrb";
            key = KeyCode.K; 
            journal = GameObject.Find("JournalP2");
            dialogueBox = GameObject.Find("DialogueTextboxP2").GetComponent<Text>();
            objectiveBox = GameObject.Find("ObjectiveTextboxP2").GetComponent<Text>();
        }
        
        journal.SetActive(false);

        ReadInDialogueList();
        ReadInObjectiveList();
        AddTextToJournal();

        /*
        dialogueBoxP1 = GameObject.Find("DialogueTextboxP1").GetComponent<Text>();
        dialogueBoxP2 = GameObject.Find("DialogueTextboxP2").GetComponent<Text>();
        objectiveBoxP1 = GameObject.Find("ObjectiveTextboxP1").GetComponent<Text>();
        objectiveBoxP2 = GameObject.Find("ObjectiveTextboxP2").GetComponent<Text>();
        */
    }
	
	// Update is called once per frame
	void Update () {
        
        //Use former line when input manager fixed

        //if (Input.GetButtonDown(button) || Input.GetKeyDown(key)) {
        if (Input.GetKeyDown(key)) { 
            ToggleJournal(); 
        }
    }


    void ToggleJournal() {
        if (journalon == true) {
            journalon = false;
            journal.SetActive(false);
        }
        else {
            journalon = true;
            journal.SetActive(true);
        }
    }

    public void AddTextToJournal() {
        if (objectives.Length == 1) {
            objectives = jm.GetObjective();
        }
        for (int i = 0; i < objectives.Length; i++) {
            objectiveBox.text += objectives[i];
            objectiveBox.text += "\n"; 
        }
        if (dialogue.Length == 1) {
            dialogue = jm.GetDialogue();
        }
        for (int i = 0; i < dialogue.Length; i++) {
            dialogueBox.text += dialogue[i];
            dialogueBox.text += "\n";
        }
    }

    private void ReadInDialogueList() {
        dialogue = textfile.text.Split('\n');
    }

    private void ReadInObjectiveList() {
        objectives = textfile.text.Split('\n');
    }


}
