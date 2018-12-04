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
	void Awake () {
        ReadInDialogueList();
        ReadInObjectiveList(); 
	}

    public string[] GetObjective() {
        return objective; 
    }

    public string[] GetDialogue() {
        return dialogue; 
    }

    public string[] GetDifObjective(int beginning, int end) {
        string[] newobjective = new string[] { "" };
        for (int i = 0; i < (end - beginning); i++) {
            newobjective[i] = objective[beginning + i];
        }
        return newobjective;
    }

    public string[] GetDifDialogue(int beginning, int end) {
        string [] newdialogue = new string[] { "" };
        for (int i = 0; i < (end - beginning); i++) {
            newdialogue[i] = dialogue[beginning + i]; 
        }
        return newdialogue; 
    }

    private void ReadInDialogueList() {
        dialogue = textfile.text.Split('\n');
    }

    private void ReadInObjectiveList() {
        objective = textfile.text.Split('\n');
    }

    private void PrintDialogueList() {
        //dialogue = textfile.text.Split('\n');

        foreach (string line in dialogue) {
            Debug.Log(line);
        }
    }

    private void PrintObjectiveList() {
        //objective = textfile.text.Split('\n');

        foreach (string line in dialogue) {
            Debug.Log(line);
        }
    }
}
