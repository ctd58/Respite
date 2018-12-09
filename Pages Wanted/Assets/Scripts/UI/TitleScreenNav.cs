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
    public GameObject staminaToggle; 

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
        Toggle tgl1 = staminaToggle.GetComponent<Toggle>();

        tgl1.isOn = false;
        PlayerPrefs.SetString("staminaToggle", "false");

        btn1.onClick.AddListener(StartGame);
        btn2.onClick.AddListener(ExitScene);
        tgl1.onValueChanged.AddListener(StaminaToggle); 

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

    public void StaminaToggle(bool isClicked) {
        if (isClicked == true) {
            PlayerPrefs.SetString("staminaToggle", "true");
        }
        else {
            PlayerPrefs.SetString("staminaToggle", "false");
        }
        /*
       GameObject P1 = GameObject.FindGameObjectWithTag("P1Stamina");
       GameObject P2 = GameObject.FindGameObjectWithTag("P2Stamina");
       if (isClicked == true) {
            P1.SetActive(true);
            P2.SetActive(true);
            P1.GetComponent<StaminaBar>().enabled = true;
            P2.GetComponent<StaminaBar>().enabled = true;
        }
        else {
            P1.SetActive(false);
            P2.SetActive(false); 
            P1.GetComponent<StaminaBar>().enabled = false;
            P2.GetComponent<StaminaBar>().enabled = false;
        }
        */
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
