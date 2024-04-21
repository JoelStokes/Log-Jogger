using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUpdate : MonoBehaviour
{
    public bool isMusic = false;

    private SaveManager saveManager;
    private Slider slider;

    void Start()
    {
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        slider = GetComponent<Slider>();

        if (isMusic){
            slider.value = saveManager.state.musicVolume;
        } else {
            slider.value = saveManager.state.sfxVolume;
        }
    }

    public void VolumeChanged(){
        if (isMusic){
            saveManager.state.musicVolume = slider.value;
        } else {
            saveManager.state.sfxVolume = slider.value;
        }

        saveManager.Save();
    }
}
