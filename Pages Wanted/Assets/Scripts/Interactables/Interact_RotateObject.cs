using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_RotateObject : Interactables {

    #region Public Variables
    public List<Switch_Target> targets;
    public Vector3 targetAngles;
    public float smooth = 1f;
    #endregion

    // Private Variables ---------------------------------------------
    #region Private Variables
    bool isActivated = false;
    bool doneRotate = false;
    #endregion

    new void Start()
    {
        base.Start();
        SetSprite(Interact_Icon_Type.PICKUP);
        targetAngles = transform.eulerAngles + 180f * Vector3.forward;
    }

    public void FixedUpdate()
    {
        if (isActivated)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, smooth * Time.deltaTime);
            if(this.transform.eulerAngles.z >= 180)
            {
                this.transform.Rotate(0, 0, 180);
                doneRotate = !doneRotate;
                isActivated = !isActivated;
                SetSprite(Interact_Icon_Type.NORMAL);
                if(targets != null)
                {
                    targets[0].onSwitchActivate();
                }
            }
        }
    }

    public override void onInteract(Character_Inventory inv)
    {
        base.onInteract(inv);
        if(!isActivated && !doneRotate)
        {
            isActivated = !isActivated;

        }

       

    }
}
