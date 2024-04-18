using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public GameObject transitionPrefab;
    public Animator anim;
    public SpriteRenderer moleSprite;

    private float transitionX = 12;
    private float transitionXEnd = -9;

    //Used to make sure user didn't try to quickly select 2 different buttons during UI animation
    private bool inSettings = false;
    private bool inQuit = false;

    public void Play(){
        if (!inSettings && !inQuit){
            CreateTransition("Level");
        }
    }

    public void Tutorial(){
        if (!inSettings && !inQuit){
            CreateTransition("Tutorial");
        }
    }

    public void HighScore(){
        if (!inSettings && !inQuit){
            CreateTransition("HighScore");
        }    
    }

    public void Settings(){
        inSettings = true;
        anim.SetBool("Settings", inSettings);

        if (inQuit){
            inQuit = false;
            anim.SetBool("Quit", inQuit);
        }
    }

    public void Quit(){
        inQuit = true;
        anim.SetBool("Quit", inQuit);

        if (inSettings){
            inSettings = false;
            anim.SetBool("Settings", inSettings);
        }
    }

    public void Return(){
        inQuit = false;
        inSettings = false;

        anim.SetBool("Settings", inSettings);
        anim.SetBool("Quit", inQuit);
    }

    public void ConfirmQuit(){
        Application.Quit();
    }

    public void Shop(){
        if (!inSettings && !inQuit){
            CreateTransition("Shop");
        }
    }

    private void CreateTransition (string scene){
        GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transitionX, 0, 0), Quaternion.identity);
        TransitionObj.GetComponent<Transition>().SetValues(scene, transitionXEnd);        
    }
}
