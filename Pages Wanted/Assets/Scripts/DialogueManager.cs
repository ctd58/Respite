using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip Lidia;
    public AudioClip Aleksy;
    public string collidingPlayer;
    public bool dontPlay = false;
    
    public AudioManager am;

    // Use this for initialization
    void Start()
    {
        audio = this.GetComponent<AudioSource>();
        am = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "P1" && !dontPlay && !am.dialoguePlaying())
        {
            dontPlay = true;
            collidingPlayer = "P1";
            audio.clip = Aleksy;
            audio.Play();
        }
        else if(other.gameObject.tag == "P1" && !dontPlay && am.dialoguePlaying())
        {
            dontPlay = true;
            collidingPlayer = "P1";
            am.que.Add(Aleksy);
        }
        else if (other.gameObject.tag == "P2" && !dontPlay && !am.dialoguePlaying())
        {
            dontPlay = true;
            collidingPlayer = "P2";
            audio.clip = Lidia;
            audio.Play();
        }
        else if (other.gameObject.tag == "P2" && !dontPlay && am.dialoguePlaying())
        {
            dontPlay = true;
            collidingPlayer = "P1";
            am.que.Add(Lidia);
        }

    }
}
