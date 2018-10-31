using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenNav : MonoBehaviour {


    public Button playButton;
    public Button quitButton;
    public GameObject titleScreen;
    public GameObject titleScreenBG; 
    public string gameScene;

    [SerializeField]
    public Slider monsterSense;
    [SerializeField]
    public Slider playerSpeed;
    [SerializeField]
    public Slider monsterBaseSpeed;
    [SerializeField]
    public Slider staminaMeter; 

    private void Start()
    {
        Button btn1 = playButton.GetComponent<Button>();
        Button btn2 = quitButton.GetComponent<Button>();

        btn1.onClick.AddListener(StartGame);
        btn2.onClick.AddListener(ExitScene);

        playerSpeed.value = PlayerPrefs.GetFloat("playerspeed"); 
        monsterBaseSpeed.value = PlayerPrefs.GetFloat("monsterbasespeed");
        monsterSense.value = PlayerPrefs.GetFloat("monstersense");
        staminaMeter.value = PlayerPrefs.GetFloat("staminaMeter"); 

        if (titleScreen.activeSelf != true)
        {
            titleScreen.SetActive(true); 
        }
        if (titleScreenBG.activeSelf != true)
        {
            titleScreenBG.SetActive(true);
        }

    }

    private void Update()
    {
        PlayerPrefs.SetFloat("playerspeed", playerSpeed.value);
        PlayerPrefs.SetFloat("monsterspeed", monsterBaseSpeed.value);
        PlayerPrefs.SetFloat("monstersense", monsterSense.value);
        PlayerPrefs.SetFloat("staminaMeter", staminaMeter.value); 
    }


    private void StartGame()
    {
        SceneManager.LoadScene(gameScene); 
    }

    private void ExitScene()
    {
        Application.Quit(); 
    }
}
