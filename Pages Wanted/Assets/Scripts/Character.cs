using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    /*
     * Variables used for the character controllers, some things may be changed when converting to rigidbodies
     * P1 starts out being able to move (bool isP1 = true), the teammate at default, P2, is unable to move (bool canMove = true). 
    */
    public CharacterController _controller;
    public bool canMove = true;
    public float speed = 3.0f;
    public bool isP1 = true;
    private Vector3 move;
    private Vector3 ro;
    public float rotateSpeed = 30.0f;
    public Character teammate;
    public int keys = 0;
    public Sound noise;
    public float moveH;
    public float moveV;
    public float lookH;
    private Vector3 foward;

	// Use this for initialization
	void Start () {
        /* gets controller
         * if the tag is equal to P1, P1 can move, but P2 cannot
         * Otherwise, P2 can move, but P2 cannot
         * teammate grabs the character object that cannot move
        */ 
        _controller = GetComponent<CharacterController>();
        if (this.tag == "P1")
        {
            isP1 = true;
            canMove = true;
            teammate = GameObject.FindGameObjectWithTag("P2").GetComponent<Character>();
        }
        else if(this.tag == "P2")
        {
            isP1 = false;
            canMove = false;
            teammate = GameObject.FindGameObjectWithTag("P1").GetComponent<Character>();
        }

        noise = this.GetComponent<Sound>();
	}
	
	// Update is called once per frame
	void Update () {
        /*
         * if canMove is true for any player, that player is allowed to move vertically and horizontally
         * if P1 has the ability to move, and the "1" key is pressed, P1's canMove attribute is set to false,
         * while P2's canMove attribute is set to true
         * if P2 has the ability to move, and the "2" key is pressed, P2's canMove attribute is set to false,
         * while P1's canMove attribute is set to true
        */
        if (canMove)
        {
            if (isP1)
            {

                foward = Input.GetAxis("P1Vertical") * transform.TransformDirection(Vector3.forward) * speed;
                transform.Rotate(new Vector3(0, Input.GetAxis("P1Horizontal") * rotateSpeed * Time.deltaTime, 0));

            }
            else
            {
                foward = Input.GetAxis("P2Vertical") * transform.TransformDirection(Vector3.forward) * speed; //maybe for running and tiptoeing
                transform.Rotate(new Vector3(0, Input.GetAxis("P2Horizontal") * rotateSpeed * Time.deltaTime, 0));
            }
            _controller.Move(foward * Time.deltaTime);
            
        }

        if (isP1 && Input.GetKey(KeyCode.Alpha1))
        {
            canMove = false;
            teammate.canMove = true;
        }

        else if (!isP1 && Input.GetKey(KeyCode.Alpha2))
        {
            canMove = false;
            teammate.canMove = true;
        }
    }
}
