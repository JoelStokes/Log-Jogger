using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public bool isMusic = false;
    public float loopTime = 0;
    private AudioSource audioSource;
    private SaveManager saveManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        SetVolume();
    }

    void Update(){
        if (!audioSource.isPlaying){
            if (isMusic && loopTime != 0){
                audioSource.time = loopTime;
                audioSource.Play();
            }
        }
    }

    public void ManualChange(){ //Called from Title Screen where changes can happen mid-scene
        SetVolume();
    }

    private void SetVolume(){
        if (isMusic){
            audioSource.volume = saveManager.state.musicVolume;
        } else {
            audioSource.volume = saveManager.state.sfxVolume;
        }
    }
}
