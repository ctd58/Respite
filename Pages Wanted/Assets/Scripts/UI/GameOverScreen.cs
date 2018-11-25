using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {

    public Button playButton;
    public Button mainmenuButton;

    void Start () {
        Button btn1 = playButton.GetComponent<Button>();
        Button btn2 = mainmenuButton.GetComponent<Button>();

        btn1.onClick.AddListener(replayGame);
        btn2.onClick.AddListener(mainMenu);
    }
	
    private void replayGame()
    {
        SceneManager.LoadScene("1.02.00"); 
    }

    private void mainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
