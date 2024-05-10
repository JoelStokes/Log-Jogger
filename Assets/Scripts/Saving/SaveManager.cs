using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveState state;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();

        Debug.Log(Serializer.Serialize<SaveState>(state));
    }

    //Save the whole state of this saveState script to the player pref
    public void Save()
    {
        PlayerPrefs.SetString("save", Serializer.Serialize<SaveState>(state));
    }

    // Load previous saved state from the player prefs
    public void Load()
    {
        if(PlayerPrefs.HasKey("save")){
            state = Serializer.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        } else {
            state = new SaveState();
            Save();
            Debug.Log("No save file found, creating a new one!");
        }
    }

    //Check if the bit is set, if so skin is owned
    public bool IsSkinOwned(int index){
        return (state.skinOwned & (1 << index)) != 0;
    }

    //Unlock a skin in the "skinOwned" int
    public void UnlockSkin(int index){
        state.skinOwned |= 1 << index;
    }
}
