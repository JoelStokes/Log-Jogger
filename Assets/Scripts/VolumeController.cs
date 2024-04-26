using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public bool isMusic = false;

    private AudioSource audioSource;
    private SaveManager saveManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        SetVolume();
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
