using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Journal : Interactables {

    
    public AudioClip Lidia;
    public AudioClip Aleksy;
    public AudioManager am;
    private int select;

    // Setup Methods -------------------------------------------------
    new void Start()
    {
        base.Start();
        SetSprite(Interact_Icon_Type.PICKUP);
        am = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Private Methods -------------------------------------------------
    // Add this key to the list of keys in inventory
    public override void onInteract(Character_Inventory inv)
    {
        select = Random.Range(1, 3);
        if(select == 1)
        {
            am.que.Add(Lidia);
        }
        else
        {
            am.que.Add(Aleksy);
        }
              
        Destroy(this.gameObject);
    }
}
