using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public Transform target = null;
    public float baseMoveSpeed = 4.0f;
    public float rotSpeed, movSpeed;
    public float distance;
    public float maxDistance;
    public List<GameObject> sounds;
    public float chaseTime = 0.0f;
    public float slowSpeed = 1.0f;
    public bool canMove = true;
    public float inputDelay = 0.0f;
    public int playerLives = 4;
    public GameObject spawn;
    [SerializeField]
    public GameObject health1;
    [SerializeField]
    public GameObject health2;
    [SerializeField]
    public GameObject health3;
    [SerializeField]
    public GameObject health4;
    [SerializeField]
    public GameObject gameoverScreen; 


    void Start()
    {
        spawn = GameObject.FindGameObjectWithTag("DemonSpawn");
        sounds.Add(GameObject.FindGameObjectWithTag("P1"));
        sounds.Add(GameObject.FindGameObjectWithTag("P2"));
        gameoverScreen.SetActive(false); 
    }

    void Update()
    {
        //Widchard use this to call game over screen
        if(playerLives == 0)
        {
            Debug.Log("Game Over");
            gameoverScreen.SetActive(true);
        }

        findTarget();
        //target = GameObject.FindGameObjectWithTag("P1").transform;
        //if (Vector3.Distance(target.position, gameObject.transform.position) <= maxDistance)
        if (target != null && canMove)
        {
            FollowSound();
            chaseTime += Time.deltaTime;
        }
        else if(canMove)
        {
            chaseTime = 0.0f;
        }
        else if(!canMove)
        {
            Debug.Log("STUNNED");
            StartCoroutine(stun());
        }
    }

    //makes it go towards sound
    void FollowSound()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotSpeed * Time.deltaTime);

        transform.position += transform.forward * (movSpeed * slowSpeed) * Time.deltaTime;
    }

    //Determines what object is making the loudest noise and goes to it
    void findTarget()
    {
        //every 2.5 secs the monster is chasing it gets faster by .75
        if (chaseTime / 2.5f > 1.0f)
        {

            chaseTime -= 2.5f;
            movSpeed += 0.75f;
        }
        //If not chasing rest speed
        else if (chaseTime == 0.0f)
        {
            movSpeed = baseMoveSpeed;
        }

        //Determines what sound is the loudest and sets it as a target
        //PLEASE try to get the AI to remember the location (waypoint) of where the last sound came from and go to that
        //I cant get it to remember it forgets once the player moves off of the floor board
        target = null;
        float highest = 0.0f;
        float temp = 0.0f;
        foreach (GameObject noise in sounds)
        {
            temp = noise.GetComponent<Sound>().sound;
            if (temp > highest)
            {
                highest = temp;
                target = noise.transform;
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("I gotcha boi");
        if(other.gameObject.tag == "P1" || other.gameObject.tag == "P2")
        {
            playerLives -= 1;
            respawn();
            hideHealth(); 
        }
    }

    private void hideHealth()
    {
        health1.SetActive(false);
        if (health1.activeSelf == false && health2.activeSelf == true && health3.activeSelf == true && health4.activeSelf == true)
        {
            health2.SetActive(false);
        }
        else if(health1.activeSelf == false && health2.activeSelf == false && health3.activeSelf == true && health4.activeSelf == true)
        {
            health3.SetActive(false); 
        }
        else if(health1.activeSelf == false && health2.activeSelf == false && health3.activeSelf == false && health4.activeSelf == true)
        {
            health4.SetActive(false); 
        }
    }

    void respawn()
    {
        this.transform.position = spawn.transform.position;
    }

    IEnumerator stun()
    {
        Debug.Log(Time.time);
        yield return new WaitForSecondsRealtime(inputDelay);
        canMove = true;
        Debug.Log(Time.time);
    }
}
