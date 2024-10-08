using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnMenu : MonoBehaviour
{
    public GameObject transitionPrefab;
    public Animator deathAnimator;
    public GameObject burstPrefab;
    public Animator flagAnimator;
    public AudioClip highScoreMusic;

    private float burstCounter = 0;
    private float burstLim = 1.75f;
    private bool burstFinished = false;

    private float transitionX = 8;
    private float transitionXEnd = -5;
    private float pauseTransitionX = 12;
    private float pauseTransitionXEnd = -9;

    private bool isHighScore = false;
    private Vector3 angelPos;
    private VolumeController volumeController;
    private bool changing = false;
    private bool paused = false;

    void Start(){
        volumeController = GameObject.Find("MusicManager").GetComponent<VolumeController>();
    }

    void Update(){
        if (isHighScore && !burstFinished){
            if (burstCounter >= burstLim){
                GameObject Burst = GameObject.Instantiate(burstPrefab, new Vector3(angelPos.x, angelPos.y, angelPos.z), Quaternion.identity);
                Burst.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                burstFinished = true;
                volumeController.ChangeMusic(highScoreMusic);
            } else {
                burstCounter += Time.deltaTime;
            }
        }
    }

    public void PlayerDied(){
        deathAnimator.SetTrigger("Dead");
        if (isHighScore){
            flagAnimator.SetTrigger("HighScore");
        }
    }

    public void RetryLevel(){
        if (!changing){
            if (!paused){
                CreateTransition(SceneManager.GetActiveScene().name);
                changing = true;
            } else {
                CreatePauseTransition(SceneManager.GetActiveScene().name);
                changing = true;
            }
        }
    }

    public void HighScores(){
        if (!changing){
            CreateTransition("HighScore");
            changing = true;
        }
    }

    public void MainMenu(){
        if (!changing){
            if (!paused){
                CreateTransition("Title");
                changing = true;
            } else {
                CreatePauseTransition("Title");
                changing = true;
            }
        }
    }

    public void NewHighScore(Vector3 newPos){
        isHighScore = true;
        angelPos = newPos;
    }

    public void SetPaused(){
        paused = true;
    }

    private void CreateTransition (string scene){
        GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transform.position.x + transitionX, transform.position.y, 0), Quaternion.identity);
        TransitionObj.GetComponent<Transition>().SetValues(scene, transform.position.x + transitionXEnd, -25, .45f);        
    }

    private void CreatePauseTransition (string scene){
        GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transform.position.x + pauseTransitionX, transform.position.y, 0), Quaternion.identity);
        TransitionObj.GetComponent<Transition>().SetValues(scene, transform.position.x + pauseTransitionXEnd, -40, .85f);
    }
}
