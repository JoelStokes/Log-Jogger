using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float yForce;
    public float xSpeed;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public Vector2 LaunchSpring(){
        anim.SetTrigger("Launch");

        return new Vector2(xSpeed, yForce);
    }
}
