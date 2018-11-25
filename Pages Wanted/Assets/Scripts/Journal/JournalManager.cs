using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 

public class JournalManager : MonoBehaviour {

    //private Journal journalP1;
    //private Journal journalP2;

    public TextAsset textfile; 

    private string[] objective = new string[] {""};
    private string[] dialogue = new string[] { "" }; 

    //Functions
    //Return the whole objectives array, as well as the dialogue array. 
    //Return parts of these arrays. 
    //Read from a text file and insert it into any one of these arrays. 

    

	// Use this for initialization
	void Start () {
        ReadInDialogueList();
        ReadInObjectiveList(); 
	}

    public string[] GetObjective() {
        return objective; 
    }

    public string[] GetDialogue() {
        return dialogue; 
    }

    private void ReadInDialogueList() {
        dialogue = textfile.text.Split('\n');

        foreach (string line in dialogue) {
            Debug.Log(line);
        }
    }

    private void ReadInObjectiveList() {
        objective = textfile.text.Split('\n');

        foreach (string line in dialogue) {
            Debug.Log(line);
        }
    }
}
