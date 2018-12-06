using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource audio;
    public GameObject[] dBoxes;
    public List<AudioClip> que = new List<AudioClip>();


    // Use this for initialization
    void Start () {
        audio = this.GetComponent<AudioSource>();
        dBoxes = GameObject.FindGameObjectsWithTag("Dialogue");
        audio.Play();
	}

    // Update is called once per frame
    void Update()
    {
        if(que != null && !dialoguePlaying())
        {
            audio.clip = que[0];
            que.RemoveAt(0);
            audio.Play();
        }
    }

    public bool dialoguePlaying()
    {
        //Checks to see if any dialogue boxes are playing sound
       foreach (GameObject d in dBoxes)
        {
            if(d.GetComponent<AudioSource>().isPlaying)
            {
                return true;
            }
        }
       //Checks if the intro clip is playing
       if(audio.isPlaying)
        {
            return true;
        }
       return false;
    }
}
