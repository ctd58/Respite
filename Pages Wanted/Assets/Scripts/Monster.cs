using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 

public class Monster : MonoBehaviour {
    // Public or Serialized Variables for Inspector -----------------
    [SerializeField] [Range(0.0F, 3.5F)] private float chaseTime = 25.0f;
    [SerializeField] [Range(300F, 800F)] private float baseMoveSpeed = 400f;
    [SerializeField] [Range(3.0F, 10.0F)] private float stunDelay = 5.0f;
    [SerializeField] [Range(10.0F, 20.0F)] private float rotSpeed = 15.0f;
    [SerializeField] [Range(0.0f, 1.0f)] private float fallOffStrength = 0.01f;
    public float slowSpeed = 100f;
    public Image insignia1;
    public Image insignia2; 
    public enum STATE { WANDER, INSPECT, ATTACK, STUNNED }
    // Private Variables ----------------------------------------------
    private List<Transform> wayPoints = new List<Transform>();
    private int imgCounter = 50;
    private int imgnum = 0; 
    private Transform currentWaypoint;
    private int i = 0;
    private int counter = 0;
    private bool madeWaypoint;
    private NavMeshAgent navMeshAgent;     
    private float currentMovSpeed;
    private Transform target = null;
    private GameObject[] soundObjects;
    private List<GameObject> players = new List<GameObject>();
    private float senseDistance = 8000f; 
    private STATE _currentState;
    private MonsterManager monstermanager;
    

    void Start()
    {
        insignia1.enabled = false;
        insignia2.enabled = false;
        /*
        GameObject other = GameObject.Find("TitleScreenNav");
        TitleScreenNav titleScreenNav = other.GetComponent<TitleScreenNav>();
        if (titleScreenNav != null)
        {
            baseMoveSpeed = titleScreenNav.monsterBaseSpeed.value;
            sensePlayerDistance = titleScreenNav.monsterSense.value;
        }
        */
        Debug.Log(PlayerPrefs.GetFloat("monsterbasespeed") + "  " + PlayerPrefs.GetFloat("monstersense"));
        checkprefs();
        //baseMoveSpeed = PlayerPrefs.GetFloat("monsterbasespeed");
        baseMoveSpeed = 400;
        //sensePlayerDistance = PlayerPrefs.GetFloat("monstersense"); 
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        // gameoverScreen.SetActive(false); 
        //TODO: make it search out objects with component<Sound>;
        //EnterStateWander ();
        monstermanager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>(); 
    }

    void checkprefs()
    {
        //Eventually make max and min v
        if (PlayerPrefs.GetFloat("monsterbasespeed") == 0.0f || PlayerPrefs.GetFloat("monsterbasespeed") > 800f || PlayerPrefs.GetFloat("monsterbasespeed") < 400f)
        {
            PlayerPrefs.SetFloat("monsterspeed", 500f);
        }
        /*
        if (PlayerPrefs.GetFloat("monstersense") == 0.0f || PlayerPrefs.GetFloat("monstersense") < 300f || PlayerPrefs.GetFloat("monstersense") > 800f)
        {
            PlayerPrefs.SetFloat("monstersense", 600f);
        }
        */
    }

    public STATE GetState() {
        return _currentState; 
    }
    
    void Update () {
        Debug.Log(_currentState);
        switch (_currentState) {
		case STATE.WANDER:
			UpdateWander ();
			break;
		case STATE.INSPECT:
			UpdateInspect ();
			break;
		case STATE.ATTACK:
			UpdateAttack ();
			break;
		case STATE.STUNNED:
			UpdateStun();
			break;
		}
        if (insignia1.enabled == true || insignia2.enabled == true) {
            imgnum++; 
            if (imgnum == imgCounter) {
                imgnum = 0;
                insignia1.enabled = false;
                insignia2.enabled = false;
            }
        }
	}

    // WANDER STATE ---------------------------------------------------------------------

    // code to setup wandering
    public void TriggerWander(List<Transform> newWaypoints) {
        wayPoints = newWaypoints;
        i = 0;
        EnterStateWander();
    }
    
    private void EnterStateWander() {
        currentMovSpeed = baseMoveSpeed; 
		_currentState = STATE.WANDER;
        currentWaypoint = wayPoints[i];
        madeWaypoint = true; 
	}

	private void UpdateWander() {
        counter++;
        //Debug.Log(counter); 
        if (counter > 400) {
            counter = 0;
           // Debug.Log("Made waypoint");
            madeWaypoint = true; 
        } else {
            if (currentWaypoint != null && madeWaypoint == true) {
                Vector3 targetV = currentWaypoint.transform.position;
                target = currentWaypoint.transform;
                //Need to make this based on speed, so the idea is maybe a while loop
                //where we tell the demon to move to a certain distance with base speed, and
                //if it no longer needs to move there because it is there, set the next waypoint.
                //navMeshAgent.speed = 1000;
                navMeshAgent.SetDestination(targetV);
                madeWaypoint = false;
                i = (i + 1) % wayPoints.Count;
             //   Debug.Log(i); 
            }
            currentWaypoint = wayPoints[i]; 
        }
        if (monstermanager.DetectPlayer()) { EnterStateAttack(); }
	}



    // INSPECT STATE ---------------------------------------------------------------------
    // public void CreateNewWaypoint(){
    //     //Update function to do smart pathing. 
    //     if (target != null){
    //         wayPoints.Add(target);
    //     }
    // }

    // public void DeleteNewWaypoint(Transform badwaypoint){
    //     if (wayPoints.Contains(badwaypoint)){
    //         //tell that waypoint to die
    //         wayPoints.Remove(badwaypoint);
    //     }
    // }

    public void TriggerInspect() {
        EnterStateInspect();
    }

    private void EnterStateInspect() {
		_currentState = STATE.INSPECT;
	}

	private void UpdateInspect() {
        //Debug.Log(target.name);
        //Increases speed if it has been chasing for a multiple of 2.5 seconds
        // chaseTime += Time.deltaTime;
        // if (chaseTime / 2.5f > 1.0f) {
        //     chaseTime -= 2.5f;
        //     currentMovSpeed += 7.5f;
        //     if(currentMovSpeed> 800f) {
        //         currentMovSpeed = 800f;
        //     }
        // }
        target = monstermanager.getTarget(); 
        FollowSound();
        Vector3 targetV = target.position;
        transform.LookAt(target); 
        navMeshAgent.SetDestination(targetV);
        //Check if it is on top of the targets position
        if(Vector3.Distance(target.position,this.transform.position) < senseDistance) {
            Debug.Log("HERE");
            EnterStateWander();
        }
        if (monstermanager.DetectPlayer()) { EnterStateAttack(); }
	}

    //makes it go towards sound
    void FollowSound() {
        transform.LookAt(target);
        //TODO: Make this use navmesh
        //transform.position += transform.forward * (currentMovSpeed * slowSpeed) * Time.deltaTime;
    }

    // ATTACK STATE ---------------------------------------------------------------------

    public void TriggerAttack() {
        EnterStateAttack(); 
    }


	private void EnterStateAttack() {
		_currentState = STATE.ATTACK;
        UpdateAttack(); 
	}

	private void UpdateAttack() {
        //Debug.Log("Attack!"); 
        if (monstermanager.DetectPlayer() == true) {
            target = monstermanager.getTarget();
            Vector3 targetV = target.position; 
            navMeshAgent.SetDestination(targetV);
            //UpdateAttack(); 
            //EnterStateWander(); 
        } else {
            EnterStateWander(); 
        }
    }

    // STUN STATE ---------------------------------------------------------------------

    // Player ability script should call this function
	public void Stun() {
		EnterStateStun ();
	}

	private void EnterStateStun() {
		_currentState = STATE.STUNNED;
		StartCoroutine("stun");
	}

	private void UpdateStun() {
	} // do nothing here, the stun coroutine will set state back to wander

    IEnumerator stun()
    {
        yield return new WaitForSecondsRealtime(stunDelay);
        EnterStateWander();
    }

    public void Teleport(Transform destination) {
        navMeshAgent.Warp(destination.position);
    }

    // If they catch the player ---------------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "P1" || other.gameObject.tag == "P2")
        {
            target = null;
            GameObject.Find("Canvas").GetComponent<OverallUIManager>().DecreaseHealth();
            respawn();
        }
        if (other.gameObject.tag == "P1") {
            insignia1.enabled = true; 
        }
        if (other.gameObject.tag == "P2") {
            insignia2.enabled = true;
        }
    }

    void respawn() {
        navMeshAgent.Warp(monstermanager.GetSpawnPoint().position);
        EnterStateStun();
    }
}
