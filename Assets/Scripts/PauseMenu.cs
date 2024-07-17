using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausedText;
    public SpriteRenderer coverSprite;
    public PlayerController playerController;
    public VolumeController volumeController;

    private bool paused = false;
    private float previousTimeScale;

    void Start(){
        volumeController = GameObject.Find("MusicManager").GetComponent<VolumeController>();
    }

    public void ToggleMenu(){
        paused = !paused;

        if (paused){
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            pausedText.SetActive(true);
            coverSprite.color = new Vector4(0,0,0,.25f);
            playerController.TogglePause(true);
            volumeController.Pause();
        } else {
            pausedText.SetActive(false);
            coverSprite.color = new Vector4(0,0,0,0);
            Time.timeScale = previousTimeScale;
            playerController.TogglePause(false);
            volumeController.Resume();
        }
    }
}
