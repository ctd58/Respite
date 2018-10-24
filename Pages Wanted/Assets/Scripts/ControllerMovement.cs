using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ControllerMovement : MonoBehaviour {

    public float speed = 6.0f;
    public float rotateSpeed = 90.0f;
    public bool isP1 = true;
    public bool canMove = true;
    public ControllerMovement teammate;
    private string playerNum = "";
    private string switchb = "";
    public Sound sound;
    private Vector3 mov;
    public CharacterController _mycontroller;

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetFloat("PlayerSpeed", speed);
        PlayerPrefs.SetFloat("PlayerMoveX", mov.x);
        PlayerPrefs.SetFloat("PlayerMoveY", mov.y);
        PlayerPrefs.SetFloat("PlayerMoveZ", mov.z);
        PlayerPrefs.SetString("Player1AllowedtoMove", "true");
        PlayerPrefs.SetString("Player2AllowedtoMove", "false");
        if (this.tag == "P1")
        {
            isP1 = true;
            canMove = true;
            teammate = GameObject.FindGameObjectWithTag("P2").GetComponent<ControllerMovement>();
            playerNum = "P1";
            switchb = "P1yt button";
        }
        else if (this.tag == "P2")
        {
            isP1 = false;
            canMove = false;
            teammate = GameObject.FindGameObjectWithTag("P1").GetComponent<ControllerMovement>();
            playerNum = "P2";
            switchb = "P2yt button";
        }
        _mycontroller = this.GetComponent<CharacterController>();
        sound = this.GetComponent<Sound>();
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EndGame")
        {
            SceneManager.LoadScene("GameOverScreen");
        }
    }

    void Switch()
    {
        canMove = false;
        teammate.canMove = true;
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
        if (canMove)
        {
            Move();
            if (Input.GetButton(switchb))
            {
                Switch();
            }
        }
        else
        {
            //sound.sound = 0;
        }
        PlayerComeBackasFalse();
    }

    void Move()
    {
        float x = Input.GetAxis(playerNum + "Horizontal");
        float z = Input.GetAxis(playerNum + "Vertical");

        mov = new Vector3(x * speed * Time.deltaTime, 0, z * speed * Time.deltaTime);
        PlayerPrefs.SetFloat("PlayerMoveX", mov.x);
        PlayerPrefs.SetFloat("PlayerMoveY", mov.y);
        PlayerPrefs.SetFloat("PlayerMoveZ", mov.z);
        _mycontroller.Move(transform.TransformDirection(mov));
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

}
