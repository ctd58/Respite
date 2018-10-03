using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Monster : MonoBehaviour {

    /*Save current position and all list of waypoints*/

    [SerializeField]
    Transform curr_position;

    [SerializeField]
    List<Transform> waypoints = new List<Transform>();


    /*Private variables*/ 
    NavMeshAgent navMeshAgent;
    private int wcounter;
    private bool madeit; 

    // Use this for initialization
    void Start () {
        /*Set navMesh*/
        navMeshAgent = this.GetComponent<NavMeshAgent>(); 

        /*if Nav hasnt been set*/
        if(navMeshAgent == null)
        {
            Debug.Log("We have a problem."); 
        }
        else
        {
            /*if none or 1 waypoint, generate waypoints based on sound*/
            if (waypoints == null)
            {
                Debug.Log("No destination.");
            }
            else if(waypoints.Count <= 1)
            {
                Debug.Log("Need to make waypoint dynamically for patrol, not doing that yet."); 
            }
            else
            {
                /*make sure that after walking to a previously set curr_position (if there is one), the first place it goes is to the first item
                 * of the list */
                wcounter = 0;
                madeit = false;
                monsterwalk(); 
            }
        }
	}

    private void monsterwalk()
    {
        /*walking ai, making it go after the waypoint*/
        if (curr_position != null && madeit == false)
        {
            Vector3 targetV = curr_position.transform.position;
            bool s = navMeshAgent.SetDestination(targetV);
            madeit = s;
            Debug.Log(madeit); 

        }
    }

    // Update is called once per frame
    void Update () {
        /*what I believe to be making the ai stuck.*/
		if (madeit == true)
        { 
            if (curr_position.transform.position != waypoints[wcounter].transform.position)
            {
                curr_position.transform.position = waypoints[wcounter].transform.position; 
            }
            wcounter = (wcounter + 1) % waypoints.Count;
            monsterwalk();
            madeit = false; 
        }
	}
}
