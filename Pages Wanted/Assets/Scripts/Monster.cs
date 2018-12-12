using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 

public class Monster : MonoBehaviour {
    // Public or Serialized Variables for Inspector -----------------
    public bool debug;
    [SerializeField] [Range(3.0F, 10.0F)] private float stunDelay = 5.0f;
    public float slowSpeed = 100f;
    public Image insignia1;
    public Image insignia2;
    public ParticleSystem senseParticles;
    public ParticleSystem hitParticles;

    // Private Variables ----------------------------------------------
    private NavMeshAgent navMeshAgent;  
    private float currentMovSpeed;
    private Transform target = null;
    private float collideDistance = 80f;
    private MonsterManager monstermanager;
    private int imgCounter = 50;
    private int imgnum = 0; 


    void Start()
    {
        insignia1.enabled = false;
        insignia2.enabled = false;
        if (debug) Debug.Log(PlayerPrefs.GetFloat("monsterbasespeed") + "  " + PlayerPrefs.GetFloat("monstersense"));
        checkprefs();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        monstermanager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>(); 
        ChangeHitParticleRadius(collideDistance);
    }

    void checkprefs()
    {
        //Eventually make max and min v
        if (PlayerPrefs.GetFloat("monsterbasespeed") == 0.0f || PlayerPrefs.GetFloat("monsterbasespeed") > 800f || PlayerPrefs.GetFloat("monsterbasespeed") < 400f)
        {
            PlayerPrefs.SetFloat("monsterspeed", 500f);
        }
    }
    
    void Update () {
        //TODO: move insignia to UIManager
        if (insignia1.enabled == true || insignia2.enabled == true) {
            imgnum++; 
            if (imgnum == imgCounter) {
                imgnum = 0;
                insignia1.enabled = false;
                insignia2.enabled = false;
            }
        }
        if (debug) Debug.Log("TARGET DURING UPDATE " + target.name);
        CheckReachedTarget();
	}

    public void LookAt(Vector3 t) {
        transform.LookAt(t);
        //TODO: make this animate a bit
    }

    public void Teleport(Transform destination) {
        navMeshAgent.Warp(destination.position);
    }

    public void SetTarget(Transform newTarget) {
        target = newTarget;
        GoToTarget();
    }

    public void SetSpeed(float newSpeed) {
        navMeshAgent.speed = newSpeed;
    }

    private void GoToTarget() {
        if (debug) Debug.Log("TARGET " + target.name);
        bool success = navMeshAgent.SetDestination(target.position);
        if (!success) {
            if (debug) Debug.Log("Could not reach waypoint, teleporting to waypoint");
            Teleport(target);
        } else if (debug) Debug.Log("Going to target successfully");
    }

    void CheckReachedTarget() {
        if (Vector3.Distance(transform.position, navMeshAgent.destination) < collideDistance) {
            if (debug) Debug.Log("Got to Target " + target.name);
            // Get a new target
            SetTarget(monstermanager.GetNewTarget());
        }
    }

// If player effect player health and then respawn
    public void HitPlayer(bool hitP1) {
        if (debug) Debug.Log("Hit Player!");
        GameObject.Find("Canvas").GetComponent<OverallUIManager>().DecreaseHealth();
        respawn();
        if (hitP1) {
            insignia1.enabled = true; 
        } else {
            insignia2.enabled = true;
        }
    }

    void respawn() {
        navMeshAgent.Warp(monstermanager.GetSpawnPoint().position);
        //EnterStateStun();
    }

    public void ChangeHitParticleColor(Color newColor) {
        var col = hitParticles.main.startColor;
        col.color = newColor;
    }

    public void ChangeHitParticleRadius(float newRadius) {
        var shape = senseParticles.shape;
        shape.radius = newRadius;
    }

    public void ChangeSenseConeRadius(float newRadius) {
        Debug.Log("newRadius " + newRadius);
        float coneHeight = 50.0f;
        float newAngle = (Mathf.Atan(newRadius/coneHeight)/(Mathf.PI/180));
        Debug.Log("newAngle " + newAngle);
        var shape = senseParticles.shape;
        shape.angle = newAngle;
        shape.radius = 0.0001f;
    }
}
