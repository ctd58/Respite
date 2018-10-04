using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMovement : MonoBehaviour {

    public float speed = 6.0f;
    public float rotateSpeed = 90.0f;
    public bool isP1 = true;
    public bool canMove = true;
    public ControllerMovement teammate;
    private string playerNum = "";
    private string switchb = "";
    public Sound sound;

	// Use this for initialization
	void Start () {
        if (this.tag == "P1")
        {
            isP1 = true;
            canMove = true;
            teammate = GameObject.FindGameObjectWithTag("P2").GetComponent<ControllerMovement>();
            playerNum = "P1";
            switchb = "rbe";
        }
        else if (this.tag == "P2")
        {
            isP1 = false;
            canMove = false;
            teammate = GameObject.FindGameObjectWithTag("P1").GetComponent<ControllerMovement>();
            playerNum = "P2";
            switchb = "lbq";
        }

        sound = this.GetComponent<Sound>();
        
    }
	
	// Update is called once per frame
	void Update () {
		
        



    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Move();
            if (Input.GetButton(switchb))
            {
                canMove = false;
                teammate.canMove = true;
            }
        }
        else
        {
            sound.sound = 0;
        }
    }

    void Move()
    {
        float x = Input.GetAxis(playerNum + "Horizontal");
        float z = Input.GetAxis(playerNum + "Vertical");

        transform.Translate(new Vector3(x * speed * Time.deltaTime, 0, z * speed * Time.deltaTime));
        transform.Rotate(new Vector3(0, Input.GetAxis(playerNum + "Mouse X") * rotateSpeed * Time.deltaTime, 0));
        if (x == 0.0f && z == 0.0f)
            sound.sound = 0;
        else
            sound.sound = 1;

    }
}
