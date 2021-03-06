﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video; 

public class GameOverScreen : MonoBehaviour {

    public Button playButton;
    public Button mainmenuButton;
    public VideoPlayer vp; 

    void Start () {
        Button btn1 = playButton.GetComponent<Button>();
        Button btn2 = mainmenuButton.GetComponent<Button>();

        vp.Play(); 
        btn1.onClick.AddListener(replayGame);
        btn2.onClick.AddListener(mainMenu);
    }
	
    private void replayGame()
    {
        vp.Stop(); 
        SceneManager.LoadScene("1.03.00"); 
    }

    private void mainMenu()
    {
        vp.Stop();
        SceneManager.LoadScene("TitleScreen");
    }
}
