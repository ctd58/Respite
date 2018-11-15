using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    private AudioSource audio;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
        audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
