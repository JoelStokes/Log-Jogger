using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public bool isMusic = false;
    public float loopTime = 0;
    public float soundModifier = 1;

    private float pausePitch = .85f;
    private bool isDead = false;
    private float volumeDecrease = .3f;
    private float pitchDecrease = .3f;

    private AudioSource audioSource;
    private SaveManager saveManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        SetVolume();
    }

    void Update(){
        if (!audioSource.isPlaying && isMusic && loopTime != 0){
            audioSource.time = loopTime;
            audioSource.Play();
        }

        if (isDead){
            if (audioSource.volume > 0 && audioSource.pitch > 0){
                audioSource.volume -= (volumeDecrease * Time.deltaTime);
                audioSource.pitch -= (pitchDecrease * Time.deltaTime);
            } else {
                audioSource.volume = 0;
            }
        }
    }

    public void ManualChange(){ //Called from Title Screen where changes can happen mid-scene
        SetVolume();
    }

    private void SetVolume(){
        if (isMusic){
            audioSource.volume = saveManager.state.musicVolume * soundModifier;
        } else {
            audioSource.volume = saveManager.state.sfxVolume * soundModifier;
        }
    }

    public void Pause(){
        audioSource.pitch = pausePitch;
    }

    public void Resume(){
        audioSource.pitch = 1;
    }

    public void PlayerDied(){
        isDead = true;
    }

    //Used to play High Score jingle after run ends
    public void ChangeMusic(AudioClip newAudio){
        audioSource.clip = newAudio;
        audioSource.volume = saveManager.state.musicVolume * soundModifier;
        audioSource.pitch = 1;
        audioSource.time = 0;
        audioSource.Play();

        //Prevent looping & pitch changes
        isMusic = false;
        isDead = false;
    }
}
