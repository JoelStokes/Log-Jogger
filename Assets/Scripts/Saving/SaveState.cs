using System.Collections.Generic;

public class SaveState
{
    public int highScore = 0;
    public List<int> lastScores = new List<int>();

    public int wormCount = 0;

    public int skinOwned = 0;
    public int currentSkin = 0;

    //Add section to check if each skin has been unlocked, along with which skin is currently equipped

    public float sfxVolume = 1f;
    public float musicVolume = 1f;
    public bool secretBeaten = false;
}
