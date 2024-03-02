using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMachine : MonoBehaviour
{
    public GameObject machineBurstPrefab;
    public AudioClip[] machineSFX;

    //Have volume be set by game settings
    private float volume = .5f;

    void OnMouseDown(){
        PlayAudio(machineSFX, .1f);
        Instantiate(machineBurstPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void PlayAudio(AudioClip[] audioClips, float volumeModifier){    //Overload function for choosing random sound effect from array to play
        if (audioClips.Length != 0){
            int rand = Random.Range(0, audioClips.Length-1);

            AudioSource.PlayClipAtPoint(audioClips[rand], transform.position, volume + volumeModifier);
        }
    }
}