using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_MonsterUpdate : MonoBehaviour {

    [SerializeField]
    Transform curr_waypoint;

    [SerializeField]
    List<Transform> waypoints = new List<Transform>();

    [SerializeField]
    List<Sound> soundcues = new List<Sound>();

    [SerializeField]
    List<Transform> soundpoints = new List<Transform>();

    [SerializeField]
    Transform sampleSoundPoint;

    Sound sampleSoundcue = new Sound(); 
    
    

    //Navmesh for AI
    NavMeshAgent navMeshAgent;
    public Monster tracking;


    //Three states, wander/patrol, attack, inspect

    //If hears sound, go to sound. Otherwise, patrol

    bool patrol;
    int index = 0;
    public int z = 600;
    int counter = 0;

    // Use this for initialization
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        tracking = this.GetComponent<Monster>();
        //loudestpoint1();
        sampleSoundcue.sound = .45f; 
    }

    private void Update()
    {
        //If Monster can move
    /*    if (tracking.canMove)
        {
            counter++;
            //Debug.Log(counter);

            //If there's sound, it needs to patrol. 
            if (soundpoints.Count == 0)
            {
                Debug.Log("hi");
                //Z is the amount of frames it needs to make it to the waypoint
                //Eventually refactor into speed
                if (counter >= z)
                {
                    counter = 0;
                    //Patrol stuff
                    patrol = true;

                    //If it made it to the proper waypoint, or there was no original waypoint, start patrolling
                    if (patrol == true)
                    {
                        //Increment counter by 1, get the waypoint based on index, send it to loudest point.
                        index = (index + 1) % (waypoints.Count);
                        curr_waypoint = waypoints[index];
                        loudestpoint(curr_waypoint);
                    }
                }
                //Has not gotten to the proper waypoint, or is traveling to a sound point. 
                else
                {
                    //Debug.Log("wait");
                }
            }
            //Has sound points
            else
            {
                Debug.Log("low");
                //Go through soundpoints, add some holder variables
                Sound loudest = null;
                Transform addpoint = null; 
                int i = 0; 
                //Find the loudest soundpoint
                foreach(Transform s in soundpoints)
                {
                    if (soundcues == null || i > soundcues.Count)
                    {
                        //Do nothing
                    }
                    else
                    {
                        if (loudest == null || loudest.sound < soundcues[i].sound)
                        {
                            loudest = soundcues[i];
                            addpoint = s;
                        }
                    }
                    i++;
                }
                //Send addpoint's result to loudestpoint
                loudestpoint(addpoint);

                //Delete that soundpoint and soundcue from the respective lists.
                if (i <= soundpoints.Count)
                {
                    Debug.Log("deleted"); 
                    soundpoints.Remove(soundpoints[i-1]);
                }
                if (i <= soundcues.Count)
                {
                    soundcues.Remove(soundcues[i-1]);
                }
            }
        }
        if (counter == 500)
        {
            soundcues.Add(sampleSoundcue);
            soundpoints.Add(sampleSoundPoint);
        } */
    } 

    private void createNewwaypoint()
    {

    }

    private void loudestpoint(Transform newwaypoint)
    {
        if (curr_waypoint != null || newwaypoint != null)
        {
            Vector3 targetV = newwaypoint.transform.position;
            navMeshAgent.SetDestination(targetV);
            patrol = false;
            //curr_waypoint.transform.position = waypoints[2].transform.position; 
        }
    }
}
