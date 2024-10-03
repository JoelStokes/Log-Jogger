using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausedText;
    public SpriteRenderer coverSprite;
    public PlayerController playerController;
    public VolumeController volumeController;
    public RespawnMenu respawnMenu;

    public GameObject ResumeButton;
    public GameObject RetryButton;
    public GameObject HomeButton;

    private bool paused = false;
    private float previousTimeScale;
    private bool changing = false;

    void Start(){
        volumeController = GameObject.Find("MusicManager").GetComponent<VolumeController>();
        respawnMenu = GameObject.Find("UI").GetComponent<RespawnMenu>();
    }

    public void ToggleMenu(){
        if (!changing){
            paused = !paused;

            if (paused && !playerController.GetDead()){
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0;
                pausedText.SetActive(true);
                ResumeButton.SetActive(true);
                RetryButton.SetActive(true);
                HomeButton.SetActive(true);

                coverSprite.color = new Vector4(0,0,0,.4f);
                playerController.TogglePause(true);
                volumeController.Pause();
            } else {
                pausedText.SetActive(false);
                ResumeButton.SetActive(false);
                RetryButton.SetActive(false);
                HomeButton.SetActive(false);

                coverSprite.color = new Vector4(0,0,0,0);
                Time.timeScale = previousTimeScale;
                playerController.TogglePause(false);
                volumeController.Resume();
            }
        }
    }

    public void PauseReset(){
        if (!changing){
            Time.timeScale = previousTimeScale;
            respawnMenu.SetPaused();
            respawnMenu.RetryLevel();
            changing = true;            
        }

    }

    public void PauseMainMenu(){
        if (!changing){
            Time.timeScale = previousTimeScale;
            respawnMenu.SetPaused();
            respawnMenu.MainMenu();
            changing = true;            
        }
    }
}
