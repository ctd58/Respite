using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenNav : MonoBehaviour {

    public static bool isGamePaused = true;
    private string pauseButton1 = "MainGamePauseP1";
    private string pauseButton2 = "MainGamePauseP2";
    public GameObject pauseMenu;

    private void Start() {
        pauseMenu.SetActive(false); 
    }

    public void PauseMenu() {
        if (isGamePaused == true) {
            //Resume
            Resume(); 
            isGamePaused = false; 
        }
        else {
            //Pause
            Pause(); 
            isGamePaused = true; 
        }
    }

    public void Resume() {
        pauseMenu.SetActive(false); 
        Time.timeScale = 1f; 
    }

    public void Pause() {
        pauseMenu.SetActive(true); 
        Time.timeScale = 0f; 
    }

    public void Quit() {
        Application.Quit(); 
    }
}
