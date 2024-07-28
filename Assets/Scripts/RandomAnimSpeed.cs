using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimSpeed : MonoBehaviour
{
    public float min;
    public float max;
    public int animStates;

    //private int nextAnimState = 0;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        ChangeState();
    }

    public void ChangeState(){
        int randomState = Random.Range(1, animStates);
        anim.SetInteger("NextState", randomState);

        float randomSpeed = Random.Range(min, max);
        anim.speed = randomSpeed;
    }
}
