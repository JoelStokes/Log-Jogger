using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnMenu : MonoBehaviour
{
    public GameObject transitionPrefab;
    public Animator deathAnimator;
    public GameObject burstPrefab;
    public Animator flagAnimator;

    private float burstCounter = 0;
    private float burstLim = 1.75f;
    private bool burstFinished = false;

    private float transitionX = 12;
    private float transitionXEnd = -9;
    private bool isHighScore = false;
    private Vector3 angelPos;

    void Update(){
        if (isHighScore && !burstFinished){
            Debug.Log(burstCounter);
            if (burstCounter >= burstLim){
                GameObject Burst = GameObject.Instantiate(burstPrefab, new Vector3(angelPos.x, angelPos.y, angelPos.z), Quaternion.identity);
                Burst.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                burstFinished = true;
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
        CreateTransition("Level");
    }

    public void HighScores(){
        CreateTransition("HighScore");
    }

    public void MainMenu(){
        CreateTransition("Title");
    }

    public void NewHighScore(Vector3 newPos){
        isHighScore = true;
        angelPos = newPos;
    }

    private void CreateTransition (string scene){
        GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transform.position.x + transitionX, transform.position.y, 0), Quaternion.identity);
        TransitionObj.GetComponent<Transition>().SetValues(scene, transform.position.x + transitionXEnd);        
    }
}
