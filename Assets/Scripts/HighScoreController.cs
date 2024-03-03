using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreController : MonoBehaviour
{
    public GameObject transitionPrefab;

    private float transitionX = 12;
    private float transitionXEnd = -9;

    public void MainMenu(){
        CreateTransition("Title");
    }

    private void CreateTransition (string scene){
        GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transitionX, transform.position.y, 0), Quaternion.identity);
        TransitionObj.GetComponent<Transition>().SetValues(scene, transitionXEnd);        
    }
}
