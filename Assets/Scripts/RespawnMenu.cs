using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnMenu : MonoBehaviour
{
    public GameObject transitionPrefab;
    public Animator deathAnimator;

    private float transitionX = 12;
    private float transitionXEnd = -9;

    public void PlayerDied(){
        deathAnimator.SetTrigger("Dead");
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

    //Should scaling be applied to the transition based on camera sizing for consistency?
    private void CreateTransition (string scene){
        GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transform.position.x + transitionX, transform.position.y, 0), Quaternion.identity);
        TransitionObj.GetComponent<Transition>().SetValues(scene, transitionXEnd);        
    }
}
