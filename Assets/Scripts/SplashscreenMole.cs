using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashscreenMole : MonoBehaviour
{
    public SplashscreenController splashscreenController;
    private Rigidbody2D rigi;
    private Animator anim;
    private float moveSpeed = 5.5f;


    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        anim.SetTrigger("Start");
    }

    void FixedUpdate()
    {
        rigi.velocity = new Vector2(moveSpeed, 0);
    }

    //Map when mole passes to edge of screen & when trail finishes
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "End"){
            splashscreenController.StartTextAnim();
        }
    }
}
