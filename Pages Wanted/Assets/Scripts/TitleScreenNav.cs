using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class TitleScreenNav : MonoBehaviour {

    public GameObject titlescreen;
    public GameObject mainmenu;

    int i = -1; 

	// Use this for initialization
	void Start () {
        titlescreen.SetActive(true); 
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            i = 1; 
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            i = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            i = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            i = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            i = 5;
        }
        traverse(i);
	}

    private void traverse(int i)
    {
        if (i > 0 && titlescreen.activeSelf == true)
        {
            titlescreen.SetActive(false);
            mainmenu.SetActive(true); 
        }
        else if (i == 1 && mainmenu.activeSelf == true)
        {
            //Play Game
            SceneManager.LoadScene(1); 
        }
        else if (i == 0 && mainmenu.activeSelf == true)
        {
            titlescreen.SetActive(true);
            mainmenu.SetActive(false);
        }
        else if (i == 2 && mainmenu.activeSelf == true)
        {
            Debug.Log("controls");
            //Controls
            //SceneManager.LoadScene(1);
        }
        else if (i == 3 && mainmenu.activeSelf == true)
        {
            Debug.Log("settings");
            //Settings
            //SceneManager.LoadScene(1);
        }
        else if (i == 4 && mainmenu.activeSelf == true)
        {
            Debug.Log("credits");
            //Settings
            //SceneManager.LoadScene(1);
        }

    }

}
