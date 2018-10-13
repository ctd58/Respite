using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Monster : MonoBehaviour {

    /*Save current position and all list of waypoints*/

    [SerializeField]
    Transform curr_waypoint;

    [SerializeField]
    List<Transform> waypoints = new List<Transform>();

    //Navmesh for AI
    NavMeshAgent navMeshAgent;


    bool h;
    int index = 0;
    public int z = 300;
    int counter = 0;

    // Use this for initialization
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        //loudestpoint1();
    }

    private void Update()
    {
        counter++;
        Debug.Log(counter);

        if (counter >= z)
        {
            counter = 0;
            h = true;
            if (h == true)
            {
                index = (index + 1) % (waypoints.Count);
                curr_waypoint = waypoints[index];
                loudestpoint(curr_waypoint);
            }
        }
        else
        {
            Debug.Log("wait");
        }

    }

    private void createNewwaypoint()
    {

    }

    private void loudestpoint(Transform newwaypoint)
    {
        if (curr_waypoint != null)
        {
            Vector3 targetV = newwaypoint.transform.position;
            navMeshAgent.SetDestination(targetV);
            h = false;
            //curr_waypoint.transform.position = waypoints[2].transform.position; 
        }
    }

}
