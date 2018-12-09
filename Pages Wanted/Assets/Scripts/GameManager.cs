using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

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

        StartCoroutine("SetControls");
	}


    public IEnumerable SetControls() {
        // first wait a second for the game controllers to be recognized
        yield return new WaitForSeconds(1);
        // check if no controllers plugged in
        if (ReInput.controllers.joystickCount == 0) {
            Debug.Log("ONLY ONE CONTROLLER: Setting both players to use keyboard");
            Player p1 = ReInput.players.GetPlayer(0);
            p1.controllers.maps.LoadMap<KeyboardMap>(0, 1, 0, true);
            p1.controllers.maps.LoadMap<KeyboardMap>(0, 3, 0, true);
            // if we want mouse, then exchange previous line with:
            // p1.controllers.maps.LoadMap<MouseMap>(0, 1, 0, true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
