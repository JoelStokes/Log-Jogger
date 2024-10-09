using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRGB : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SaveManager saveManager;
    private bool isRGB = false;
    private float redValue = 0;
    private float greenValue = 0;
    private float blueValue = 1;
    private float colorChangeSpeed = 1f;

    void Start()
    {
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (saveManager.state.currentSkin == 12){
            isRGB = true;
        } else {
            isRGB = false;
            spriteRenderer.color = new Vector4(1,1,1,1);
        }

        if (isRGB){
            if (blueValue >= 1 && greenValue < 1 && redValue <= 0){
                greenValue += colorChangeSpeed * Time.deltaTime;
            } else if (greenValue >= 1 && blueValue > 0){
                blueValue -= colorChangeSpeed * Time.deltaTime;
            } else if (greenValue >= 1 && redValue < 1){
                redValue += colorChangeSpeed * Time.deltaTime;
            } else if (redValue >= 1 && greenValue > 0){
                greenValue -= colorChangeSpeed * Time.deltaTime;
            } else if (redValue >= 1 && blueValue < 1) {
                blueValue += colorChangeSpeed * Time.deltaTime;
            } else if (blueValue >= 1 && redValue > 0){
                redValue -= colorChangeSpeed * Time.deltaTime;
            }

            spriteRenderer.color = new Vector4(redValue, greenValue, blueValue, 1);
        }
    }
}
