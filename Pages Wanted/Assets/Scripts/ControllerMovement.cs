using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rewired;

public class ControllerMovement : MonoBehaviour {
	// Public or Serialized Variables for Inspector -----------------
	#region Public Variables
    public bool debug;
    public float speed = 6.0f;
    public float rotateSpeed = 90.0f;
    public AudioSource audio;
    public AudioClip thump;
    public Animator myAnim;
    public AudioClip cloth;
    [SerializeField] [Range(0.0f, 5.0f)] private float thumpVolume = 3.0f;
    [SerializeField] public Image imageBrand;
    #endregion

    // Private Variables ---------------------------------------------
    #region Private Variables
    private int playerId; 
    // The Rewired player id of this character
    private Player player; 
    // The Rewired Player
    private bool isP1 = true;
    private ControllerMovement teammate;
    private bool canMove = true;
    private string playerNum = "";
    private string switchb = "";
    private CharacterController _mycontroller;
    private Sound sound;
    private Vector3 mov;
    private bool grounded;
    private Camera pCamera;
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
            playerId = 0;
            canMove = true;
            teammate = GameObject.FindGameObjectWithTag("P2").GetComponent<ControllerMovement>();
            playerNum = "P1";
            switchb = "P1yt button";
        } else if (this.tag == "P2") {
            isP1 = false;
            playerId = 1;
            canMove = false;
            teammate = GameObject.FindGameObjectWithTag("P1").GetComponent<ControllerMovement>();
            playerNum = "P2";
            switchb = "P2yt button";
        }

        //Gets controller component on the player
        _mycontroller = this.GetComponent<CharacterController>();
        
        //Gets the sound component on the player
        sound = this.GetComponent<Sound>();

        //Gets audio source component
        audio = GetComponent<AudioSource>();
        
        //Gets the thump audio source on the player
        //TODO: make private and get via thumpNoise = this.GetComponent<AudioSource>();
        pCamera = GameObject.Find(playerNum + "Camera").GetComponent<Camera>();

        player = ReInput.players.GetPlayer(playerId);
        if (debug) Debug.Log("REWIRED PLAYER " + player);
    }
	
	// Update is called once per frame
	void Update () {
        //Checks if player wants to quit the game
        // if (Input.GetKey(KeyCode.Escape))
        //     Application.Quit();

    }

    public bool CanMove() {
        return canMove; 
    }

    private void OnTriggerEnter(Collider other)
    {
        //Sends game to gameover screen
        if (other.tag == "EndGame")
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    void FixedUpdate()
    {
        Look();
        //If current player can move then get movement input and check if they press the switch button
        if (canMove)
        {
            Move();

            //If switch putton is pressed for this player and they can move then switch which player can move
            // if (Input.GetButtonDown(switchb))
            // {
            //     Switch();
            // }
        }
        else
        {
            //sound.sound = 0;
        }

        //Dont know what this does i believe widchard added it
        PlayerComeBackasFalse();
    }

    //Handles switching of player
    void Switch()
    {
        //Makes it so that walk animation stops playing
        myAnim.SetFloat("Speed", 0);

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

    // Gets player movement and look
    void Move()
    {
        // Play cloth audio
        // TODO: broken AF
        // audio.PlayOneShot(cloth);

        //Gets vertical and horizontal input from Rewired player by name or action id
        float x = player.GetAxis("Move Horizontal");
        float z = player.GetAxis("Move Vertical");
        if (debug) Debug.Log("REWIRED X MOVEMENT " + player);
        if (debug) Debug.Log("REWIRED Y MOVEMENT " + player);

        //Change to scale if needed. 
        float gravity = 980f; 
        float y = gravity * Time.deltaTime;

        if (grounded == true) {
            //grounded
            y = 0;
        }
        
        //Makes a vector based on speed and z and x inputs
        mov = new Vector3(x * speed * Time.deltaTime, -y, z * speed * Time.deltaTime);

        PlayerPrefs.SetFloat("PlayerMoveX", mov.x);
        PlayerPrefs.SetFloat("PlayerMoveY", mov.y);
        PlayerPrefs.SetFloat("PlayerMoveZ", mov.z);

        //Passes speed to the animator
        myAnim.SetFloat("Speed", Mathf.Abs(mov.x + mov.z));
        if (debug) Debug.Log("Speed: " + Mathf.Abs(mov.x + mov.z));

        //Moves the player in the direction they are facing (Direction camera is looking)
        _mycontroller.Move(transform.TransformDirection(mov));
    }

    void Look() {
        Vector3 rotateangl = pCamera.gameObject.transform.rotation.eulerAngles;
        float xAxisclamp = 0.0f;
        float rotamnty = 0f;//Input.GetAxis(playerNum + "Mouse Y");
        rotateangl.z = 0;

        xAxisclamp -= rotamnty;

        if (xAxisclamp > 90) {
            xAxisclamp = 90;
            rotateangl.x = 90;
        }
        else if (xAxisclamp < -90) {
            xAxisclamp = -90;
            rotateangl.x = 270;
        }
        else {
            rotateangl.x -= rotamnty;
        }
        //rotateangl = rotateangl * 1.3f; 
        pCamera.gameObject.transform.rotation = Quaternion.Euler(rotateangl);


        //Rotates the object in the x direction when the look button is used
        //transform.Rotate(0, Input.GetAxis(playerNum + "Mouse X") * rotateSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Monster") {
            if (debug) Debug.Log("cool");
            
            //imageBrand.enabled = true; 
        }
        grounded = true; 
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
        if (debug) Debug.Log("COLLISION");
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
