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

    //Audio Handling
    private float volume;
    public AudioSource audioSource;
    public float soundModifier;
    public AudioClip playSFX;
    public AudioClip backSFX;
    public AudioClip settingsSFX;
    public AudioClip quitSFX;
    private SaveManager saveManager;

    void Start(){
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
    }

    public void Play(){
        if (!inSettings && !inQuit){
            PlayAudio(playSFX);
            CreateTransition("Level");
        }
    }

    public void Tutorial(){
        if (!inSettings && !inQuit){
            PlayAudio(playSFX);
            CreateTransition("Tutorial");
        }
    }

    public void HighScore(){
        if (!inSettings && !inQuit){
            PlayAudio(playSFX);
            CreateTransition("HighScore");
        }    
    }

    public void Settings(){
        PlayAudio(settingsSFX);
        inSettings = true;
        anim.SetBool("Settings", inSettings);

        if (inQuit){
            inQuit = false;
            anim.SetBool("Quit", inQuit);
        }
    }

    public void Quit(){
        PlayAudio(quitSFX);
        inQuit = true;
        anim.SetBool("Quit", inQuit);

        if (inSettings){
            inSettings = false;
            anim.SetBool("Settings", inSettings);
        }
    }

    public void Return(){
        PlayAudio(backSFX);
        inQuit = false;
        inSettings = false;

        anim.SetBool("Settings", inSettings);
        anim.SetBool("Quit", inQuit);
    }

    public void ConfirmQuit(){
        Application.Quit();
    }

    public void Shop(){
        PlayAudio(playSFX);
        if (!inSettings && !inQuit){
            CreateTransition("Shop");
        }
    }

    public void Home(){
        //Used in Shop menu to return back to Title
        CreateTransition("Title");
    }

    private void CreateTransition (string scene){
        GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transitionX, 0, 0), Quaternion.identity);
        TransitionObj.GetComponent<Transition>().SetValues(scene, transitionXEnd, -45);        
    }

    public void PlayAudio(AudioClip audioClip){
        audioSource.volume = saveManager.state.sfxVolume * soundModifier;
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
