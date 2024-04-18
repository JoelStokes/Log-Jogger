using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When scene is starting, call PlayerPrefs to see which skin to apply
public class SkinLoader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Load PlayerPref
        //Apply proper skin
    }
}
