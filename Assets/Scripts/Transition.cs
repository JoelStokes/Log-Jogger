using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public float endPos;
    public bool startup;
    public string loadScene;
    public float moveSpeed = -30f;

    private bool valuesSet = false;

    void Start(){
        transform.SetParent(Camera.main.transform);
    }

    void Update()
    {
        if (startup || valuesSet){
            transform.position = new Vector3(transform.position.x + (moveSpeed * Time.deltaTime), transform.position.y, transform.position.z);

            if (transform.position.x < endPos){
                if (startup){
                    Destroy(gameObject);
                } else {
                    SceneManager.LoadScene(loadScene);
                }
            }
        }
    }

    public void SetValues(string scene, float x, float speed){
        loadScene = scene;
        endPos = x;
        moveSpeed = speed;
        valuesSet = true;
    }

    //Overload for scenes where transition is called with higher camera zoom for local Scale update to help transition consistency
    public void SetValues(string scene, float x, float speed, float localScaleChange){
        loadScene = scene;
        endPos = x;
        moveSpeed = speed;
        valuesSet = true;

        transform.localScale = new Vector3(localScaleChange, localScaleChange, transform.localScale.z);
    }
}
