using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public GameObject[] SwitchBlocksGrow;   //Grow start dotted & become filled, Shrink start filled & become dotted
    public GameObject[] SwitchBlocksShrink;
    private bool pressed = false;
    private Animator anim;

    void Start(){
        anim = GetComponent<Animator>();
    }

    public void Pressed(){
        if (!pressed){
            anim.SetTrigger("Pressed");

            for (int i=0; i<SwitchBlocksGrow.Length; i++){
                SwitchBlocksGrow[i].transform.GetChild(0).gameObject.SetActive(true);
                SwitchBlocksGrow[i].GetComponent<Animator>().SetTrigger("Grow");
            }

            for (int i=0; i<SwitchBlocksShrink.Length; i++){
                SwitchBlocksShrink[i].GetComponent<Animator>().SetTrigger("Shrink");
                Destroy(SwitchBlocksShrink[i].transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>());
            }

            pressed = true;
        }
    }
}
