using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverallUIManager : MonoBehaviour {

	public CharUIManager P1UI;
	public CharUIManager P2UI;
    public List<GameObject> PlayerHealth; 
	private int currentHealth = 4;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DecreaseHealth() {
		currentHealth -= 1;
		PlayerHealth[currentHealth].SetActive(false);
		if (currentHealth == 0) {
            Debug.Log("Game Over");
            SceneManager.LoadScene("GameOverScreen"); 
        }
	}
}
