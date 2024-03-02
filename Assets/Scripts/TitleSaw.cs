using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSaw : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject Saw;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Title", true);
    }

    void Update()
    {
        Saw.transform.Rotate(new Vector3(0,0,rotationSpeed * Time.deltaTime), Space.World);
    }

    void OnMouseDown(){
        anim.SetTrigger("Clicked");
    }
}
