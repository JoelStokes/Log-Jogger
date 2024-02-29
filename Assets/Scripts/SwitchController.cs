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

    void OnDisable(){   //Reset to default position for later use
        anim.SetBool("Pressed", false);
        pressed = false;

        for (int i=0; i<SwitchBlocksGrow.Length; i++){
            SwitchBlocksGrow[i].GetComponent<Animator>().SetTrigger("Shrink");
            SwitchBlocksGrow[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i=0; i<SwitchBlocksShrink.Length; i++){
            SwitchBlocksShrink[i].GetComponent<Animator>().SetTrigger("Grow");
            SwitchBlocksShrink[i].transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void Pressed(){
        if (!pressed){
            anim.SetBool("Pressed", true);

            for (int i=0; i<SwitchBlocksGrow.Length; i++){
                SwitchBlocksGrow[i].transform.GetChild(0).gameObject.SetActive(true);
                SwitchBlocksGrow[i].GetComponent<Animator>().SetTrigger("Grow");
            }

            for (int i=0; i<SwitchBlocksShrink.Length; i++){
                SwitchBlocksShrink[i].GetComponent<Animator>().SetTrigger("Shrink");
                SwitchBlocksShrink[i].transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }

            pressed = true;
        }
    }
}
