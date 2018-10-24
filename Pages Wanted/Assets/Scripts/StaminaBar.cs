using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StaminaBar : MonoBehaviour {


    //Gets the Green part of the stamina bar and the text on it. 
    public Image staminaBar;
    public Text ratioStamina;

    //Get the player, also will need to get if player is allowed to move or not (need to check character controller for that). 
    public GameObject player;
    public Vector3 basePosition;
    public Vector3 currPosition;

    //

    //Controls how many frames that needs to pass before the function should check if the player has moved (for LoseStamina())
    public int frameCheck = 10;
    private int counter = 0; 

    //Currently setting the max stamina to be 400 S-Points. Will have a slider for this in the future. 
    public int maxStamina = 400;
    public int currStamina = 400;
    public int regenRate = 5; 


	// Use this for initialization
	void Start () {
        float x = PlayerPrefs.GetFloat("PlayerMoveX");
        float y = PlayerPrefs.GetFloat("PlayerMoveY");
        float z = PlayerPrefs.GetFloat("PlayerMoveZ");
        maxStamina = (int)PlayerPrefs.GetFloat("staminaMeter");
        currStamina = maxStamina; 
        basePosition = new Vector3(x, y, z);
        currPosition = basePosition; 
	}
	
    //Function wrapper to controller if stamina needs to be drained or regenerated
    private void UpdateStaminaBar()
    {
        string c = "true";
        //placeholder, need to figure out the variable that causes players to switch
        if (player.tag == "P1")
        {
            if (PlayerPrefs.GetString("Player1AllowedtoMove") == c)
            {
                LoseStamina();
            }
            else
            {
                RegenStamina();
            }
        }

        if (player.tag == "P2")
        {
            if (PlayerPrefs.GetString("Player2AllowedtoMove") == c)
            {
                LoseStamina();
            }
            else
            {
                RegenStamina();
            }
        }

        float ratioNumber = (float)currStamina / (float)maxStamina;
        staminaBar.rectTransform.localScale = new Vector3(ratioNumber, 1, 1);
        //ratioStamina.text = (ratioNumber * 100).ToString() + '%';
        if (currStamina == 0 && PlayerPrefs.GetString("Player1AllowedtoMove") == "true")
        {
            PlayerPrefs.SetString("Player1AllowedtoMove", "false");
            PlayerPrefs.SetString("Player2AllowedtoMove", "true");
            RegenStamina(); 
        }
        if (currStamina == 0 && PlayerPrefs.GetString("Player2AllowedtoMove") == "true")
        {
            PlayerPrefs.SetString("Player1AllowedtoMove", "true");
            PlayerPrefs.SetString("Player2AllowedtoMove", "false");
            RegenStamina();
        }
    }

    //For ALL General Cases (not including forced switches). 
    //If the player has been moving, they should lose stamina by a certain ratio amount that they are allowed to move. 
    //If the player has not been moving, but is allowed to move, they neither regain nor lose stamina
    //If the player is not allowed to move, they should regain stamina by a set amount.

    
    private void LoseStamina()
    {
        if (currPosition == basePosition)
        {
            //You good, Health bar stays the same
        }
        else
        {
            //Calculate how much the player moves based on position. 
            Vector3 calculated = new Vector3((player.transform.position.x - basePosition.x), (player.transform.position.y - basePosition.y), (player.transform.position.z - basePosition.z));
            float calculatedX = Mathf.Abs(calculated.x);
            float calculatedY = Mathf.Abs(calculated.y);
            float calculatedZ = Mathf.Abs(calculated.z);
            float finalCalc = calculatedX + calculatedY + calculatedZ;
            float fullLoss = finalCalc / (PlayerPrefs.GetFloat("PlayerSpeed"));
            currStamina = currStamina - (int)fullLoss; 
            if (currStamina <= 0)
            {
                currStamina = 0; 
            }
            
        }
    }

    private void RegenStamina()
    {
        //for everytime regenstamina is called, player regens 15. 
        currStamina += regenRate; 
        if (currStamina > maxStamina)
        {
            currStamina = maxStamina; 
        }
    }

	// Update is called once per frame
	void Update () {
        float x = PlayerPrefs.GetFloat("PlayerMoveX");
        float y = PlayerPrefs.GetFloat("PlayerMoveY");
        float z = PlayerPrefs.GetFloat("PlayerMoveZ");
        currPosition = new Vector3(x, y, z); 
        if (counter == frameCheck)
        {
            counter = 0;
            UpdateStaminaBar();
            basePosition = currPosition; 
        }
        else
        {
            counter++;
        }
   	}
}
