using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour {

    public GameObject monster;
    public ControllerMovement player;
    public Sound pSound;
    public bool canStun = true;
    public bool canSlow = true;
    public float slowSpeed = 0.5f;
    public float stunLen = 2.0f;
    public string pTag = "";
    public float castLen = 0.0f; //sec
    public float maxCastLen = 5.0f; //sec

    // Use this for initialization
    void Start()
    {
        monster = GameObject.FindGameObjectWithTag("Monster");
        player = this.GetComponent<ControllerMovement>();
        pSound = this.GetComponent<Sound>();
        pTag = this.gameObject.tag;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(pTag + "bo button") && !player.canMove)
        {
            Debug.Log("Got the button for " + pTag);
            castLen += Time.deltaTime;
            if (castLen >= maxCastLen)
            {
                castLen = 0;
                Debug.Log("Done");
            }
        }
        else
        {
            castLen = 0;
        }
    }
}
