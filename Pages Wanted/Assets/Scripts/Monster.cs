using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public Transform target;
    public float baseMoveSpeed = 4.0f;
    public float rotSpeed, movSpeed;
    public float distance;
    public float maxDistance;
    public List<GameObject> sounds;
    public float chaseTime = 0.0f;

    void Start()
    {
        sounds.Add(GameObject.FindGameObjectWithTag("P1"));
        sounds.Add(GameObject.FindGameObjectWithTag("P2"));
    }

    void Update()
    {
        findTarget();
        //target = GameObject.FindGameObjectWithTag("P1").transform;
        //if (Vector3.Distance(target.position, gameObject.transform.position) <= maxDistance)
        if (target != null)
        {
            FollowSound();
            chaseTime += Time.deltaTime;
        }
        else
        {
            chaseTime = 0.0f;
        }
    }

    void FollowSound()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotSpeed * Time.deltaTime);

        transform.position += transform.forward * movSpeed * Time.deltaTime;
    }

    void findTarget()
    {
        if (chaseTime / 2.5f > 1.0f)
        {

            chaseTime -= 2.5f;
            movSpeed += 0.75f;
        }
        else if (chaseTime == 0.0f)
        {
            movSpeed = baseMoveSpeed;
        }
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
}
