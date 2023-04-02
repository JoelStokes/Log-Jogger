using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public GameObject[] SwitchBlocks;

    private Animator anim;

    void Start(){
        anim = GetComponent<Animator>();
    }

    public void Pressed(){
        anim.SetTrigger("Pressed");

        for (int i=0; i<SwitchBlocks.Length; i++){
            SwitchBlocks[i].transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
