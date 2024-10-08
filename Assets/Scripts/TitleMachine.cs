using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMachine : MonoBehaviour
{
    public GameObject machineBurstPrefab;
    public AudioClip machineSFX;
    public AudioClip machinePointsSFX;

    //Have volume be set by game settings
    private float volume = .5f;
    private float machineSFXMod = .35f;
    private float pointSFXMod = .9f;

    void Start(){
        volume = GameObject.Find("SaveManager").GetComponent<SaveManager>().state.sfxVolume;
    }

    void OnMouseDown(){
        PlayAudio();
        Instantiate(machineBurstPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void PlayAudio(){
        AudioSource.PlayClipAtPoint(machineSFX, Camera.main.transform.position, volume * machineSFXMod);
        AudioSource.PlayClipAtPoint(machinePointsSFX, Camera.main.transform.position, volume * pointSFXMod);
    }
}