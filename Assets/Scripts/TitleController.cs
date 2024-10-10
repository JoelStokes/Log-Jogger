using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using UnityEngine.UI;

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
    public AudioClip secretSFX;
    private SaveManager saveManager;

    //Secret Password Handling
    private double currentPassword;
    private double correctPassword = 123211;    //Reversed from click order
    private double currentDigit = 0;
    private bool passwordSuccess = false;
    public TextMeshProUGUI tutorialButtonText;
    public Button tutorialButton;
    public SpriteRenderer tutorialIcon;
    public Sprite iconReplacement;

    void Start(){
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
    }

    void Update(){
        if (!passwordSuccess){
            if (currentPassword == correctPassword){
                passwordSuccess = true;
                PlayAudio(secretSFX);
                tutorialButtonText.text = "100m";

                ColorBlock colors = tutorialButton.colors;
                colors.normalColor = new Vector4(1,.4f,.4f,1);
                colors.highlightedColor = new Vector4(1,.5f,.5f,1);
                colors.pressedColor = new Vector4(1,.3f,.3f,1);
                colors.selectedColor = new Vector4(1,.3f,.3f,1);
                colors.disabledColor = new Vector4(1,.1f,.1f,1);

                tutorialButton.colors = colors;
                tutorialIcon.sprite = iconReplacement;
            } 
        }
    }

    public void Play(){
        if (!inSettings && !inQuit){
            PlayAudio(playSFX);
            CreateTransition("Level");
        }
    }

    public void Tutorial(){
        if (!inSettings && !inQuit){
            if (passwordSuccess){
                PlayAudio(playSFX);
                CreateTransition("100m");
            } else {
                PlayAudio(playSFX);
                CreateTransition("Tutorial");
            }
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
        if (!inSettings && !inQuit){
            PlayAudio(playSFX);
            CreateTransition("Shop");
        }
    }

    public void Home(){
        PlayAudio(backSFX);
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

    public void CodeClick(int number){
        if (currentPassword < correctPassword){
            currentPassword += (number * (Math.Pow(10,currentDigit)));
            currentDigit++;
        }
    }
}
