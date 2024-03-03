using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausedText;
    public SpriteRenderer coverSprite;
    private bool paused = false;
    private float previousTimeScale;

    public void ToggleMenu(){
        paused = !paused;

        if (paused){
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            pausedText.SetActive(true);
            coverSprite.color = new Vector4(0,0,0,.25f);
        } else {
            pausedText.SetActive(false);
            coverSprite.color = new Vector4(0,0,0,0);
            Time.timeScale = previousTimeScale;
        }
    }
}
