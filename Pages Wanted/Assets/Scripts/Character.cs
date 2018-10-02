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
    public float speed = 10;
    public bool isP1 = true;
    private Vector3 move;
    public Character teammate;
    public int keys = 0;
    public Sound noise;

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
                move = new Vector3(Input.GetAxis("P1Horizontal"), 0, Input.GetAxis("P1Vertical"));
            }
            else
            {
                move = new Vector3(Input.GetAxis("P2Horizontal"), 0, Input.GetAxis("P2Vertical"));
            }
            _controller.Move(move * Time.deltaTime * speed); //maybe for running and tiptoeing

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
