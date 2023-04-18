using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool paused = false;

    void Start()
    {
        
    }

    public void ToggleMenu(){
        paused = !paused;

        if (paused){
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }
}
