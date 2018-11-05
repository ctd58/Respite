using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StaminaBar : MonoBehaviour {

    
    //Gets the Green part of the stamina bar and the text on it. 
    public Image staminaBar;
    public Text ratioStamina;

    //Get the player, also will need to get if player is allowed to move or not (need to check character controller for that). 
    public bool player1;
    private Vector3 basePosition;
    private Vector3 currPosition;

    //Get the value of the other player's stamina
    public Text staminaOtherPlayer; 

    //Controls how many frames that needs to pass before the function should check if the player has moved (for LoseStamina())
    public int frameCheck = 5;
    private int counter = 0; 

    //Currently setting the max stamina to be 400 S-Points. Will have a slider for this in the future. 
    public int maxStamina = 400;
    public int currStamina = 400;
    public int regenRate = 105; 

    private GameObject player;

	// Use this for initialization
	void Start () {
        if (player1) player = GameObject.FindGameObjectWithTag("P1");
        else player = GameObject.FindGameObjectWithTag("P2");
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
        ratioStamina.text = (ratioNumber * 100).ToString() + '%';
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
    /// <summary>
    /// Personally, I don't agree with the group or the critique that the stamina should of both players should at least be 100% 
    /// of one person's bar at all times. However, because it was the majority rule, the new RegenStamina will do as they wish, 
    /// and the old one should be commented out. Should there be a need to revert, I will leave the old function, untouched, but commented out.
    /// </summary>
    
    private void RegenStamina()
    {
        string c = staminaOtherPlayer.text;
        string[] words = c.Split('.', '%');
        int player2ratio = int.Parse(words[0], System.Globalization.NumberStyles.AllowDecimalPoint);
        int truevalue = (player2ratio * maxStamina);
        Debug.Log(truevalue); 

        //If nonmoving player's stamina is less than max stamina - moving player's stamina 
        if ((maxStamina - truevalue) > currStamina)//Replace
        {
            currStamina = maxStamina - truevalue; //moving player's stamina. 
        }
        else
        {
            currStamina += regenRate;
            if (currStamina > maxStamina)
            {
                currStamina = maxStamina;
            }
        }
        if (currStamina > maxStamina){
            currStamina = maxStamina; 
        }
    }
    /*
    private void RegenStamina()
    {
        //for everytime regenstamina is called, player regens 15. 
        currStamina += regenRate; 
        if (currStamina > maxStamina)
        {
            currStamina = maxStamina; 
        }
    }
    */

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
