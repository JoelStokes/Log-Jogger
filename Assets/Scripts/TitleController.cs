using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public GameObject transitionPrefab;
    private float transitionX = 12;
    private float transitionXEnd = -9;

    public void Play(){
        CreateTransition("Level");
    }

    public void Tutorial(){
        CreateTransition("Tutorial");
    }

    public void HighScore(){
        CreateTransition("HighScore");
    }

    public void Settings(){
        //Replace buttons with Settings options
    }

    public void Quit(){
        Application.Quit();
    }

    private void CreateTransition (string scene){
        GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transitionX, 0, 0), Quaternion.identity);
        TransitionObj.GetComponent<Transition>().SetValues(scene, transitionXEnd);        
    }
}
