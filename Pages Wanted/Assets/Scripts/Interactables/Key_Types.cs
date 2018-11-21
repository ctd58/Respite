public enum KeyTypes {
    /// <summary>
	/// Public enum, controls dropdowns for locks and keys
	/// To add a new key, add a new color to the enum
    /// </summary>

	GOLD,
	IRON,
	BRONZE,
    HALLWAY01,
    RUBY,
    SAPPHIRE,
    EMERALD,
    AMETHYST,
        
};

public class Key_Obj {
    /// <summary>
	/// Public object to help organize data about each key
	/// Could eventually hold sprite/model/texture data
    /// </summary>
	public KeyTypes type;
	public int keyId;
}