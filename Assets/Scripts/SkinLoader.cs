using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When scene is starting, call PlayerPrefs to see which skin to apply
public class SkinLoader : MonoBehaviour
{
    public Sprite[] Skins;
    public SpriteRenderer spriteRenderer;

    private SaveManager saveManager;

    /*
    0 Default
    1 Blue
    2 Green
    3 Pink
    4 Purple
    5 Orange
    6 Flannel
    7 Robot / Suit
    8 Space
    9 Naked
    10 Silver
    11 Gold
    */

    void Start()
    {
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        UpdateSkin();
    }

    public void UpdateSkin(){
        spriteRenderer.sprite = Skins[saveManager.state.currentSkin];
    }
}
