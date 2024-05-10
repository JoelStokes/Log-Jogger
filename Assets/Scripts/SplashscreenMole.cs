using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashscreenMole : MonoBehaviour
{
    public bool shop = false;
    public SplashscreenController splashscreenController;
    private Rigidbody2D rigi;
    private Animator anim;
    
    public float moveSpeed = 5.5f;


    void Start()
    {
        //Load mole skin

        rigi = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        anim.SetTrigger("Start");

        if (shop){
            anim.speed = .5f;
        }
    }

    void FixedUpdate()
    {
        if (!shop){
            rigi.velocity = new Vector2(moveSpeed, 0);
        }
    }

    //Map when mole passes to edge of screen & when trail finishes
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "End"){
            splashscreenController.StartTextAnim();
        }
    }

    public void ChangeSkin(){
        //Call load again, shop button was pressed
    }
}
