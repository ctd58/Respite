using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ControllerMovement : MonoBehaviour {
	// Public or Serialized Variables for Inspector -----------------
	#region Public Variables
    public float speed = 6.0f;
    public float rotateSpeed = 90.0f;
    public AudioSource thumpNoise;
    [SerializeField] [Range(0.0f, 5.0f)] private float thumpVolume = 3.0f;
    #endregion

    // Private Variables ---------------------------------------------
	#region Private Variables
    private bool isP1 = true;
    private ControllerMovement teammate;
    private bool canMove = true;
    private string playerNum = "";
    private string switchb = "";
    private CharacterController _mycontroller;
    private Sound sound;
    private Vector3 mov;
    #endregion


#if UNITY_EDITOR_WIN

#endif
	// Use this for initialization
	void Start () {
        PlayerPrefs.SetFloat("PlayerSpeed", speed);
        PlayerPrefs.SetFloat("PlayerMoveX", mov.x);
        PlayerPrefs.SetFloat("PlayerMoveY", mov.y);
        PlayerPrefs.SetFloat("PlayerMoveZ", mov.z);
        PlayerPrefs.SetString("Player1AllowedtoMove", "true");
        PlayerPrefs.SetString("Player2AllowedtoMove", "false");

        //Checks if tag on palyer is p1 if it is then it sets the ability to move to true
        //Finds the other players component
        //Sets a string for the tag so we can call the specific inputs for each player
        //Sets the switch button for that player
        if (this.tag == "P1") {
            isP1 = true;
            canMove = true;
            teammate = GameObject.FindGameObjectWithTag("P2").GetComponent<ControllerMovement>();
            playerNum = "P1";
            switchb = "P1yt button";
        } else if (this.tag == "P2") {
            isP1 = false;
            canMove = false;
            teammate = GameObject.FindGameObjectWithTag("P1").GetComponent<ControllerMovement>();
            playerNum = "P2";
            switchb = "P2yt button";
        }

        //Gets controller component on the player
        _mycontroller = this.GetComponent<CharacterController>();
        
        //Gets the sound component on the player
        sound = this.GetComponent<Sound>();
        
        //Gets the thump audio source on the player
        //TODO: make private and get via thumpNoise = this.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        //Checks if player wants to quit the game
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();



    }

    private void OnTriggerEnter(Collider other)
    {
        //Sends game to gameover screen
        if (other.tag == "EndGame")
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    //Handles switching of player
    void Switch()
    {
        //Sets current player to can not move and other player to can move
        canMove = false;
        teammate.canMove = true;

        //Dont know what this does, I believe Widchard added it
        if (teammate == GameObject.FindGameObjectWithTag("P2").GetComponent<ControllerMovement>())
        {
            PlayerPrefs.SetString("Player1AllowedtoMove", "false");
            PlayerPrefs.SetString("Player2AllowedtoMove", "true");
        }
        else
        {
            PlayerPrefs.SetString("Player1AllowedtoMove", "true");
            PlayerPrefs.SetString("Player2AllowedtoMove", "false");
        }
    }

    void FixedUpdate()
    {
        //If current player can move then get movement input and check if they press the switch button
        if (canMove)
        {
            Move();

            //If switch putton is pressed for this player and they can move then switch which player can move
            if (Input.GetButtonDown(switchb))
            {
                Switch();
            }
        }
        else
        {
            //sound.sound = 0;
        }

        //Dont know what this does i believe widchard added it
        PlayerComeBackasFalse();
    }

    // Gets player movement and look
    void Move()
    {
        //Gets vertical and horizontal input from players input button (PlayerTag + ButtonName)
        float x = Input.GetAxis(playerNum + "Horizontal");
        float z = Input.GetAxis(playerNum + "Vertical");

        //Makes a vector based on speed and z and x inputs
        mov = new Vector3(x * speed * Time.deltaTime, 0, z * speed * Time.deltaTime);

        PlayerPrefs.SetFloat("PlayerMoveX", mov.x);
        PlayerPrefs.SetFloat("PlayerMoveY", mov.y);
        PlayerPrefs.SetFloat("PlayerMoveZ", mov.z);

        //Moves the player in the direction they are facing (Direction camera is looking)
        _mycontroller.Move(transform.TransformDirection(mov));

        //Rotates the object in the x direction when the look button is used
        transform.Rotate(new Vector3(0, Input.GetAxis(playerNum + "Mouse X") * rotateSpeed * Time.deltaTime, 0));
       
        /*if (x == 0.0f && z == 0.0f)
            sound.sound = 0;
        else
            sound.sound = 1;*/

    }

    //If playerpref comes back as false. 
    void PlayerComeBackasFalse()
    {
        if (isP1 == true && canMove.ToString() != PlayerPrefs.GetString("Player1AllowedtoMove"))
        {
            if (PlayerPrefs.GetString("Player1AllowedtoMove") == "false")
            {
                Switch();
            }
            else
            {
                //canMove = true; 
            }
        }
        if (isP1 == false && canMove.ToString() != PlayerPrefs.GetString("Player2AllowedtoMove"))
        {
            if (PlayerPrefs.GetString("Player2AllowedtoMove") == "false")
            {
                Switch(); 
            }
            else
            {
                //canMove = true;
            }
        }
    }

    public bool GetCanMove() {
        return canMove;
    }

    /*
    void OnControllerColliderHit(ControllerColliderHit hit) {
        Debug.Log("COLLISION");
        StartCoroutine("BumpSound");
    }

    IEnumerator BumpSound() {
        thumpNoise.Play();
        sound.sound = thumpVolume;
        yield return new WaitForSeconds(thumpNoise.clip.length);
        thumpNoise.Stop();
        sound.sound = 0f;
    }
    */
}
