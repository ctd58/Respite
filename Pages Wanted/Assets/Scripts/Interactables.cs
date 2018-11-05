using UnityEngine;

public abstract class Interactables : MonoBehaviour {


    // Public or Serialized Variables for Inspector -----------------
    #region Public Variables
    // TODO: refactor to aggregate MakesSound script
    //[SerializeField] protected bool makesSound = false;
    #endregion

    // Private Variables ---------------------------------------------
    #region Private Variables
    protected Sprite spriteIcon;
    #endregion
    private Interact_MakeNoise noise; //Interactables has a makeNoise 
    private Sprite sprite;

    // Public Methods ------------------------------------------------
    #region Public Methods
    public virtual void onInteract(Character_Inventory inv) {}

	public Sprite GetSprite() {
		return spriteIcon;
	}

	public void SetSprite(Interact_Icon_Type type) {
		spriteIcon = Interact_Icon.GetSprite(type);
	}
	#endregion
    //sprite Get/Setters
    public Sprite getsprite(){
        return sprite; 
    }
    public void setsprite(Sprite value){
        sprite = value;
    }
    //onInteract version, can be overloaded
    public void onInteract(){

    }

}
